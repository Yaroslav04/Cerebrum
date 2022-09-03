using Cerebrum.Core.ViewModel;

namespace Cerebrum.Core.View;

public partial class MainPage : ContentPage
{
    MainPageViewModel mainPageViewModel;
    public MainPage()
	{
		InitializeComponent();
		mainPageViewModel = new MainPageViewModel();
		BindingContext = mainPageViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        mainPageViewModel.OnAppearing();
    }
}