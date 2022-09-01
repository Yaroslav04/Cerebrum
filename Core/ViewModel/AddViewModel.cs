using Cerebrum.Core.Model;
using Cerebrum.Core.Servises;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Cerebrum.Core.ViewModel
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class AddViewModel : BaseViewModel
    {
        public AddViewModel()
        {
            SaveCommand = new Command(Save);
            ClearCommand = new Command(async () => await Clear());
            AddTegCommand = new Command(AddTeg);
            AddTypeCommand = new Command(AddType);
            AddAuthorityCommand = new Command(AddAuthority);
            AddFileCommand = new Command(async () => await AddFile());

            AuthorityItems = new ObservableCollection<string>();
            TypeItems = new ObservableCollection<string>();
            TegItems = new ObservableCollection<TegClass>();
            KeyTegItems = new ObservableCollection<string>(App.Tegs);
            FileItems = new ObservableCollection<FileClass>();

            Clear();

            Debug.WriteLine(FileManager.AppPath());

        }

        #region Properties

        private string id;
        public string Id
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
                //LoadCase(value);
            }
        }

        #region Teg
        public ObservableCollection<TegClass> TegItems { get; }

        private TegClass selectedTeg;
        public TegClass SelectedTeg
        {
            get => selectedTeg;
            set
            {
                SetProperty(ref selectedTeg, value);
            }
        }
        public ObservableCollection<string> KeyTegItems { get; }

        private string selectedKeyTeg;
        public string SelectedKeyTeg
        {
            get => selectedKeyTeg;
            set
            {
                SetProperty(ref selectedKeyTeg, value);
            }
        }

        private string valueTeg;
        public string ValueTeg
        {
            get => valueTeg;
            set
            {
                SetProperty(ref valueTeg, value);
            }
        }


        #endregion

        #region Files
        public ObservableCollection<FileClass> FileItems { get; }

        #endregion

        #region Panel
        public ObservableCollection<string> AuthorityItems { get; }
        public ObservableCollection<string> TypeItems { get; }

        private string descriptionPanel;
        public string DescriptionPanel
        {
            get => descriptionPanel;
            set
            {
                SetProperty(ref descriptionPanel, value);
            }
        }

        private string identificationPanel;
        public string IdentificationPanel
        {
            get => identificationPanel;
            set
            {
                SetProperty(ref identificationPanel, value);
            }
        }

        private string documentDatePanel;
        public string DocumentDatePanel
        {
            get => documentDatePanel;
            set
            {
                SetProperty(ref documentDatePanel, value);
            }
        }

        private string selectedAuthority;
        public string SelectedAuthority
        {
            get => selectedAuthority;
            set
            {
                SetProperty(ref selectedAuthority, value);
            }
        }

        private string selectedType;
        public string SelectedType
        {
            get => selectedType;
            set
            {
                SetProperty(ref selectedType, value);
            }
        }

        private string typePanel;
        public string TypePanel
        {
            get => typePanel;
            set
            {
                SetProperty(ref typePanel, value);
            }
        }

        private string contentPanel;
        public string ContentPanel
        {
            get => contentPanel;
            set
            {
                SetProperty(ref contentPanel, value);
            }
        }

        #endregion


        #endregion

        #region Command

        public Command SaveCommand { get; }
        public Command ClearCommand { get; }
        public Command AddTegCommand { get; }
        public Command AddTypeCommand { get; }
        public Command AddAuthorityCommand { get; }
        public Command AddFileCommand { get; }

        #endregion

        private async Task Clear()
        {
            var tegs = await App.DataBase.GetTegsAsync();
            foreach (var teg in tegs)
            {
                Debug.WriteLine(teg.Id);
            }


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
            FileItems.Clear();
            DescriptionPanel = null;
            IdentificationPanel = null;
            DocumentDatePanel = null;
            SelectedAuthority = null;
            SelectedType = null;
            ContentPanel = null;
            TegItems.Clear();
            ValueTeg = null;
            TegItems.Clear();
            SelectedKeyTeg = null;
        }

        private async void Save()
        {
            ObjectClass objectClass = new ObjectClass();
            objectClass.Description = DescriptionPanel;
            objectClass.Identification = IdentificationPanel;
            objectClass.DocumentDate = Convert.ToDateTime(DocumentDatePanel);
            objectClass.Authority = SelectedAuthority;
            objectClass.Type = SelectedType;
            objectClass.Content = ContentPanel;

            if (TegItems.Count < 2)
            {
                await Shell.Current.DisplayAlert("Помилка", $"Додайте не менше 3х тегів", "ОК");
                return;
            }
            else
            {
                await App.DataBase.SaveObjectAsync(objectClass);

                var index = await App.DataBase.GetLastOblectIndex();
                if (index != -1)
                {
                    foreach (var teg in TegItems)
                    {
                        TegClass tegClass = new TegClass();
                        tegClass.Id = index;
                        tegClass.Key = teg.Key;
                        tegClass.Value = teg.Value;
                        await App.DataBase.SaveTegAsync(tegClass);
                    }

                    if (FileItems.Count > 0)
                    {
                        try
                        {
                            Directory.CreateDirectory(FileManager.DataPath(index.ToString()));

                        }
                        catch
                        {

                        }

                        foreach (var file in FileItems)
                        {
                            try
                            {
                                File.Copy(file.Path, FileManager.DataPath(Path.Combine(index.ToString(), file.Name)));
                            }
                            catch(Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                }        
            }

            await Clear();
            await Shell.Current.DisplayAlert("Збережено", $"Документ збережено", "ОК");

        }

        private void AddTeg()
        {
            TegItems.Add(
                new TegClass
                {
                    Key = SelectedKeyTeg, 
                    Value = ValueTeg
                }
                );
        }

        private async void AddAuthority()
        {
            string result = await Shell.Current.DisplayPromptAsync("Додати видавника документу", $"Введіть орган який видав документ");
            if (String.IsNullOrWhiteSpace(result))
            {
                return;
            }
            else
            {
                AuthorityItems.Add(result);
                SelectedAuthority = result.Trim();
            }
        }

        private async void AddType()
        {
            string result = await Shell.Current.DisplayPromptAsync("Додати тип документу", $"Введіть тип документу");
            if (String.IsNullOrWhiteSpace(result))
            {
                return;
            }
            else
            {
                TypeItems.Add(result);
                SelectedType = result.Trim();
            }
        }

        private async Task AddFile()
        {
            var file = await PickAndShow();
            if (file != null)
            {
                FileItems.Add(new FileClass
                {
                    Name = file[1],
                    Path = file[0]
                });
                Process.Start("explorer.exe", file[0]);
            }
        }

        public async Task<string[]> PickAndShow()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {                  
                    return new string[] {result.FullPath, result.FileName };
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
