using Cerebrum.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            TegItems = new ObservableCollection<TegClass>();
            SaveCommand = new Command(Save);
            ClearCommand = new Command(Clear);
            AddTegCommand = new Command(AddTeg);
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

        private string authorityPanel;
        public string AuthorityPanel
        {
            get => authorityPanel;
            set
            {
                SetProperty(ref authorityPanel, value);
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

        #endregion

        private async void Clear()
        {
            DescriptionPanel = null;
            IdentificationPanel = null;
            DocumentDatePanel = null;
            AuthorityPanel = null;
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
            await App.DataBase.SaveObjectAsync(objectClass);
        }

        private async void AddTeg()
        {
            TegItems.Add(
                new TegClass
                {
                    Key = "key", 
                    Value = ValueTeg
                }
                );
        }
    }
}
