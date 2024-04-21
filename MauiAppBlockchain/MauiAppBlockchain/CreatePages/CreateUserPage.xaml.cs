using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class CreateUserPage : ContentPage
{
    private readonly СonnectionService сonnectionService;

    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }
    public CreateUserPage(СonnectionService сonnection)
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
        User user = new User
        {
            Name = NameEntry.Text.Trim(),
            Password = PasswordEntry.Text.Trim(),
        };

        if (Users.Any(u => u.Name == user.Name))
        {
            DisplayArtService.PrintLabelStatus(errorView, "Данный пользователь уже существует", LabelStatus.Error);
        }
        else
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await сonnectionService.CreateUser(content);

            if (response.IsSuccessStatusCode)
            {

                await DisplayAlert("Успех", "Добавлен новый пользователь", "Ок");
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
        NameEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;

        // Очистка других элементов управления (если необходимо)
        errorView.Text = string.Empty;
        errorView.BackgroundColor = Colors.Transparent;
    }
}