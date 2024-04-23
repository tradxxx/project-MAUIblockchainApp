using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class CreateCategoryPage : ContentPage
{
    private readonly СonnectionService сonnectionService;

    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }

    public CreateCategoryPage(СonnectionService сonnection)
    {
        InitializeComponent();

        сonnectionService = сonnection;
    }

    async Task InitializeAsync()
    {
        Categories = await сonnectionService.GetCategories();
        Users = await сonnectionService.GetUsers();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await InitializeAsync();

        //errorView.Text = string.Empty;
        //errorView.BackgroundColor = Colors.Transparent;
        //sendView.Text = string.Empty;
        //sendView.Background = Colors.Transparent;
    }
    private async void SendFromData(object sender, EventArgs e)
    {
        Category category = new Category
        {
            Title = TitleEntry.Text.Trim(),
            Tag = TagEntry.Text.Trim()
        };

        if (Categories.Any(c => c.Title == category.Title))
        {
            DisplayArtService.PrintLabelStatus(errorView, "Данная категория уже существует", LabelStatus.Error);
        }
        else
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await сonnectionService.CreateCategory(content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Успех", "Категория создана", "Ок");
                ResetForm();
            }
            else
            {

                DisplayArtService.PrintLabelStatus(errorView, "Ошибка отправки", LabelStatus.Error);

            }
        }
    }

    private void ResetForm()
    {
        // Очистка полей ввода
        TitleEntry.Text = string.Empty;
        TagEntry.Text = string.Empty;

        // Очистка других элементов управления (если необходимо)
        errorView.Text = string.Empty;
        errorView.BackgroundColor = Colors.Transparent;
    }
}