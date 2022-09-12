
namespace Cerebrum.Core.Servises
{
    public static class FileManager
    {
        public static bool IsDataPathExist()
        {
            return File.Exists(Path.Combine(AppPath(), "path.ini"));
            
        }

        public static string DataPath()
        {
            string path;
            using (StreamReader sr = new StreamReader(Path.Combine(AppPath(), "path.ini")))
            {
                path = sr.ReadToEnd();
            }

            return path;

            //@"D:\Servise\GoogleDrive\Cerebrum"
        }

        public static string DataPath(string _file)
        {
            return Path.Combine(DataPath(), _file);
        }

        public static string AppPath()
        {
            return FileSystem.Current.AppDataDirectory;
        }

        public static string AppPath(string _file)
        {
            return Path.Combine(FileSystem.Current.AppDataDirectory, _file);
        }

    }
}
