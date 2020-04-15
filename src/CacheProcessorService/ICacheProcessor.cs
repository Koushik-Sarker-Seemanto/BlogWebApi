using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelsService.Models;
using StackExchange.Redis;

namespace CacheProcessorService
{
    public interface ICacheProcessor
    {
        /*Task<bool> UpdatePostAsync(string key, Post post, DateTime? expiry);
        Task<Post> FindPostAsync(string key);*/

        Task<bool> UpdatePostStringAsync(string key, Post post);
        Task<bool> DeletePostStringAsync(string key);
        Task<Post> FindPostStringAsync(string key);

        Task<bool> UpdateUserPostStringAsync(string key, List<Post> posts);
        Task<bool> InsertUserPostStringAsync(string key, Post post);
        Task<bool> UpdateUserPostStringAsync(string key, Post post);
        Task<bool> DeleteUserPostStringAsync(string key, Post post);
        Task<List<Post>> FindUserPostStringAsync(string key);
    }
}