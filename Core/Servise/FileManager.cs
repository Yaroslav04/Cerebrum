
namespace Cerebrum.Core.Servises
{
    public static class FileManager
    {
        public static string DataPath()
        {
            return @"D:\CerebrumDB";
        }

        public static string DataPath(string _file)
        {
            return Path.Combine(@"D:\CerebrumDB", _file);
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
