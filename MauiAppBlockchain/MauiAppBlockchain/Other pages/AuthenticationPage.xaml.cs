using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MauiAppBlockchain;

public partial class AuthenticationPage : ContentPage
{
    private readonly �onnectionService �onnectionService;
    public AuthenticationPage(�onnectionService �onnection)
	{
		InitializeComponent();
        �onnectionService = �onnection;

    }
    private async void OutLoginButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("��������������", "������������ �� �������������!\n��������� ������� ���������� ����������.", "��");
        Application.Current.MainPage = new AppShell();
    }
    
    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        // �������� ��������� ������� ������
        string username = LoginEntry.Text.Trim();
        string password = PasswordEntry.Text.Trim();

        // ��������� �������� ������� ������ (����� ��� ����� ����������� ���� ������)
        bool isAuthenticated = await AuthenticateUser(username, password);

        if (isAuthenticated)
        {
            // ��������� ��������� ���� ����� �������� �����������
            //await Application.Current.MainPage.Navigation.PopModalAsync();
            Application.Current.MainPage = new AppShell();
        }
        else
        {
            // ���������� ��������� �� ������
            await DisplayAlert("������", "�������� ����� ��� ������", "OK");
        }
    }

    private async Task<bool> AuthenticateUser(string username, string password)
    {
        try
        {
            // ������� ������ ��� ���� �������
            var request = new User
            {
                Name = username,
                Password = password
            };

            // ���������� POST-������ �� API ��� �����������
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await �onnectionService.Authentication(content);

            // ��������� �������� ����� �� API
            if (response.IsSuccessStatusCode)
            {
                // �������� JSON-�����
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = System.Text.Json.JsonSerializer.Deserialize<AuthResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // ���������� �������������� ��������� � ������ � ����� ������������
                string welcomeMessage = $"����� ����������, {authResponse.Name} ({authResponse.Role})!";
                await Application.Current.MainPage.DisplayAlert("�������� �����������", welcomeMessage, "OK");
                // ����������� ������ �������
                return true;
            }
            else
            {
                // ������������ ������ �����������
                var errorMessage = await response.Content.ReadAsStringAsync();
                // ����� �� ������ ���������� ��������� �� ������ ��� ���������� �� ������ ��������
                Console.WriteLine($"������ �����������: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            // ������������ ����������, ��������� ��� �������� �������
            Console.WriteLine($"������: {ex.Message}");
        }

        // ����������� �� ������
        return false;
    }

    // ��������������� ����� ��� �������������� JSON-������
    public class AuthResponse
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }
}