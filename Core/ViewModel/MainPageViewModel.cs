using Cerebrum.Core.Model;
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
            Items = new ObservableCollection<ObjectSoketClass>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SingleItemTapped = new Command<ObjectSoketClass>(SingleTapped);
            DoubleItemTapped = new Command<ObjectSoketClass>(DoubleTapped);
            AddItemCommand = new Command(Add);

            RunAsync();
        }

        private async void RunAsync()
        {
            //test

            var items = await App.DataBase.GetObjectsAsync();
            foreach(var item in items)
            {
                Items.Add(new ObjectSoketClass(item));
            }  

        }


        #region Properties

        public ObservableCollection<ObjectSoketClass> Items { get; }

        #endregion

        #region Command

        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<ObjectSoketClass> SingleItemTapped { get; }
        public Command<ObjectSoketClass> DoubleItemTapped { get; }

        #endregion

        async Task ExecuteLoadItemsCommand()
        {
            
        }

        async void SingleTapped(ObjectSoketClass item)
        {

            if (item == null)
                return;

            //await Shell.Current.GoToAsync($"{nameof(CasePage)}?{nameof(CaseViewModel.CaseId)}={item.Case}");
        }

        async void DoubleTapped(ObjectSoketClass item)
        {

            if (item == null)
                return;

            //await Shell.Current.GoToAsync($"{nameof(CasePage)}?{nameof(CaseViewModel.CaseId)}={item.Case}");
        }

        private async void Add()
        {
            await Shell.Current.GoToAsync($"{nameof(AddPage)}?{nameof(AddViewModel.Id)}={-1}");
        }


    }
}
