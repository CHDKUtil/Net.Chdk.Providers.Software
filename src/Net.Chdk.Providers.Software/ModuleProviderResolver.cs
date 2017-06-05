using Microsoft.Extensions.Logging;
using Net.Chdk.Providers.Product;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Software
{
    sealed class ModuleProviderResolver : ProviderResolver<IModuleProvider>, IModuleProviderResolver
    {
        #region Fields

        private IProductProvider ProductProvider { get; }

        #endregion

        #region Constructor

        public ModuleProviderResolver(IProductProvider productProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ProductProvider = productProvider;
        }

        #endregion

        #region IModuleProviderResolver Members

        public IModuleProvider GetModuleProvider(string productName)
        {
            return GetProvider(productName);
        }

        #endregion

        #region Providers

        protected override IEnumerable<string> GetNames()
        {
            return ProductProvider.GetProductNames();
        }

        protected override IModuleProvider CreateProvider(string productName)
        {
            return new ModuleProvider(productName, LoggerFactory);
        }

        #endregion
    }
}
