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
            Data = DataEntry.Text,
            Created = DateTime.Now,
            Hash = string.Empty,
            PreviousHash = string.Empty,
            User = UserEntry.Text
        };

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
                sendView.Text = $"������ ������ � ����� �{ruinblocks[0].Id}:\n������: {myErrorData.Status}\n�����: {ruinblocks[0].Created}\n������: {ruinblocks[0].Data}\n������������: {ruinblocks[0].User}";


            }
        }
    }


}