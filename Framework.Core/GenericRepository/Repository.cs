using Common.DTOs.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.GenericRepository
{
    public class Repository<TContext, TEntity, TKey> :
        BaseRepository<TContext, TEntity, TKey>, IBaseRepository<TContext, TEntity, TKey> where TContext : DbContext where TEntity : class, new()
    {
        public Repository(IConnectionStringsSettings connectionStringsSettings, IHttpContextAccessor accessor) : base(connectionStringsSettings, accessor)
        {
        }

        public override IBaseRepository<TContext, TEntity, TKey> Add(TEntity entity)
        {
            return base.Add(entity);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public override void ExecuteTransaction(Action action)
        {
            base.ExecuteTransaction(action);
        }

        public override TEntity GetSingle(TKey Id)
        {
            return base.GetSingle(Id);
        }

        public override IQueryable<TEntity> GetAll()
        {
            return base.GetAll();
        }

        public override IQueryable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return base.FindBy(predicate);
        }

        public override TEntity FisrtOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return base.FisrtOrDefault(predicate);
        }

        public override IBaseRepository<TContext, TEntity, TKey> Delete(TEntity entity)
        {
            return base.Delete(entity);
        }

        public override IBaseRepository<TContext, TEntity, TKey> Update(TEntity entity)
        {
            return base.Update(entity);
        }

        public override bool Exist(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return base.Exist(predicate);
        }

        public override bool ExistKey(TKey id)
        {
            return base.ExistKey(id);
        }
    }
}
