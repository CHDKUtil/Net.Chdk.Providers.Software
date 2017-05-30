using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Software
{
    sealed class ModuleProviderResolver : IModuleProviderResolver
    {
        #region Fields

        private IProductProvider ProductProvider { get; }
        private ILoggerFactory LoggerFactory { get; }

        #endregion

        #region Constructor

        public ModuleProviderResolver(IProductProvider productProvider, ILoggerFactory loggerFactory)
        {
            ProductProvider = productProvider;
            LoggerFactory = loggerFactory;

            providers = new Lazy<Dictionary<string, IModuleProvider>>(GetProviders);
        }

        #endregion

        #region IModuleProviderResolver Members

        public IModuleProvider GetModuleProvider(string productName)
        {
            IModuleProvider moduleProvider;
            Providers.TryGetValue(productName, out moduleProvider);
            return moduleProvider;
        }

        #endregion

        #region Providers

        private readonly Lazy<Dictionary<string, IModuleProvider>> providers;

        private Dictionary<string, IModuleProvider> Providers => providers.Value;

        private Dictionary<string, IModuleProvider> GetProviders()
        {
            return ProductProvider.GetProducts()
                .ToDictionary(p => p, CreateModuleProvider);
        }

        private IModuleProvider CreateModuleProvider(string productName)
        {
            return new ModuleProvider(productName, LoggerFactory);
        }

        #endregion
    }
}
