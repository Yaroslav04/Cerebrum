using Cerebrum.Core.View;
using Cerebrum.Core.ViewModel;

namespace Cerebrum;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
    }

}
