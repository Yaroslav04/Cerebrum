
namespace Cerebrum.Core.Servises
{
    public static class FileManager
    {
        public static string DataPath()
        {
            return @"/storage/emulated/0/Proviser2/";
        }

        public static string DataPath(string _file)
        {
            return Path.Combine(@"/storage/emulated/0/Proviser2/", _file);
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
