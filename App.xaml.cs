﻿using Cerebrum.Core.Servises;

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
                dataBase = new DataBase(FileManager.DataPath(), new List<string> { "ObjectDataBase.db3", "TegDataBase.db3"});
            }
            return dataBase;
        }
    }

    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
