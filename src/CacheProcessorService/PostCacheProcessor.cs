using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using ModelsService.Models;
using StackExchange.Redis;

namespace CacheProcessorService
{
    public class PostCacheProcessor : ICacheProcessor
    {
        private readonly IDatabase _redisDbContext;
        public PostCacheProcessor(IDatabase redisDbContext)
        {
            _redisDbContext = redisDbContext;
        }
        public async Task<bool> UpdatePostAsync(string key, Post post, DateTime? expiry)
        {
            if (post == null)
            {
                throw new ArgumentException("Provider was null");
            }

            try
            {
                expiry = expiry + TimeSpan.FromMinutes(2);
                byte[] serializedPost = SerializeDeserialize.Serialize(post);
                await this._redisDbContext.HashSetAsync(key, post.Id, serializedPost)
                    .ContinueWith(async t => await this._redisDbContext.KeyExpireAsync(key, expiry));
                return true;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public async Task<Post> FindPostAsync(string key)
        {
            var result = await this._redisDbContext.HashGetAsync(key, key, CommandFlags.None);
            return result.HasValue ? SerializeDeserialize.Deserialize<Post>(result) : default(Post);
        }
    }
}