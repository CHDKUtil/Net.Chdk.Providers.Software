using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Crypto;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Providers.Software
{
    sealed class SoftwareHashProvider : ISoftwareHashProvider
    {
        private IHashProvider HashProvider { get; }

        public SoftwareHashProvider(IHashProvider hashProvider)
        {
            HashProvider = hashProvider;
        }

        public SoftwareHashInfo GetHash(Stream stream, string fileName, string hashName)
        {
            return new SoftwareHashInfo
            {
                Name = hashName,
                Values = GetHashValues(stream, fileName, hashName)
            };
        }

        private Dictionary<string, string> GetHashValues(Stream stream, string fileName, string hashName)
        {
            var key = fileName.ToLowerInvariant();
            var value = HashProvider.GetHashString(stream, hashName);
            return new Dictionary<string, string>
            {
                { key, value }
            };
        }
    }
}
