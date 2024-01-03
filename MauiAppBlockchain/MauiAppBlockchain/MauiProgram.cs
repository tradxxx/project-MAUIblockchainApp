using MauiAppBlockchain.Service;
using Microsoft.Extensions.Logging;

namespace MauiAppBlockchain;

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
			});
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.43.175:5153/") });
        builder.Services.AddScoped<СonnectionService>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<CreateBlockPage>();
        builder.Services.AddSingleton<CreateCategoryPage>();
        builder.Services.AddSingleton<CreateUserPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
