using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Transactions;

namespace Q.Infrastructure.Filters
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public static UnitOfWorkAttribute? GetUnitOfWorkAttribute(ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor is not ControllerActionDescriptor actionContext) return null;
            var uowAttribute = actionContext.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>()
                ?? actionContext.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            return uowAttribute;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            if (result.Exception is not null || context.HttpContext.Response.StatusCode >= 400) return;
            var uowAttribute = GetUnitOfWorkAttribute(context.ActionDescriptor);
            if (uowAttribute == null || uowAttribute.DbContextTypes.Length == 0) return;
            using TransactionScope txScope = new(TransactionScopeAsyncFlowOption.Enabled);
            // ToList的简化写法
            List<DbContext?> dbContextList = [.. uowAttribute.DbContextTypes.Select(t => context.HttpContext.RequestServices.GetService(t) as DbContext)];
            foreach (var dbContext in dbContextList)
            {
                if (dbContext is not null)
                    await dbContext.SaveChangesAsync();
            }
            txScope.Complete();
        }
    }
}
