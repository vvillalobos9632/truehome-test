using Common.DTOs.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Framework.Core.GenericRepository
{
    public interface IBaseRepository<TContext, TEntity, TKey> where TContext : DbContext where TEntity : class
    {
        TEntity GetSingle(TKey Id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        DbSet<TEntity> Entity { get; }
        IBaseRepository<TContext, TEntity, TKey> Add(TEntity entity);
        IBaseRepository<TContext, TEntity, TKey> AddIfNotExists(TEntity entity, Expression<Func<TEntity, bool>> predicate);
        IBaseRepository<TContext, TEntity, TKey> UpdateOrAdd(TEntity entity, Expression<Func<TEntity, bool>> predicate = null);
        IBaseRepository<TContext, TEntity, TKey> Delete(TEntity entity);
        IBaseRepository<TContext, TEntity, TKey> Update(TEntity entity);
        void AddCreateInfo(TEntity entity, TEntity origEntity);
        int SaveChanges();
        Task<int> SaveChangesAsync();

        void ExecuteTransaction(Action action);
        bool Exist(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
        bool ExistKey(TKey id);

        UserLoginData GetUserData();
    }
}
