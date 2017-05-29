using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Software
{
    sealed class ModulesProviderResolver : IModulesProviderResolver
    {
        #region Fields

        private IProductProvider ProductProvider { get; }
        private ILoggerFactory LoggerFactory { get; }

        #endregion

        #region Constructor

        public ModulesProviderResolver(IProductProvider productProvider, ILoggerFactory loggerFactory)
        {
            ProductProvider = productProvider;
            LoggerFactory = loggerFactory;

            providers = new Lazy<Dictionary<string, IModulesProvider>>(GetProviders);
        }

        #endregion

        #region IModulesProviderResolver Members

        public IModulesProvider GetModulesProvider(string productName)
        {
            IModulesProvider modulesProvider;
            Providers.TryGetValue(productName, out modulesProvider);
            return modulesProvider;
        }

        #endregion

        #region Providers

        private readonly Lazy<Dictionary<string, IModulesProvider>> providers;

        private Dictionary<string, IModulesProvider> Providers => providers.Value;

        private Dictionary<string, IModulesProvider> GetProviders()
        {
            return ProductProvider.GetProducts()
                .ToDictionary(p => p, CreateModulesProvider);
        }

        private IModulesProvider CreateModulesProvider(string productName)
        {
            return new ModulesProvider(productName, LoggerFactory);
        }

        #endregion
    }
}
