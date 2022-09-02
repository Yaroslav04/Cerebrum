using Cerebrum.Core.Model;
using Cerebrum.Core.Servises;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Formats.Asn1;
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
            DeleteCommand = new Command(Delete);

            ClearCommand = new Command(async () => await Clear());
            AddTegCommand = new Command(AddTeg);
            AddTypeCommand = new Command(AddType);
            AddAuthorityCommand = new Command(AddAuthority);
            AddFileCommand = new Command(async () => await AddFile());
            TegTappedCommand = new Command<TegClass>(TegTapped);
            FileTappedCommand = new Command<FileClass>(FileTapped);
            AuthorityItems = new ObservableCollection<string>();
            TypeItems = new ObservableCollection<string>();
            TegItems = new ObservableCollection<TegClass>();
            KeyTegItems = new ObservableCollection<string>(App.Tegs);
            FileItems = new ObservableCollection<FileClass>();

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
                Load(value);
            }
        }

        private string saveButtonName;
        public string SaveButtonName
        {
            get => saveButtonName;
            set
            {
                SetProperty(ref saveButtonName, value);
            }
        }

        private async void Load(string value)
        {
            await LoadAuthNType();

            if (value == "-1")
            {
                Title = "Додати картку";
                SaveButtonName = "Зберегти";
                await Clear();
            }
            else
            {
                Title = "Редагувати картку";
                SaveButtonName = "Оновити";
                await Clear();
                var item = await App.DataBase.GetObjectAsync(int.Parse(value));
                DescriptionPanel = item.Description;
                IdentificationPanel = item.Identification;
                DocumentDatePanel = item.DocumentDate.ToShortDateString();
                SelectedAuthority = item.Authority;
                SelectedType = item.Type;
                ContentPanel = item.Content;

                var tegs = await App.DataBase.GetTegsByIdAsync(int.Parse(value));
                if (tegs != null)
                {
                    foreach (var teg in tegs)
                    {
                        TegItems.Add(teg);
                    }
                }

                try
                {
                    var files = Directory.GetFiles(Path.Combine(FileManager.DataPath(), item.N.ToString()));
                    if (files.Length > 0)
                    {
                        foreach (var file in files)
                        {

                            FileItems.Add(new FileClass
                            {
                                Name = Path.GetFileName(file),
                                Path = file
                            });
                        }
                    }
                }
                catch
                {

                }
                
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

        private bool isFileDelete;
        public bool IsFileDelete
        {
            get => isFileDelete;
            set
            {
                SetProperty(ref isFileDelete, value);
            }
        }

        private bool isFileOpen;
        public bool IsFileOpen
        {
            get => isFileOpen;
            set
            {
                SetProperty(ref isFileOpen, value);
            }
        }

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
        public Command DeleteCommand { get; }

        public Command ClearCommand { get; }
        public Command AddTegCommand { get; }
        public Command AddTypeCommand { get; }
        public Command AddAuthorityCommand { get; }
        public Command AddFileCommand { get; }
        public Command<TegClass> TegTappedCommand { get; }
        public Command<FileClass> FileTappedCommand { get; }

        #endregion

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

        private async Task Clear()
        {
            await LoadAuthNType();
            FileItems.Clear();
            DescriptionPanel = null;
            IdentificationPanel = null;
            DocumentDatePanel = DateTime.Now.ToShortDateString();
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

            if (TegItems.Count == 0)
            {
                await Shell.Current.DisplayAlert("Помилка", $"Додайте тег", "ОК");
                return;
            }
            else
            {
                if (Id == "-1")
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
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message);
                                }

                                if (IsFileDelete)
                                {
                                    File.Delete(file.Path);
                                }
                            }
                        }
                    }
                }
                else
                {
                    objectClass.N = int.Parse(Id);
                    await App.DataBase.UpdateObjectAsync(objectClass);

                    var savedTegs = await App.DataBase.GetTegsByIdAsync(int.Parse(Id));
                    if (savedTegs != null)
                    {
                        foreach (var teg in savedTegs)
                        {
                            bool sw = false;

                            foreach (var item in TegItems)
                            {
                                if (teg.Key == item.Key)
                                {
                                    if (teg.Value == item.Value)
                                    {
                                        sw = true;
                                    }
                                }
                            }

                            if (sw == false)
                            {
                                await App.DataBase.DeleteTegAsync(teg);
                            }

                        }
                    }


                    foreach (var teg in TegItems)
                    {
                        if (teg.Id == -1)
                        {
                            teg.Id = int.Parse(Id);
                            await App.DataBase.SaveTegAsync(teg);
                        }
                    }
                }            
            }

            await Clear();
            await Shell.Current.DisplayAlert("Збережено", $"Документ збережено", "ОК");
            if (Id != "-1")
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private void AddTeg()
        {
            if (SelectedKeyTeg == null)
            {
                return;
            }
            else
            {
                if (ValueTeg == null)
                {
                    return;
                }
                else
                {
                    TegItems.Add(
                new TegClass
                {
                    Id = -1,
                    Key = SelectedKeyTeg,
                    Value = ValueTeg
                }
                );
                    ValueTeg = null;
                }
            }
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
                if (IsFileOpen)
                {
                    Process.Start("explorer.exe", file[0]);
                }
            }
        }

        public async Task<string[]> PickAndShow()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync();
                if (result != null)
                {
                    return new string[] { result.FullPath, result.FileName };
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void TegTapped(TegClass item)
        {
            if (item != null)
            {
                DeleteTeg(item);
            }
        }

        private async void DeleteTeg(TegClass item)
        {
            bool answer = await Shell.Current.DisplayAlert("Видалення", $"Видалити тег {item.Key} {item.Value}?", "Так", "Ні");
            if (answer)
            {
                TegItems.Remove(item);
            }
        }

        private void FileTapped(FileClass item)
        {
            if (item != null)
            {
                DeleteFile(item);
            }
        }

        private async void DeleteFile(FileClass item)
        {
            bool answer = await Shell.Current.DisplayAlert("Видалення", $"Видалити файл {item.Name}?", "Так", "Ні");
            if (answer)
            {
                FileItems.Remove(item);
            }
        }

        private async void Delete()
        {
            if (Id != "-1")
            {
                bool answer = await Shell.Current.DisplayAlert("Видалення", $"Видалити картку?", "Так", "Ні");
                if (answer)
                {
                    await App.DataBase.DeleteObjectAsync(await App.DataBase.GetObjectAsync(int.Parse(Id)));
                    try
                    {
                        Directory.Delete(Path.Combine(FileManager.DataPath(), Id), true);
                    }
                    catch
                    {

                    }
                   
                    await Shell.Current.GoToAsync("..");
                }

            }
        }

    }
}
