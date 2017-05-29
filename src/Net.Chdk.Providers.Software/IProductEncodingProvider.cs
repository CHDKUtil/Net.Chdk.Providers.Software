using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public interface IProductEncodingProvider
    {
        SoftwareEncodingInfo GetEncoding(SoftwareProductInfo product, SoftwareCameraInfo camera);
    }
}
