using CrashReporter.Maui.ExampleApp.Services;
using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.ExampleApp;

public partial class MainPage : ContentPage
{
	private readonly IServiceProvider _services;

	private int _count = 0;

	public MainPage(IServiceProvider services)
	{
		_services = services;
		InitializeComponent();
	}

	private void OnCounterClicked(object? sender, EventArgs e)
	{
		_count++;

		if (_count == 1)
			CounterBtn.Text = $"Clicked {_count} time";
		else
			CounterBtn.Text = $"Clicked {_count} times";

		CustomSnapshotProvider.RefreshCount(_count);
	}

	private void OnCustomCrashClicked(object? sender, EventArgs e)
	{
		string message = $"This is a custom crash. Count: {_count}";
		CustomCrashReporter.ReportCrash(message);
	}

	private void OnNativeCrashClicked(object? sender, EventArgs e)
	{
		CrashHelper.NativeCrash();
	}

	private void OnCheckForCrashesClicked(object? sender, EventArgs e)
	{
		var crash = _services.CheckForCrash();
		if (crash is null)
		{
			DisplayAlert("No crash", "No crash found", "OK");
			return;
		}

	}
}
