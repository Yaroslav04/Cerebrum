using Cerebrum.Core.Model;
using Cerebrum.Core.Servises;
using Cerebrum.Core.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerebrum.Core.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            SearchCommand = new Command(Search);
            ClearCommand = new Command(Clear);
            SingleItemTapped = new Command<ObjectSoketClass>(SingleTapped);
            DoubleItemTapped = new Command<ObjectSoketClass>(DoubleTapped);
            AddItemCommand = new Command(Add);
            EditItemCommand = new Command(Edit);
            OpenAppFolderCommand = new Command(OpenAppFolder);

            Items = new ObservableCollection<ObjectSoketClass>();
            SearchTypeItems = new ObservableCollection<string>
            {
                "Всюди", "Заголовок", "Ключове слово", "КК України", "Файли", 
            };
            AuthorityItems = new ObservableCollection<string>();
            TypeItems = new ObservableCollection<string>();
            SortItems = new ObservableCollection<string>
            {
                "Від нових до старих",
                "Від старих до нових",
                "Внесено за зростанням", 
                "Внесено за спаданням"
            };

            RunAsync();
        }

        private async void RunAsync()
        {
            await LoadAuthNType();
            await LoadItems();
        }

        #region Properties

        public ObservableCollection<ObjectSoketClass> Items { get; }

        private ObjectSoketClass selectedItem;
        public ObjectSoketClass SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);

            }
        }

        #region SearchPanel

        private string searchTextSearchPanel;
        public string SearchTextSearchPanel
        {
            get => searchTextSearchPanel;
            set
            {
                SetProperty(ref searchTextSearchPanel, value);
              
            }
        }

        private string searchTypeSearchPanel;
        public string SearchTypeSearchPanel
        {
            get => searchTypeSearchPanel;
            set
            {
                SetProperty(ref searchTypeSearchPanel, value);

            }
        }

        public ObservableCollection<string> SearchTypeItems { get; }

        private string authoritySearchPanel;
        public string AuthoritySearchPanel
        {
            get => authoritySearchPanel;
            set
            {
                SetProperty(ref authoritySearchPanel, value);

            }
        }

        public ObservableCollection<string> AuthorityItems { get; }

        private string typeSearchPanel;
        public string TypeSearchPanel
        {
            get => typeSearchPanel;
            set
            {
                SetProperty(ref typeSearchPanel, value);

            }
        }

        public ObservableCollection<string> TypeItems { get; }

        private string sortSearchPanel;
        public string SortSearchPanel
        {
            get => sortSearchPanel;
            set
            {
                SetProperty(ref sortSearchPanel, value);

            }
        }

        public ObservableCollection<string> SortItems { get; }

        #endregion

        #endregion

        #region Command

        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command EditItemCommand { get; }
        public Command<ObjectSoketClass> SingleItemTapped { get; }
        public Command<ObjectSoketClass> DoubleItemTapped { get; }
        public Command SearchCommand { get; }
        public Command ClearCommand { get; }
        public Command OpenAppFolderCommand { get; }

        #endregion

        void SingleTapped(ObjectSoketClass item)
        {

            if (item == null)
                return;          
        }

        void DoubleTapped(ObjectSoketClass item)
        {

            if (item == null)
                return;
            try
            {
                var files = Directory.GetFiles(Path.Combine(FileManager.DataPath(), item.N.ToString()));
                if (files.Length == 0)
                {
                    return;
                }
                else if (files.Length == 1)
                {
                    Process.Start("explorer.exe", files[0]);
                }
                else
                {
                    Process.Start("explorer.exe", Path.Combine(FileManager.DataPath(), item.N.ToString()));
                }
            }
            catch
            {

            }      
        }

        private async void Add()
        {
            await Shell.Current.GoToAsync($"{nameof(AddPage)}?{nameof(AddViewModel.Id)}={-1}");
        }

        private async void Edit()
        {
            if (SelectedItem != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AddPage)}?{nameof(AddViewModel.Id)}={SelectedItem.N}");
            }
        }

        private void Clear()
        {
            Items.Clear();
            SelectedItem = null;
            SearchTextSearchPanel = null;
            SearchTypeSearchPanel = null;
            AuthoritySearchPanel = null;
            TypeSearchPanel = null;
            SortSearchPanel = null;   
        }

        private async void Search()
        {
            await LoadItems();
        }

        private async Task LoadItems()
        {
            SelectedItem = null;
            Items.Clear();
            var items = await App.DataBase.GetObjectsAsync();

            if (items.Count == 0)
            {
                return;
            }
            else
            {

                if (SearchTextSearchPanel != null)
                {
                    if (SearchTypeSearchPanel != null)
                    {
                        var result = await SearchFromText(items, SearchTextSearchPanel, SearchTypeSearchPanel);
                        if (result == null)
                        {
                            return;
                        }
                        else
                        {
                            items.Clear();
                            items.AddRange(result);
                        }
                    }
                    else
                    {
                        var result = await SearchFromText(items, SearchTextSearchPanel, "Всюди");
                        if (result == null)
                        {
                            return;
                        }
                        else
                        {
                            items.Clear();
                            items.AddRange(result);
                        }
                    }          
                }

                if (SortSearchPanel != null)
                {
                    switch (SortSearchPanel)
                    {
                        case "Від нових до старих":
                            items = items.OrderBy(x => x.DocumentDate).ToList();
                            break;
                        case "Від старих до нових":
                            items = items.OrderByDescending(x => x.DocumentDate).ToList();
                            break;
                        case "Внесено за зростанням":
                            items = items.OrderBy(x => x.N).ToList();
                            break;
                        case "Внесено за спаданням":
                            items = items.OrderByDescending(x => x.N).ToList();
                            break;
                    }
                }

                if (AuthoritySearchPanel != null)
                {
                    items = items.Where(x => x.Authority == AuthoritySearchPanel).ToList();
                    if (items.Count == 0)
                    {
                        return;
                    }
                }

                if (TypeSearchPanel != null)
                {
                    items = items.Where(x => x.Type == TypeSearchPanel).ToList();
                    if (items.Count == 0)
                    {
                        return;
                    }
                }

                if (items.Count == 0)
                {
                    return;
                }
                else
                {

                    List<ObjectSoketClass> objectsList = new List<ObjectSoketClass>();
                    if (items == null)
                    {
                        return;
                    }
                    else
                    {
                        foreach (var item in items)
                        {
                            ObjectSoketClass objectSoketClass = new ObjectSoketClass(item);
                            var tegs = await App.DataBase.GetTegsByIdAsync(item.N);
                            if (tegs.Count > 0)
                            {
                                objectSoketClass.TegsList = tegs;
                                foreach (var teg in tegs)
                                {
                                    if (teg.Key == "ключове слово")
                                    {
                                        objectSoketClass.Tegs = objectSoketClass.Tegs + $"#{teg.Value}  ";
                                    }
                                    else if (teg.Key == "судова справа")
                                    {

                                    }
                                    else
                                    {
                                        objectSoketClass.Tegs = objectSoketClass.Tegs + $"#{teg.Value}{teg.Key.Replace("(Особлива частина)", "")}  ";
                                    }
                                }
                            }

                            objectsList.Add(objectSoketClass);
                        }
                    }

                    foreach (var item in objectsList)
                    {
                        Items.Add(item);
                    }
                }
            }
        }

        private async Task LoadAuthNType()
        {
            TypeItems.Clear();
            var types = await App.DataBase.GetTypes();
            if (types != null)
            {
                if (types.Count > 0)
                {
                    foreach (var item in types)
                    {
                        TypeItems.Add(item);
                    }
                }

            }

            AuthorityItems.Clear();
            var auth = await App.DataBase.GetAuthorities();
            if (auth != null)
            {
                if (auth.Count > 0)
                {
                    foreach (var item in auth)
                    {
                        AuthorityItems.Add(item);
                    }
                }

            }
        }

        private async Task<List<ObjectClass>> SearchFromText(List<ObjectClass> _list, string _text, string _parametr)
        {
            List<ObjectClass> list = new List<ObjectClass>();
            if (_list.Count > 0)
            {
                switch (_parametr)
                {
                    case "Всюди":

                        var resultAD = SearchInDescription(_list, _text);
                        if (resultAD != null)
                        {
                            list.AddRange(resultAD);
                        }

                        var resultAK = await SearchInKeyWord(_list, _text);
                        if (resultAK != null)
                        {
                            list.AddRange(resultAK);
                        }

                        var resultAC = await SearchInCC(_list, _text);
                        if (resultAC != null)
                        {
                            list.AddRange(resultAC);
                        }

                        list = list.Distinct().ToList();

                        return list;

                    case "Заголовок":
                        var resultD = SearchInDescription(_list, _text);
                        if (resultD != null)
                        {
                            list.AddRange(resultD);
                        }
                        return list;
                     

                    case "Ключове слово":
                        var resultK = await SearchInKeyWord(_list, _text);
                        if (resultK != null)
                        {
                            list.AddRange(resultK);
                        }
                        return list;

                    case "КК України":
                        var resultC = await SearchInCC(_list, _text);
                        if (resultC != null)
                        {
                            list.AddRange(resultC);
                        }
                        return list;

                    case "Файли":

                        break;
                }
            }
            return null;
        }

        private List<ObjectClass> SearchInDescription(List<ObjectClass> _list, string _text)
        {
            var list = _list.Where(x => x.Description.Contains(_text, StringComparison.OrdinalIgnoreCase)).ToList();
            return list;
        }

        private async Task<List<ObjectClass>> SearchInKeyWord(List<ObjectClass> _list, string _text)
        {
            List<ObjectClass> list = new List<ObjectClass>();
            foreach (var item in _list)
            {
                List<TegClass> tegs = new List<TegClass>();
                var t = await App.DataBase.GetTegsByIdAsync(item.N);
                if (t != null)
                {
                    tegs.AddRange(t);
                }

                bool sw = false;

                if (tegs.Count > 0)
                {

                    foreach (var teg in tegs)
                    {
                        if (teg.Key == "ключове слово")
                        {
                            if (teg.Value.Contains(_text.ToLower()))
                            {
                                sw = true;
                            }
                        }
                    }
                }

                if (sw == true)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        private async Task<List<ObjectClass>> SearchInCC(List<ObjectClass> _list, string _text)
        {
            List<ObjectClass> list = new List<ObjectClass>();
            foreach (var item in _list)
            {
                List<TegClass> tegs = new List<TegClass>();
                var t = await App.DataBase.GetTegsByIdAsync(item.N);
                if (t != null)
                {
                    tegs.AddRange(t);
                }

                bool sw = false;

                if (tegs.Count > 0)
                {

                    foreach (var teg in tegs)
                    {
                        if (teg.Key == "ККУ(Особлива частина)")
                        {
                            Debug.WriteLine(teg.Value);
                            if (teg.Value.Contains(_text))
                            {
                                sw = true;
                            }
                        }
                    }
                }

                if (sw == true)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        private void OpenAppFolder()
        {
            Process.Start("explorer.exe", FileManager.AppPath());
        }

    }
}
