using Microsoft.Extensions.Options;
using ASP.NET6WithRedisCache.Utilities;
using StackExchange.Redis;

namespace ASP.NET6WithRedisCache.Interface
{
    public sealed class RepositoryHelper: IRepositoryHelper
    {
        private readonly RedisCache cache;
        private readonly RepositoryOptions options;
        public RepositoryHelper(IOptions<RepositoryOptions> options, RedisCache cache)
        {          
            this.cache = cache;
            this.options = options.Value;
        }
        public IDatabase GetCacheDatabase(RedisDatabaseTarget target) => this.cache.Connection.GetDatabase(this.GetDatabaseId(target));
        private int GetDatabaseId(RedisDatabaseTarget target)
        {
            return target switch
            {
                RedisDatabaseTarget.Transient => this.options.RedisTransientDatabaseId,
                RedisDatabaseTarget.Persistent => this.options.RedisPersistentDatabaseId,
                _ => throw new NotSupportedException(),
            };
        }
    }
}
