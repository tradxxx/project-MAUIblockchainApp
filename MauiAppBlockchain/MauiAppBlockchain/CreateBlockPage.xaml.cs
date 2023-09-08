using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;
using static MauiAppBlockchain.MainPage;

namespace MauiAppBlockchain;

public partial class CreateBlockPage : ContentPage
{
	public CreateBlockPage()
	{
		InitializeComponent();
    }

	private async void SendFromData(object sender, EventArgs e)
	{
		Block block = new Block
		{
			Data = DataEntry.Text.Trim(),
			Created = DateTime.Now,
			Hash = string.Empty,
			PreviousHash = string.Empty,
            User = UserEntry.Text.Trim()
        };
       
		

		HttpClient client = new HttpClient();
		var json = JsonConvert.SerializeObject(block);
		var content = new StringContent(json, Encoding.UTF8, "application/json");
		var response = await client.PostAsync("http://192.168.0.12:5153/api/Chain", content);

		if (response.IsSuccessStatusCode)
		{
			trigger.Text = "Успех";
		}
		else
		{
            var myError = await response.Content.ReadFromJsonAsync<BlocksData>();
			if(myError.Blocks is IList<Block> ruinblocks)
				trigger.Text = $"Ошибка\n{myError.Status}\n{myError.Last_time_block}\n{ruinblocks[0].Data}\n{ruinblocks[0].User}";
		}
	}

    
}