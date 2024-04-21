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
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://192.168.43.175/") });
        builder.Services.AddScoped<СonnectionService>();
        builder.Services.AddScoped<App>();
        builder.Services.AddScoped<MainPage>();
        builder.Services.AddScoped<CreateBlockPage>();
        builder.Services.AddScoped<CreateCategoryPage>();
        builder.Services.AddScoped<CreateUserPage>();
        builder.Services.AddScoped<SearchBlockPage>();
		builder.Services.AddScoped<AuthenticationPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
