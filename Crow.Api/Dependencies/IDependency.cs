using System.Threading.Tasks;

namespace Crow.Api.Dependencies
{
    public interface IDependency
    {
        string Repository { get; }
        bool HasReleases();
        void GetRelease(string id, string targetDirectory);
        Task GetReleaseAsync(string id, string targetDirectory);
        void DownloadFiles(string id, string targetDirectory);
        Task DownloadFilesAsync(string id, string targetDirectory);
    }
}