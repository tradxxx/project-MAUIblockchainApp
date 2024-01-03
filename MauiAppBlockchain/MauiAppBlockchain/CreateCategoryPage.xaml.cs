using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class CreateCategoryPage : ContentPage
{
    private readonly �onnectionService �onnectionService;

    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }

    public CreateCategoryPage(�onnectionService �onnection)
    {
        InitializeComponent();

        �onnectionService = �onnection;
    }

    async Task InitializeAsync()
    {
        Categories = await �onnectionService.GetCategories();
        Users = await �onnectionService.GetUsers();
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
            DisplayArtService.PrintLabelStatus(errorView, "������ ��������� ��� ����������", LabelStatus.Error);
        }
        else
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await �onnectionService.CreateCategory(content);

            if (response.IsSuccessStatusCode)
            {
                DisplayArtService.PrintLabelStatus(sendView, "�����", LabelStatus.Success);
            }
            else
            {

                DisplayArtService.PrintLabelStatus(errorView, "������ ��������", LabelStatus.Error);

            }
        }
    }
}