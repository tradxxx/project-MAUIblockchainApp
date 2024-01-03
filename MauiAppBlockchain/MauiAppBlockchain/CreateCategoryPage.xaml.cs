using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class CreateCategoryPage : ContentPage
{
    private readonly ÑonnectionService ñonnectionService;

    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }

    public CreateCategoryPage(ÑonnectionService ñonnection)
    {
        InitializeComponent();

        ñonnectionService = ñonnection;
    }

    async Task InitializeAsync()
    {
        Categories = await ñonnectionService.GetCategories();
        Users = await ñonnectionService.GetUsers();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await InitializeAsync();
    }
    private async void SendFromData(object sender, EventArgs e)
    {
        Category category = new Category
        {
            Title = TitleEntry.Text.Trim(),
            Icon = IconEntry.Text.Trim()
        };

        if (Categories.Any(c => c.Title == category.Title))
        {
            DisplayArtService.PrintLabelStatus(errorView, "Äàííàÿ êàòåãîğèÿ óæå ñóùåñòâóåò", LabelStatus.Error);
        }
        else
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await ñonnectionService.CreateCategory(content);

            if (response.IsSuccessStatusCode)
            {
                DisplayArtService.PrintLabelStatus(sendView, "Óñïåõ", LabelStatus.Success);
            }
            else
            {

                DisplayArtService.PrintLabelStatus(errorView, "Îøèáêà îòïğàâêè", LabelStatus.Error);

            }
        }
    }
}