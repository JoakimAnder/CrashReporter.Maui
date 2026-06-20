using CrashReporter.Maui.ExampleApp.Services;

namespace CrashReporter.Maui.ExampleApp;

public partial class AppShell : Shell
{
	public AppShell(IServiceProvider services)
	{
		InitializeComponent();
	}

	override void OnNavigated(ShellNavigatedEventArgs args)
	{
		base.OnNavigated(args);

		// I'm using the navigation event to trigger a snapshot update as <see cref="CustomSnapshotProvider"/> is a simple provider that just returns the current page, so we want to make sure to update it whenever we navigate to a new page.
		CustomSnapshotProvider.RefreshPage(args.Source?.GetType().Name ?? "Unknown");
	}
}
