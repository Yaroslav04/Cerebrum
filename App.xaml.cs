using Cerebrum.Core.Servises;

namespace Cerebrum;

public partial class App : Application
{
    static DataBase dataBase;
    public static DataBase DataBase
    {
        get
        {
            if (dataBase == null)
            {
                dataBase = new DataBase(FileManager.AppPath(), new List<string> { "ObjectDataBase.db3", "TegDataBase.db3"});
            }
            return dataBase;
        }
    }

    public static List<string> Tegs = new List<string>
    {
        "ККУ(Особлива частина)", "судова справа", "ключове слово"
    };

    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
