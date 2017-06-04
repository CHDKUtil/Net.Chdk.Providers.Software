using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Software
{
    sealed class FileSystemProvider : IFileSystemProvider
    {
        private IEnumerable<IProductFileSystemProvider> FileSystemProviders { get; }

        public FileSystemProvider(IEnumerable<IProductFileSystemProvider> fileSystemProviders)
        {
            FileSystemProviders = fileSystemProviders;
        }

        public string GetBootFileSystem(SoftwareProductInfo product, CameraInfo camera)
        {
            return FileSystemProviders
                .Select(p => p.GetBootFileSystem(product, camera))
                .FirstOrDefault(f => f != null);
        }
    }
}
