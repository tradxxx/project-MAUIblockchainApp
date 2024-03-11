using MauiAppBlockchain.Service;

namespace MauiAppBlockchain;

public partial class App : Application
{
    private readonly СonnectionService сonnectionService;
    public App(СonnectionService сonnection)
	{
		InitializeComponent();
        сonnectionService = сonnection;
        MainPage = new AuthenticationPage(сonnectionService);
	}
}
