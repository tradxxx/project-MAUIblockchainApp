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
    private readonly �onnectionService �onnectionService;
    public CreateBlockPage(�onnectionService �onnection)
    {
        InitializeComponent();
        �onnectionService = �onnection;
    }

    private async void SendFromData(object sender, EventArgs e)
    {
        Block block = new Block
        {
            Amount = Convert.ToDecimal(AmountEntry.Text),
            Description = DescriptionEntry.Text
        };

        var categories = await �onnectionService.GetCategories();
        var users = await �onnectionService.GetUsers();

        var category = categories.FirstOrDefault(x => x.Title == �ategoryEntry.Text);
        var user = users.FirstOrDefault(x => x.Name == UserEntry.Text);

        if (category == null)
        {
            DisplayArtService.PrintLabelStatus(errorView, "�� ������� ���������", LabelStatus.Error);
        }
        else if (user == null)
        {
            DisplayArtService.PrintLabelStatus(errorView, "�� ������ ������������", LabelStatus.Error);
            
        }
        else
        {
            block.CategoryId = category.Id;
            block.UserId = user.Id;

            if (ValidatorData.ValidateCheckData(block, errorView))
            {
                var json = JsonConvert.SerializeObject(block);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await �onnectionService.CreateBlock(content);

                if (response.IsSuccessStatusCode)
                {
                    sendView.Text = "�����";
                }
                else
                {

                    DisplayArtService.PrintLabelStatus(errorView, "������ ��������", LabelStatus.Error);

                    //����� ���������� ��� ��������� ����
                    var myErrorData = await response.Content.ReadFromJsonAsync<BlocksData>();

                    var ruinblocks = myErrorData.Blocks as IList<Block>;
                    sendView.FontFamily = "OpenSansSemibold";
                    sendView.Text = $"������ ������ � ����� �{ruinblocks[0].Id}:\n������: {myErrorData.Status}\n�����: {ruinblocks[0].Date}\n�����: {ruinblocks[0].Amount}\n��������: {ruinblocks[0].Date}\n���������: {ruinblocks[0].Category.Title}\n������������: {ruinblocks[0].User.Name}";


                }
            }

        }

       
    }


}