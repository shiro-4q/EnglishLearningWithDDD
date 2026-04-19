using Q.DomainCommons.Models;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore
{
    public static class EFCoreExtensions
    {
        /// <summary>
        /// set global 'IsDeleted=false' queryfilter for every entity
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void EnableSoftDeletionGlobalFilter(this ModelBuilder modelBuilder)
        {
            var entityTypesHasSoftDeletion = modelBuilder.Model.GetEntityTypes()
                .Where(e => e.ClrType.IsAssignableTo(typeof(ISoftDelete)));

            foreach (var entityType in entityTypesHasSoftDeletion)
            {
                var isDeletedProperty = entityType.FindProperty(nameof(ISoftDelete.IsDeleted));
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var filter = Expression.Lambda(Expression.Not(Expression.Property(parameter, isDeletedProperty.PropertyInfo)), parameter);
                entityType.SetQueryFilter(filter);
            }
        }

        public static IQueryable<T> Query<T>(this DbContext ctx) where T : BaseEntity
        {
            return ctx.Set<T>().AsNoTracking();// 不跟踪的查询，EFCore 不会记录实体状态，性能更高，但是也不能修改或删除
        }
    }
}
