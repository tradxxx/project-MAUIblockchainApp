using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MauiAppBlockchain;

public partial class MainPage : ContentPage
{

	public class Block
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? Id { get; set; }
		[Required(ErrorMessage = "Please enter your name")]
		public string Data { get; set; }
		public DateTime Created { get; set; }
		public string Hash { get; set; }
		public string PreviousHash { get; set; }
		[Required(ErrorMessage = "Please enter your user")]
		public string User { get; set; }

	}
	public class BlocksData
	{
		public int Blocks_count { get; set; }
		public DateTime Last_time_block { get; set; }
		public IEnumerable<Block> Blocks { get; set; }
	}

	public MainPage()
	{
		InitializeComponent();
	}
	private async void OnButton2Clicked(object sender, System.EventArgs e)
	{
		var client = new HttpClient();
		string request = await client.GetStringAsync(new Uri("http://192.168.43.175:5153/api/Chain"));
		var Mychain = JsonConvert.DeserializeObject<BlocksData>(request);
		blockList.ItemsSource = Mychain.Blocks;
		label2.Text = "База данных успешно загружена с веб-службы";

		string Host = System.Net.Dns.GetHostName();
		string IP = System.Net.Dns.GetHostByName(Host).AddressList[0].ToString();
		label1.Text = Host + "  " + IP;

	}

	private async void OnButtonClicked(object sender, System.EventArgs e)
	{
		// При нажатии кнопки производим переход на CreateBlockPage
		Navigation.PushAsync(new CreateBlockPage());
	}

}

