using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModelsService.Models;
using StackExchange.Redis;

namespace CacheProcessorService
{
    public interface ICacheProcessor
    {
        Task<bool> UpdatePostAsync(string key, Post post, DateTime? expiry);
        Task<Post> FindPostAsync(string key);
    }
}