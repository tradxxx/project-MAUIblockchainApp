using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using MauiAppBlockchain.Service;
using MauiAppBlockchain.Models;

namespace MauiAppBlockchain;

public partial class MainPage : ContentPage
{
	private readonly СonnectionService сonnectionService;
   
	public MainPage(СonnectionService сonnection)
	{
		InitializeComponent();

        сonnectionService = сonnection;

        collectionView.SelectionChanged += (sender, e) => {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedElement = e.CurrentSelection[0] as Block; // Получите выбранный элемент
				DisplayFullInfoBlock(selectedElement);                                 // Выполните нужные действия с выбранным элементом
            }
        };

    }
    private async void DisplayFullInfoBlock(Block model)
    {
		await DisplayAlert($"Блок {model.Id}",$"Данные: {model.Data}\nПользователь: {model.User}", "OK");
    }
    
    private async void OnButton2Clicked(object sender, System.EventArgs e)
	{
		string request = await сonnectionService.GetBlocks();
		var Mychain = JsonConvert.DeserializeObject<BlocksData>(request);
        collectionView.ItemsSource = Mychain.Blocks;
		label2.Text = "База данных успешно загружена с веб-службы";

		string Host = System.Net.Dns.GetHostName();
		string IP = System.Net.Dns.GetHostByName(Host).AddressList[0].ToString();
		label1.Text = Host + "  " + IP;

	}

	private async void OnButtonClicked(object sender, System.EventArgs e)
	{
        // При нажатии кнопки производим переход на CreateBlockPage
        await Navigation.PushAsync(new CreateBlockPage(сonnectionService));
	}

}

