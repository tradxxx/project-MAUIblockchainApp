using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;
using static MauiAppBlockchain.MainPage;

namespace MauiAppBlockchain;

public partial class CreateBlockPage : ContentPage
{
    private readonly ÑonnectionService ñonnectionService;
    public CreateBlockPage(ÑonnectionService ñonnection)
    {
        InitializeComponent();
        ñonnectionService = ñonnection;
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
            var response = await ñonnectionService.CreateBlock(content);

            if (response.IsSuccessStatusCode)
            {
                sendView.Text = "Óñïåõ";
            }
            else
            {

                DisplayArtService.PrintLabelStatus(errorView, "Îøèáêà îòïğàâêè", LabelStatus.Error);

                //Âûâîä èíôîğìàöèè ïğî ôàëüøèâûé áëîê
                var myErrorData = await response.Content.ReadFromJsonAsync<BlocksData>();

                var ruinblocks = myErrorData.Blocks as IList<Block>;
                sendView.FontFamily = "OpenSansSemibold";
                sendView.Text = $"Îøèáêà äàííûõ â áëîêå ¹{ruinblocks[0].Id}:\nÑòàòóñ: {myErrorData.Status}\nÂğåìÿ: {ruinblocks[0].Created}\nÄàííûå: {ruinblocks[0].Data}\nÏîëüçîâàòåëü: {ruinblocks[0].User}";


            }
        }
    }


}