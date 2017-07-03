using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Software
{
    sealed class SourceProvider : ISourceProvider
    {
        private IEnumerable<IProductSourceProvider> ProductSourceProviders { get; }

        public SourceProvider(IEnumerable<IProductSourceProvider> productSourceProviders)
        {
            ProductSourceProviders = productSourceProviders;
        }

        public IEnumerable<Tuple<string, string, SoftwareSourceInfo>> GetSources(CategoryInfo category)
        {
            return ProductSourceProviders
                .SelectMany(p => GetSources(p, category));
        }

        public IEnumerable<Tuple<string, string, SoftwareSourceInfo>> GetSources(SoftwareProductInfo product)
        {
            return ProductSourceProviders
                .SelectMany(p => GetSources(p, product));
        }

        private static IEnumerable<Tuple<string, string, SoftwareSourceInfo>> GetSources(IProductSourceProvider provider, CategoryInfo category)
        {
            return provider
                .GetSources(category)
                .Select(kvp => Tuple.Create(provider.ProductName, kvp.Key, kvp.Value));
        }

        private static IEnumerable<Tuple<string, string, SoftwareSourceInfo>> GetSources(IProductSourceProvider provider, SoftwareProductInfo product)
        {
            return provider
                .GetSources(product)
                .Select(kvp => Tuple.Create(provider.ProductName, kvp.Key, kvp.Value));
        }

        public IEnumerable<SoftwareSourceInfo> GetSources(SoftwareProductInfo product, string sourceName)
        {
            return ProductSourceProviders
                .SelectMany(p => p.GetSources(product, sourceName));
        }
    }
}
