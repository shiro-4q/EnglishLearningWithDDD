using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;
using System.Reflection;

namespace Q.Infrastructure.Filters;

public class AutoValidationFilter : IAsyncActionFilter
{
    // 使用线程安全的字典进行缓存MethodInfo
    // 因为MethodInfo是没有状态并且和Type是一一对应的，所以可以缓存
    private static readonly ConcurrentDictionary<Type, MethodInfo> _validateMethodCache = new();

    private static MethodInfo GetValidateAsyncMethod(Type modelType, Type validatorType)
    {
        return _validateMethodCache.GetOrAdd(modelType, t =>
        {
            return validatorType.GetMethod(nameof(IValidator<>.ValidateAsync), [t, typeof(CancellationToken)])!;
        });
    }


    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 获取所有需要验证的参数
        var values = context.ActionArguments.Values.Where(v => v != null && v.GetType().IsDefined(typeof(AutoValidationAttribute), true));
        if (context.ActionDescriptor is not ControllerActionDescriptor || !values.Any())
        {
            await next();
            return;
        }

        var serviceProvider = context.HttpContext.RequestServices;
        var cancellation = context.HttpContext.RequestAborted;
        foreach (var model in values)
        {
            var modelType = model!.GetType();

            // 获取IValidator<T>
            var validatorType = typeof(IValidator<>).MakeGenericType(modelType);
            if (validatorType is null) continue;

            // 通过注入获取validator实例
            if (serviceProvider.GetService(validatorType) is not IValidator validator) continue;

            // 通过反射获取ValidateAsync方法
            var validateAsyncMethod = GetValidateAsyncMethod(modelType, validatorType);
            var task = (Task<ValidationResult>)validateAsyncMethod.Invoke(validator, [model, cancellation])!;
            var result = await task;

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        await next();
    }
}
