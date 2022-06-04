using System.IO;
using TrzyszczCMS.TrzyszczCMS.Core.Server.Services.Interfaces;

namespace TrzyszczCMS.TrzyszczCMS.Core.Server.Services.Implementation
{
    public class FileHandler : IFileFacade
    {
        public bool Exists(string path) =>
            File.Exists(path);
        
        public Stream OpenReadAsStream(string path) =>
            File.OpenRead(path);
        
        public Stream GetFileStreamAsStream(string path, FileMode mode, FileAccess access) =>
            new FileStream(path, mode, access);

        public void Delete(string path) =>
            File.Delete(path);
    }
}
