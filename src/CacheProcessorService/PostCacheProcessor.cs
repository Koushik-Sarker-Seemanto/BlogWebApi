using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly TimeSpan _expiryTime;
        public PostCacheProcessor(IDatabase redisDbContext)
        {
            _redisDbContext = redisDbContext;
            _expiryTime = TimeSpan.FromMinutes(10);
        }
        /*public async Task<bool> UpdatePostAsync(string key, Post post, DateTime? expiry)
        {
            if (post == null)
            {
                throw new ArgumentException("Provider was null");
            }

            try
            {
                expiry = expiry + _expiryTime;
                byte[] serializedPost = SerializeDeserialize.Serialize(post);
                await this._redisDbContext.HashSetAsync(key, post.Id, serializedPost)
                    .ContinueWith(async t => await this._redisDbContext.KeyExpireAsync(key, expiry));
                return true;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }*/
        public async Task<bool> UpdatePostStringAsync(string key, Post post)
        {
            if (post == null)
            {
                throw new ArgumentException("Provider was null");
            }

            try
            {
                byte[] serializedPost = SerializeDeserialize.Serialize(post);
                await this._redisDbContext.StringSetAsync(key, serializedPost, _expiryTime, When.Always,
                    CommandFlags.None);
                return true;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public async Task<bool> DeletePostStringAsync(string key)
        {
            try
            {
                await this._redisDbContext.KeyDeleteAsync(key);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateUserPostStringAsync(string key, List<Post> posts)
        {
            var newKey = $"user_post: {key}";
            if (posts.Count == 0)
            {
                return false;
            }
            byte[] serializedPost = SerializeDeserialize.Serialize(posts);
            await this._redisDbContext.StringSetAsync(newKey, serializedPost, _expiryTime, When.Always,
                CommandFlags.None);
            //await this._redisDbContext.SetAddAsync(newKey, serializedPost, CommandFlags.None);
            
            return true;
        }
        public async Task<bool> InsertUserPostStringAsync(string key, Post post)
        {
            var newKey = $"user_post: {key}";
            if (post == null)
            {
                return false;
            }

            var existing= await this.FindUserPostStringAsync(key);
            if (existing == null)
            {
                return false;
            }
            existing.Add(post);
            byte[] serializedPost = SerializeDeserialize.Serialize(existing);
            await this._redisDbContext.StringSetAsync(newKey, serializedPost, _expiryTime, When.Always,
                CommandFlags.None);
            
            return true;
        }
        public async Task<bool> UpdateUserPostStringAsync(string key, Post post)
        {
            var newKey = $"user_post: {key}";
            if (post == null)
            {
                return false;
            }

            var existing= await this.FindUserPostStringAsync(key);
            var exist = existing?.FirstOrDefault(e => e.Id == post.Id);
            if (exist == null)
            {
                return false;
            }

            var idx = existing.IndexOf(exist);
            existing[idx] = post;
            byte[] serializedPost = SerializeDeserialize.Serialize(existing);
            await this._redisDbContext.StringSetAsync(newKey, serializedPost, _expiryTime, When.Always,
                CommandFlags.None);
            
            return true;
        }
        
        public async Task<bool> DeleteUserPostStringAsync(string key, Post post)
        {
            var newKey = $"user_post: {key}";
            if (post == null)
            {
                return false;
            }

            var existing= await this.FindUserPostStringAsync(key);

            var exist = existing?.FirstOrDefault(e => e.Id == post.Id);
            if (exist == null)
            {
                return false;
            }
            existing.Remove(exist);
            byte[] serializedPost = SerializeDeserialize.Serialize(existing);
            await this._redisDbContext.StringSetAsync(newKey, serializedPost, _expiryTime, When.Always,
                CommandFlags.None);
            
            return true;
        }

        /*public async Task<Post> FindPostAsync(string key)
        {
            var result = await this._redisDbContext.HashGetAsync(key, key, CommandFlags.None);
            return result.HasValue ? SerializeDeserialize.Deserialize<Post>(result) : default(Post);
        }*/
        public async Task<Post> FindPostStringAsync(string key)
        {
            var result = await this._redisDbContext.StringGetAsync(key, CommandFlags.None);
            return result.HasValue ? SerializeDeserialize.Deserialize<Post>(result) : null;
        }
        public async Task<List<Post>> FindUserPostStringAsync(string key)
        {
            var newKey = $"user_post: {key}";
            var result = await this._redisDbContext.StringGetAsync(newKey, CommandFlags.None);
            return result.HasValue ? SerializeDeserialize.Deserialize<List<Post>>(result) : null;
        }
        
    }
}