using Microsoft.Extensions.Logging;
using Net.Chdk.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Providers.Software
{
    sealed class ModuleProvider : IModuleProvider
    {
        #region Constants

        private const string DataPath = "Data";
        private const string ProductPath = "Product";
        private const string DataFileName = "components.json";

        #endregion

        #region Fields

        private ILogger Logger { get; }

        #endregion

        #region Constructor

        public ModuleProvider(string productName, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<ModuleProvider>();
            ProductName = productName;

            data = new Lazy<ComponentsData>(GetData);
            modules = new Lazy<Dictionary<string, ModuleData>>(GetModules);
            moduleNames = new Lazy<Dictionary<string, string>>(GetModuleNames);
        }

        #endregion

        #region IModuleProvider Members

        public string Path => Data.Modules.Path;

        public string Extension => Data.Modules.Extension;

        public string GetModuleName(string filePath)
        {
            string moduleName;
            ModuleNames.TryGetValue(filePath, out moduleName);
            return moduleName;
        }

        public string GetModuleTitle(string name)
        {
            ModuleData module;
            if (!Modules.TryGetValue(name, out module))
                return null;
            return module.Title;
        }

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
            public IDictionary<string, ModuleData> Children { get; set; }
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

        #region Modules

        private readonly Lazy<Dictionary<string, ModuleData>> modules;

        private Dictionary<string, ModuleData> Modules => modules.Value;

        private Dictionary<string, ModuleData> GetModules()
        {
            Dictionary<string, ModuleData> modules = new Dictionary<string, ModuleData>();
            GetModules(Data.Modules.Children, modules);
            return modules;
        }

        private void GetModules(IDictionary<string, ModuleData> children, Dictionary<string, ModuleData> modules)
        {
            if (children != null)
            {
                foreach (var kvp in children)
                {
                    var name = kvp.Key;
                    var module = kvp.Value;
                    if (name.Length > 0)
                        modules.Add(name, module);
                    GetModules(module.Children, modules);
                }
            }
        }

        #endregion

        #region ModuleNames

        private readonly Lazy<Dictionary<string, string>> moduleNames;

        private Dictionary<string, string> ModuleNames => moduleNames.Value;

        private Dictionary<string, string> GetModuleNames()
        {
            var moduleNames = new Dictionary<string, string>();
            GetModuleNames(Data.Modules.Children, moduleNames);
            return moduleNames;
        }

        private static void GetModuleNames(IDictionary<string, ModuleData> modules, Dictionary<string, string> moduleNames)
        {
            if (modules != null)
            {
                foreach (var kvp in modules)
                {
                    var files = kvp.Value.Files;
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            moduleNames.Add(file.ToLowerInvariant(), kvp.Key);
                        }
                    }
                    GetModuleNames(kvp.Value.Children, moduleNames);
                }
            }
        }

        #endregion
    }
}
