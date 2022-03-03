using Common.DTOs.Settings;
using Common.DTOs.Users;
using Common.Utils.Utils;
using Framework.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Text;

namespace Framework.Core.GenericRepository
{
    public abstract class BaseRepository<TContext, TEntity, Tkey> :
IBaseRepository<TContext, TEntity, Tkey> where TContext : DbContext where TEntity : class, new()
    {
        private readonly IConnectionStringsSettings _connectionStringsSettings;
        private readonly IHttpContextAccessor _httpAccessor;

        public BaseRepository(IConnectionStringsSettings connectionStringsSettings, IHttpContextAccessor accessor)
        {
            _connectionStringsSettings = connectionStringsSettings;
            _httpAccessor = accessor;

            if (Context == null)
                Context = GetConnection();

        }

        public UserLoginData GetUserData()
        {
            if (_httpAccessor.HttpContext == null)
                return new UserLoginData();

            return JsonExtensions.DeserializeFromJson<UserLoginData>(_httpAccessor.HttpContext.User?.Identity?.Name);
        }

        private TContext GetConnection()
        {
            var connectionstring = _connectionStringsSettings[typeof(TContext).Name].DesencriptarTexto(true);

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            optionsBuilder.EnableSensitiveDataLogging(true);
            //optionsBuilder.UseLoggerFactory(_loggerFactory);

            optionsBuilder.UseNpgsql(connectionstring);
            DbContextOptions<TContext> opt = optionsBuilder.Options;
            return (TContext)Activator.CreateInstance(typeof(TContext), opt);
        }

        public TContext Context { get; set; }

        public DbSet<TEntity> Entity
        {
            get
            {
                return Context.Set<TEntity>();
            }
        }

        public List<Func<SqlDataReader, IEnumerable>> _resultSets;

        public virtual TEntity GetSingle(Tkey Id)
        {
            using (var context = GetConnection())
            {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                return context.Set<TEntity>().Find(Id);
            }
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking();
            return query;
        }

        public virtual IQueryable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>().AsNoTracking().Where(predicate);
            return query;
        }

        public virtual TEntity FisrtOrDefault(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            TEntity query = Context.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
            return query;
        }

        public virtual IBaseRepository<TContext, TEntity, Tkey> Add(TEntity entity)
        {
            var dateToProcess = DateTime.Now;
            AddCreateInfo(entity, dateToProcess);
            AddEditInfo(entity, dateToProcess);
            Context.Set<TEntity>().Add(entity);
            return this;
        }

        public IBaseRepository<TContext, TEntity, Tkey> AddIfNotExists(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            var exists = predicate != null ? Entity.Any(predicate) : Entity.Any();
            if (!exists)
                Add(entity);
            return this;
        }

        public IBaseRepository<TContext, TEntity, Tkey> UpdateOrAdd(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            var origEntity = predicate != null ? Entity.AsNoTracking().FirstOrDefault(predicate) : null;

            if (origEntity != null)
            {
                AddCreateInfo(entity, origEntity);
                return Update(entity);
            }
            else
                return Add(entity);
        }

        public virtual IBaseRepository<TContext, TEntity, Tkey> Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);

            return this;
        }

        public virtual IBaseRepository<TContext, TEntity, Tkey> Update(TEntity entity)
        {
            AddEditInfo(entity, DateTime.Now);
            Context.Entry(entity).State = EntityState.Modified;
            return this;
        }

        public virtual bool Exist(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AsNoTracking().Any(predicate);
        }

        public virtual bool ExistKey(Tkey id)
        {
            return (GetSingle(id) != null);
        }

        public virtual int SaveChanges()
        {
            var rowAffected = 0;

            try
            {
                var validationContext = new ValidationContext(this);
                Validator.ValidateObject(this, validationContext);

                rowAffected = Context.SaveChanges();
                return rowAffected;
            }
            catch (Exception ex)
            {
                var exceptionsDb = new StringBuilder();
                throw new Exception(exceptionsDb.ToString(), ex);
            }
        }

        public async virtual Task<int> SaveChangesAsync()
        {
            Task<int> rowAffected;
            try
            {
                var validationContext = new ValidationContext(this);
                Validator.ValidateObject(this, validationContext);

                rowAffected = Context.SaveChangesAsync();
                return rowAffected.Result;
            }
            catch (Exception ex)
            {
                var exceptionsDb = new StringBuilder();
                throw new Exception(exceptionsDb.ToString(), ex);
            }
        }

        public virtual void ExecuteTransaction(Action action)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Database.SetCommandTimeout(int.MaxValue);
                    action();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Transaction Error", ex);
                }
            }

        }

        private void AddCreateInfo(TEntity entity, DateTime dateToProcess)
        {
            var propieties = ExtensionMethods.getPropertiesObject(entity);
            var propietieCreate = propieties.ToList().Find(x => x.Name.ToUpper() == "USUARIOREGISTRO");
            var propietieFCreate = propieties.ToList().Find(x => x.Name.ToUpper() == "FECHAREGISTRO");

            if (propietieCreate != null)
                propietieCreate.SetValue(entity, GetUserData().NombreUsuario);
            //propietieCreate.SetValue(entity, CurrentSession.Usuario.ToUpper());

            if (propietieFCreate != null)
                propietieFCreate.SetValue(entity, dateToProcess);
        }

        public void AddCreateInfo(TEntity entity, TEntity origEntity)
        {
            var propieties = ExtensionMethods.getPropertiesObject(origEntity);
            var propietieCreate = propieties.ToList().Find(x => x.Name.ToUpper() == "USUARIOACTUALIZACION");
            var propietieFCreate = propieties.ToList().Find(x => x.Name.ToUpper() == "FECHAACTUALIZACION");

            if (propietieCreate != null)
                propietieCreate.SetValue(entity, propietieCreate.GetValue(origEntity));

            if (propietieFCreate != null)
                propietieFCreate.SetValue(entity, propietieFCreate.GetValue(origEntity));
        }

        private void AddEditInfo(TEntity entity, DateTime processDate)
        {
            var propieties = ExtensionMethods.getPropertiesObject(entity);
            var propietieModifico = propieties.ToList().Find(x => x.Name.ToUpper() == "USUARIOACTUALIZACION");
            var propietieFModifico = propieties.ToList().Find(x => x.Name.ToUpper() == "FECHAACTUALIZACION");

            if (propietieModifico != null)
                propietieModifico.SetValue(entity, GetUserData().NombreUsuario);

            if (propietieFModifico != null)
                propietieFModifico.SetValue(entity, processDate);
        }

       
    }

}
