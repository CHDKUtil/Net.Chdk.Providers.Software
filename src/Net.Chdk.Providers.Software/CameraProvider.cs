using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Software
{
    sealed class CameraProvider : ICameraProvider
    {
        private IEnumerable<IProductCameraProvider> CameraProviders { get; }

        public CameraProvider(IEnumerable<IProductCameraProvider> cameraProviders)
        {
            CameraProviders = cameraProviders;
        }

        public SoftwareCameraInfo GetCamera(string productName, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo)
        {
            return CameraProviders
                .Select(p => p.GetCamera(productName, cameraInfo, cameraModelInfo))
                .FirstOrDefault(c => c != null);
        }

        public SoftwareEncodingInfo GetEncoding(SoftwareProductInfo productInfo, SoftwareCameraInfo cameraInfo)
        {
            return CameraProviders
                .Select(p => p.GetEncoding(productInfo, cameraInfo))
                .FirstOrDefault(c => c != null);
        }
    }
}
