using Cerebrum.Core.ViewModel;

namespace Cerebrum.Core.View;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainPageViewModel();
	}
}