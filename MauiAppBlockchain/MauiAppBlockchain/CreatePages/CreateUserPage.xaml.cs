using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class CreateUserPage : ContentPage
{
    private readonly ÑonnectionService ñonnectionService;

    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }
    public CreateUserPage(ÑonnectionService ñonnection)
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
        User user = new User
        {
            Name = NameEntry.Text.Trim()
        };

        if (Users.Any(u => u.Name == user.Name))
        {
            DisplayArtService.PrintLabelStatus(errorView, "Äàííûé ïîëüçîâàòåëü óæå ñóùåñòâóåò", LabelStatus.Error);
        }
        else
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await ñonnectionService.CreateUser(content);

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