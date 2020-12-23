using Microsoft.Extensions.Caching.Distributed;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace TimeNotes.Infrastructure.Cache
{
    public sealed class RedisCache
    {
        private readonly IDistributedCache _cache;
        private readonly IFormatter _binaryFormatter;
        public RedisCache(IDistributedCache cache)
        {
            _cache = cache;
            _binaryFormatter = new BinaryFormatter();
        }

        public async Task SetAsync(string key, object value, byte expiresInMinutes = 1)
            => await _cache.SetAsync(key, ToByteArray(value), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiresInMinutes) });

        public async Task<TValue> GetAsync<TValue>(string key)
            => (TValue)FromByteArray(await _cache.GetAsync(key));

        public async Task RemoveAsync(string key)
            => await _cache.RemoveAsync(key);

        public async Task<bool> ContainsKeyAsync(string key)
            => (await _cache.GetAsync(key)) != null;
 
        private byte[] ToByteArray(object value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                _binaryFormatter.Serialize(stream, value);
                return stream.ToArray();
            }
        }

        private object FromByteArray(byte[] value)
        {
            if (value is null) return null;

            using MemoryStream memoryStream = new MemoryStream(value);
            return _binaryFormatter.Deserialize(memoryStream);
        }
    }
}
