using System.Threading.Tasks;

namespace Crow.Dependencies
{
    public interface IDependency
    {
        bool HasReleases();
        void GetRelease(string id, string targetDirectory);
        Task GetReleaseAsync(string id, string targetDirectory);
        void DownloadFiles(string id, string targetDirectory);
        Task DownloadFilesAsync(string id, string targetDirectory);
    }
}