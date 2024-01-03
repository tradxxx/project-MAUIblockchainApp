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

        collectionView.SelectionChanged += (sender, e) =>
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedElement = e.CurrentSelection[0] as Block; // Получите выбранный элемент
                DisplayFullInfoBlock(selectedElement);                                 // Выполните нужные действия с выбранным элементом
            }
        };

    }
    private async void DisplayFullInfoBlock(Block model)
    {
        await DisplayAlert($"Блок {model.Id}", $"Cумма: {model.Amount}\nОписание: {model.Description}\nВремя:{model.Date}\nКатегория: {model.Category.Title}\nПользователь: {model.User.Name}", "OK");
    }

    private async void OnButton2Clicked(object sender, System.EventArgs e)
    {
        var Mychain = await сonnectionService.GetBlocks();
        collectionView.ItemsSource = Mychain.Blocks;
        label2.Text = "База данных успешно загружена с веб-службы";


        labelIPHost.Text = await сonnectionService.GetIpHost();


    }

    private async void OnButtonClicked(object sender, System.EventArgs e)
    {
        // При нажатии кнопки производим переход на CreateBlockPage
        await Navigation.PushAsync(new CreateBlockPage(сonnectionService));
    }

    private async void OnButton3Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CreateCategoryPage(сonnectionService));
    }

    private async void OnButton4Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CreateUserPage(сonnectionService));
    }
}

