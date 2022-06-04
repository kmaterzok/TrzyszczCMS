using System.IO;

namespace TrzyszczCMS.TrzyszczCMS.Core.Server.Services.Interfaces
{
    /// <summary>
    /// The interface of instance that handles files, especially using <see cref="System.IO.File"/> methods.
    /// </summary>
    public interface IFileFacade
    {
        bool Exists(string path);

        Stream OpenReadAsStream(string path);
        
        Stream GetFileStreamAsStream(string path, FileMode mode, FileAccess access);

        void Delete(string path);
    }
}
