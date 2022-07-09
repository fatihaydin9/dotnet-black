using Black.Database.Base.RepositoryBase.Abstract;
using Black.Database.Base.UnitOfWorkBase.Concrete;
using Black.Domain.Base;
using Black.Infrastructure.Extensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using SqlKata;
using SqlKata.Compilers;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Black.Database.RepositoryBase.Concrete;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase, new()
{
    #pragma warning disable CS8618  //SETTINGS
    private readonly DbContext _context; //db-context-relational
    private readonly string _tableName;  //table-name-relational
    public GenericRepository(DbContext context) => _context = context; //relational-db-context
    public GenericRepository(string tableName) => _tableName = tableName; //relational-db-tableName
    private readonly MongoDbContext _mongoDbContext = new MongoDbContext(); //nosql-mongo-context
    private readonly FilterDefinitionBuilder<TEntity> _filterBuilder = Builders<TEntity>.Filter; //nosql-mongo-filter
    private IMongoCollection<TEntity> dbCollection; //nosql-mongo-collection
    #pragma warning restore CS8618



    #region QueryBase [Dapper]
    public virtual async Task<TEntity> FindByIdAsync(long id)
    {
        TEntity item = default(TEntity);
        using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            var compiler = new SqlServerCompiler();
            var queryBuilder = new Query(_tableName).Where("Id", id).ToQueryString();
            item = (await connection.QueryAsync<TEntity>(queryBuilder)).SingleOrDefault();
        }
        return item;
    }
    public virtual async Task<TEntity> FindByGuidIdAsync(Guid guidId)
    {
        TEntity item = default(TEntity);
        using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            var compiler = new SqlServerCompiler();
            var queryBuilder = new Query(_tableName).Where("GuidId", guidId).ToQueryString();
            item = (await connection.QueryAsync<TEntity>(queryBuilder)).SingleOrDefault();
        }
        return item;
    }
    public virtual async Task<IQueryable<TEntity>> FindAllAsync()
    {
        IQueryable<TEntity> item = default(IQueryable<TEntity>);
        using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            var compiler = new SqlServerCompiler();
            var queryBuilder = new Query(_tableName).WhereNotNull("Id").ToQueryString();
            var executer = (await connection.QueryAsync<TEntity>(queryBuilder));
        }
        return item;
    }
    public async Task ExecuteAsync(string rawQuery, TEntity entity)
    {
        using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            await connection.ExecuteAsync(rawQuery, entity);
        }
    }
    #endregion QueryBase

    #region CommandBase [Entity Framework]
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        return entity;
    }
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        await Task.Run(() => { _context.Set<TEntity>().Update(entity); });
        return entity;
    }
    public async Task DeleteAsync(long id)
    {
        var entity = await FindByIdAsync(id);
        _context.Set<TEntity>().Remove(entity);
    }
    public async Task DeleteAsync(Guid guidId)
    {
        var entity = await FindByGuidIdAsync(guidId);
        _context.Set<TEntity>().Remove(entity);
    }
    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<IEnumerable<TEntity>>().AddRangeAsync(entities);
    }
    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _context.Set<IEnumerable<TEntity>>().UpdateRange(entities);
    }
    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _context.Set<IEnumerable<TEntity>>().RemoveRange(entities);
    }
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().AnyAsync(predicate);
    }
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().CountAsync(predicate);
    }
    public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.ToListAsync();
    }
    public async Task<TEntity> GetOnlyAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.SingleOrDefaultAsync();
    }
    #endregion CommandBase

    #region NoSQLBase [MongoDb]
    public async Task<IList<TEntity>> GetAllAsyncNoSql()
    {
        var finder = await dbCollection.FindAsync(_filterBuilder.Empty);
        return finder.ToList();
    }
    public async Task<TEntity> GetByGuidIdNoSql(Guid guidId)
    {
        FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.GuidId, guidId);
        var finder = await dbCollection.FindAsync(filter);
        return await finder.FirstOrDefaultAsync();
    }
    public async Task<TEntity> GetByIdNoSql(long id)
    {
        FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.Id, id);
        var finder = await dbCollection.FindAsync(filter);
        return await finder.FirstOrDefaultAsync();
    }
    public async Task CreateAsyncNoSql(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentException(nameof(entity));
        }

        string collectionName = nameof(TEntity);
        dbCollection = _mongoDbContext.GetMongoDatabase().GetCollection<TEntity>(collectionName);
        await dbCollection.InsertOneAsync(entity);

    }
    public async Task UpdateAsyncNoSql(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentException(nameof(entity));
        }

        FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.Id, entity.Id);
        await dbCollection.ReplaceOneAsync(filter, entity);
    }
    public async Task RemoveAsyncNoSql(Guid guidId)
    {
        FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.GuidId, guidId);
        await dbCollection.DeleteOneAsync(filter);
    }
    public async Task RemoveAsyncNoSql(long id)
    {
        FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.Id, id);
        await dbCollection.DeleteOneAsync(filter);
    }
    public async Task CreateManyNoSql(IList<TEntity> entityCollection)
    {
        if (entityCollection == null)
        {
            throw new ArgumentException(nameof(entityCollection));
        }

        string collectionName = nameof(TEntity);
        dbCollection = _mongoDbContext.GetMongoDatabase().GetCollection<TEntity>(collectionName);
        await dbCollection.InsertManyAsync(entityCollection);

    }
    public async Task UpdateManyNoSql(IList<TEntity> entityCollection)
    {
        if (entityCollection == null)
        {
            throw new ArgumentException(nameof(entityCollection));
        }
        foreach (var entity in entityCollection)
        {
            FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }
    }
    public async Task RemoveManyNoSql(List<Guid> guidIdList)
    {
        foreach (var guidId in guidIdList)
        {
            FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.GuidId, guidId);
            await dbCollection.DeleteManyAsync(filter);
        }
    }
    public async Task RemoveAsyncNoSql(List<long> idList)
    {
        foreach (var id in idList)
        {
            FilterDefinition<TEntity> filter = _filterBuilder.Eq(e => e.Id, id);
            await dbCollection.DeleteManyAsync(filter);
        }
    }
    #endregion

}