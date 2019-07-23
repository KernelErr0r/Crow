using System;
using System.IO;
using System.Threading.Tasks;

namespace Crow.Dependencies
{
    public class LocalDependency : IDependency
    {
        public string Repository { get; }

        private string filePath;

        public LocalDependency(string filePath, string repository)
        {
            this.filePath = filePath;
            this.Repository = repository;
        }

        public void DownloadFiles(string id, string targetDirectory)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var path = fileNameWithoutExtension + (String.IsNullOrWhiteSpace(id) ? "" : $"-{id}" + extension);

            File.Copy(path, Path.Combine(targetDirectory, path));
        }

        public async Task DownloadFilesAsync(string id, string targetDirectory)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var path = fileNameWithoutExtension + (String.IsNullOrWhiteSpace(id) ? "" : $"-{id}" + extension);

            await Task.Run(() =>
            {
                File.Copy(path, Path.Combine(targetDirectory, path));
            });
        }

        public void GetRelease(string id, string targetDirectory)
        {
            DownloadFiles(id, targetDirectory);
        }

        public async Task GetReleaseAsync(string id, string targetDirectory)
        {
            await DownloadFilesAsync(id, targetDirectory);
        }

        public bool HasReleases()
        {
            return File.Exists(filePath);
        }
    }
}