using Net.Chdk.Json;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Net.Chdk.Providers.Software
{
    sealed class ModulesProvider : IModulesProvider
    {
        #region Constants

        private const string DataPath = "Data";
        private const string ProductPath = "Product";
        private const string DataFileName = "components.json";

        #endregion

        #region Constructor

        public ModulesProvider(string productName)
        {
            ProductName = productName;

            data = new Lazy<ComponentsData>(GetData);
        }

        #endregion

        #region IModulesProvider Members

        public string Path => Data.Modules.Path;

        public string Extension => Data.Modules.Extension;

        #endregion

        #region Serializer

        private static readonly Lazy<JsonSerializer> serializer = new Lazy<JsonSerializer>(GetSerializer);

        private static JsonSerializer Serializer => serializer.Value;

        private static JsonSerializer GetSerializer()
        {
            return JsonSerializer.CreateDefault();
        }

        #endregion

        #region Data

        sealed class ModulesData
        {
            public string Path { get; set; }
            public string Extension { get; set; }
        }

        sealed class ComponentsData
        {
            public ModulesData Modules { get; set; }
        }

        private readonly Lazy<ComponentsData> data;

        private ComponentsData Data => data.Value;

        private string ProductName { get; }

        private ComponentsData GetData()
        {
            var filePath = System.IO.Path.Combine(DataPath, ProductPath, ProductName, DataFileName);
            using (var reader = File.OpenText(filePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return Serializer.Deserialize<ComponentsData>(jsonReader);
            }
        }

        private static JsonSerializerSettings Settings => new JsonSerializerSettings
        {
            Converters = new[] { new HexStringJsonConverter() }
        };

        #endregion
    }
}
