using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface IProductFileSystemProvider
    {
        string GetBootFileSystem(SoftwareProductInfo product, CameraInfo camera);
    }
}