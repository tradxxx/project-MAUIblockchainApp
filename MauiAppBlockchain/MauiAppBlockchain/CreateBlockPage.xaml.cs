using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;
using static MauiAppBlockchain.MainPage;

namespace MauiAppBlockchain;

public partial class CreateBlockPage : ContentPage
{
    private readonly СonnectionService сonnectionService;
    public CreateBlockPage(СonnectionService сonnection)
    {
        InitializeComponent();
        сonnectionService = сonnection;
    }

    private async void SendFromData(object sender, EventArgs e)
    {
        Block block = new Block
        {
            Amount = Convert.ToDecimal(AmountEntry.Text),
            Description = DescriptionEntry.Text
        };

        var categories = await сonnectionService.GetCategories();
        var users = await сonnectionService.GetUsers();

        var category = categories.FirstOrDefault(x => x.Title == СategoryEntry.Text);
        var user = users.FirstOrDefault(x => x.Name == UserEntry.Text);

        if (category == null)
        {
            DisplayArtService.PrintLabelStatus(errorView, "Не найдена категория", LabelStatus.Error);
        }
        else if (user == null)
        {
            DisplayArtService.PrintLabelStatus(errorView, "Не найден пользователь", LabelStatus.Error);
            
        }
        else
        {
            block.CategoryId = category.Id;
            block.UserId = user.Id;

            if (ValidatorData.ValidateCheckData(block, errorView))
            {
                var json = JsonConvert.SerializeObject(block);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await сonnectionService.CreateBlock(content);

                if (response.IsSuccessStatusCode)
                {
                    sendView.Text = "Успех";
                }
                else
                {

                    DisplayArtService.PrintLabelStatus(errorView, "Ошибка отправки", LabelStatus.Error);

                    //Вывод информации про фальшивый блок
                    var myErrorData = await response.Content.ReadFromJsonAsync<BlocksData>();

                    var ruinblocks = myErrorData.Blocks as IList<Block>;
                    sendView.FontFamily = "OpenSansSemibold";
                    sendView.Text = $"Ошибка данных в блоке №{ruinblocks[0].Id}:\nСтатус: {myErrorData.Status}\nВремя: {ruinblocks[0].Date}\nСумма: {ruinblocks[0].Amount}\nОписание: {ruinblocks[0].Date}\nКатегория: {ruinblocks[0].Category.Title}\nПользователь: {ruinblocks[0].User.Name}";


                }
            }

        }

       
    }


}