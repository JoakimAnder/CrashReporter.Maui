using Microsoft.Extensions.Logging;

namespace CrashReporter.Maui.ExampleApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			// I moved all the interesting registration to it's own class, but you can also register reporters and handlers here if you want.
			// See <see cref="AppBuilderExtensions"/> for more information.
			.UseCrashHandling();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
