using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Software
{
    sealed class EncodingProvider : IEncodingProvider
    {
        private IEnumerable<IProductEncodingProvider> EncodingProviders { get; }

        public EncodingProvider(IEnumerable<IProductEncodingProvider> encodingProviders)
        {
            EncodingProviders = encodingProviders;
        }

        public SoftwareEncodingInfo GetEncoding(SoftwareProductInfo product, SoftwareCameraInfo camera)
        {
            return EncodingProviders
                .Select(p => p.GetEncoding(product, camera))
                .FirstOrDefault(c => c != null);
        }
    }
}
