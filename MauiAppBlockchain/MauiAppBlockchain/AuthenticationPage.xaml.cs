using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class AuthenticationPage : ContentPage
{
    private readonly СonnectionService сonnectionService;
    public AuthenticationPage(СonnectionService сonnection)
	{
		InitializeComponent();
        сonnectionService = сonnection;

    }

    private async void LoginButton_Clicked(object sender, EventArgs e)
    {
        // Получаем введенные учетные данные
        string username = LoginEntry.Text.Trim();
        string password = PasswordEntry.Text.Trim();

        // Выполняем проверку учетных данных (здесь вам нужно реализовать свою логику)
        bool isAuthenticated = await AuthenticateUser(username, password);

        if (isAuthenticated)
        {
            // Закрываем модальное окно после успешной авторизации
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        else
        {
            // Отображаем сообщение об ошибке
            await DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
        }
    }

    private async Task<bool> AuthenticateUser(string username, string password)
    {
        try
        {
            // Создаем объект для тела запроса
            var request = new User
            {
                Name = username,
                Password = password
            };

            // Отправляем POST-запрос на API для авторизации
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await сonnectionService.Authentication(content);

            // Проверяем успешный ответ от API
            if (response.IsSuccessStatusCode)
            {
                // Авторизация прошла успешно
                return true;
            }
            else
            {
                // Обрабатываем ошибку авторизации
                var errorMessage = await response.Content.ReadAsStringAsync();
                // Здесь вы можете отобразить сообщение об ошибке или обработать ее другим способом
                Console.WriteLine($"Ошибка авторизации: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            // Обрабатываем исключения, возникшие при отправке запроса
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // Авторизация не прошла
        return false;
    }
}