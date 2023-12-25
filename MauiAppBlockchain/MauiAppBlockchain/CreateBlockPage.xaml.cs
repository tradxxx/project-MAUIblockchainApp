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
    private readonly Task<IEnumerable<Category>> _categories;
    private readonly Task<IEnumerable<User>> _users;
    private IList<Category> categories;
    private IList<User> users;

    public CreateBlockPage(�onnectionService �onnection)
    {
        InitializeComponent();
        
        �onnectionService = �onnection;

        _categories = �onnectionService.GetCategories();
        _users = �onnectionService.GetUsers();

        AsyncHelperCategory();
        AsyncHelperUser();      
    }
    async void AsyncHelperCategory()
    {
        categories = (IList<Category>)await _categories;    
    }
    async void AsyncHelperUser()
    {
        users = (IList<User>)await _users;
    }
    private async void SendFromData(object sender, EventArgs e)
    {
        Block block = new Block
        {
            Amount = Convert.ToDecimal(AmountEntry.Text),
            Description = DescriptionEntry.Text
        };

        
        var category = categories.FirstOrDefault(x => x.Title == �ategoryEntry.Text.Trim());
        var user = users.FirstOrDefault(x => x.Name == UserEntry.Text.Trim());

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