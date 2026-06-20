using Microsoft.Extensions.DependencyInjection;

namespace CrashReporter.Maui.ExampleApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	override async void OnStart()
	{
		base.OnStart();

		// Initialize the crash handling system. This will run all the initialization code and also check if there are any unhandled crashes from previous runs that needs to be handled.
		await this.InitializeCrashHandling();
	}
}