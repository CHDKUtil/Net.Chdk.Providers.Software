using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface IProductCameraProvider
    {
        SoftwareCameraInfo GetCamera(SoftwareProductInfo productInfo, CameraInfo cameraInfo, CameraModelInfo cameraModelInfo);
    }
}
