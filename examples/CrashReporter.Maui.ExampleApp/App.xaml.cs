using Microsoft.Extensions.DependencyInjection;

namespace CrashReporter.Maui.ExampleApp;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell(_serviceProvider));
	}

	protected override async void OnStart()
	{
		base.OnStart();

		// Initialize the crash handling system. This will run all the initialization code and also check if there are any unhandled crashes from previous runs that needs to be handled.
		await _serviceProvider.InitializeCrashHandling();
	}
}