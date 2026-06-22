using CrashReporter.Maui.ExampleApp.Services;
using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.ExampleApp;

public partial class MainPage : ContentPage
{
	private readonly ICrashManager _crashManager;
	private readonly ISnapshotCollector _snapshotCollector;

	private int _count;

	public MainPage(ICrashManager crashManager, ISnapshotCollector snapshotCollector)
	{
		_crashManager = crashManager;
		_snapshotCollector = snapshotCollector;
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
		CustomCrashReporter.ReportCrash(message, _snapshotCollector);
	}

	private void OnNativeCrashClicked(object? sender, EventArgs e)
	{
		CrashHelper.NativeCrash();
	}

	private async void OnCheckForCrashesClicked(object? sender, EventArgs e)
	{
		var crash = await _crashManager.Consume(CancellationToken.None);
		if (crash is null)
		{
			await DisplayAlertAsync("No crash", "No crash found", "OK");
			return;
		}

		var selected = await DisplayActionSheetAsync("Crash found", "Cancel", destruction: null, "Send to handlers");
		if (!string.Equals(selected, "Send to handlers"))
			return;

		await _crashManager.HandleCrash(crash, CancellationToken.None);
		await DisplayAlertAsync("Crash handeled", crash.Type, "OK");

	}
}
