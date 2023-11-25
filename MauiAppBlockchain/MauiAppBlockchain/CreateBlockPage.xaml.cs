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
                //Âûâîä èíôîğìàöèè ïğî ôàëüøèâûé áëîê
                var myError = await response.Content.ReadFromJsonAsync<BlocksData>();
                if (myError.Blocks is IList<Block> ruinblocks)
                    sendView.Text = $"Îøèáêà:\n{myError.Status}\nÂğåìÿ: {ruinblocks[0].Created}\nÄàííûå: {ruinblocks[0].Data}\n{ruinblocks[0].User}";
            }
        }
    }


}