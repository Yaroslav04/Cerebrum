using Cerebrum.Core.ViewModel;

namespace Cerebrum.Core.View;

public partial class AddPage : ContentPage
{
	public AddPage()
	{
		InitializeComponent();
		BindingContext = new AddViewModel();
	}
}