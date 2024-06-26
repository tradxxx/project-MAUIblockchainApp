using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class CreateUserPage : ContentPage
{
    private readonly �onnectionService �onnectionService;

    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }
    public CreateUserPage(�onnectionService �onnection)
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
            DisplayArtService.PrintLabelStatus(errorView, "������ ������������ ��� ����������", LabelStatus.Error);
        }
        else
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await �onnectionService.CreateUser(content);

            if (response.IsSuccessStatusCode)
            {

                await DisplayAlert("�����", "�������� ����� ������������", "��");
                ResetForm();
            }
            else
            {

                DisplayArtService.PrintLabelStatus(errorView, "������ ��������", LabelStatus.Error);

            }
        }
    }
    private void ResetForm()
    {
        // ������� ����� �����
        NameEntry.Text = string.Empty;
        PasswordEntry.Text = string.Empty;

        // ������� ������ ��������� ���������� (���� ����������)
        errorView.Text = string.Empty;
        errorView.BackgroundColor = Colors.Transparent;
    }
}