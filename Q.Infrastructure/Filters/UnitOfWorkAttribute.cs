using Microsoft.EntityFrameworkCore;

namespace Q.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {
        public Type[] DbContextTypes { get; init; }

        public UnitOfWorkAttribute(params Type[] dbContextTypes)
        {
            DbContextTypes = dbContextTypes;

            foreach (var dbContextType in dbContextTypes)
            {
                if (!typeof(DbContext).IsAssignableFrom(dbContextType))
                {
                    throw new ArgumentException($"Type {dbContextType.FullName} is not a DbContext");
                }
            }
        }
    }
}
