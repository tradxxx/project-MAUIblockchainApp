using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace MauiAppBlockchain;

public partial class CreateBlockPage : ContentPage
{
    private readonly �onnectionService �onnectionService;

    //private Task<IEnumerable<Category>> _categories;
    //private Task<IEnumerable<User>> _users;
    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }

    public CreateBlockPage(�onnectionService �onnection)
    {
        InitializeComponent();
        
        �onnectionService = �onnection;

        //_categories = �onnectionService.GetCategories();
        //_users = �onnectionService.GetUsers();

        //AsyncHelperCategory();
        //AsyncHelperUser();

        
    }

    async Task InitializeAsync()
    {
        Categories = await �onnectionService.GetCategories();
        Users = await �onnectionService.GetUsers();
        pickerCategories.ItemsSource = (System.Collections.IList)Categories;
        pickerUsers.ItemsSource = (System.Collections.IList)Users;
       
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await InitializeAsync();

        //errorView.Text = string.Empty;
        //errorView.BackgroundColor = Colors.Transparent;
        //sendView.Text = string.Empty;
        //sendView.Background = Colors.Transparent;
    }


    //async void AsyncHelperCategory()
    //{
    //    Categories = (List<Category>)await _categories;    
    //}
    //async void AsyncHelperUser()
    //{
    //    Users = (List<User>)await _users;
    //}



    private async void SendFromData(object sender, EventArgs e)
    {
        Block block = new Block
        {
            Amount = Convert.ToDecimal(AmountEntry.Text),
            Description = DescriptionEntry.Text,
            CategoryId = (pickerCategories.SelectedItem as Category).Id,
            UserId = (pickerUsers.SelectedItem as User).Id
            
        };


        //var category = Categories.FirstOrDefault(x => x.Title == �ategoryEntry.Text.Trim());
        //var user = Users.FirstOrDefault(x => x.Name == UserEntry.Text.Trim());

        //if (category == null)
        //{
        //    DisplayArtService.PrintLabelStatus(errorView, "�� ������� ���������", LabelStatus.Error);
        //}
        //else if (user == null)
        //{
        //    DisplayArtService.PrintLabelStatus(errorView, "�� ������ ������������", LabelStatus.Error);

        //}
        //else
        //{
        //    block.CategoryId = category.Id;
        //    block.UserId = user.Id;



        //}

        if (ValidatorData.ValidateCheckData(block, errorView))
        {
            var json = JsonConvert.SerializeObject(block);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await �onnectionService.CreateBlock(content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("�����", "���� ��������", "��");
                ResetForm();
            }
            else
            {
           

                switch ((int)response.StatusCode)
                {
                    case 400:
                        DisplayArtService.PrintLabelStatus(errorView, "����������� ����������� ���������", LabelStatus.Error);
                        //����� ���������� ��� ��������� ����
                        var myErrorData = await response.Content.ReadFromJsonAsync<BlocksData>();

                        var ruinblocks = myErrorData.Blocks as IList<Block>;
                        sendView.FontFamily = "OpenSansSemibold";
                        sendView.Text = $"������ ������ � ����� �{ruinblocks[0].Id}:\n������: {myErrorData.Status}\n�����: {ruinblocks[0].Date}\n�����: {ruinblocks[0].Amount}\n��������: {ruinblocks[0].Date}\n���������: {ruinblocks[0].Category.Title}\n������������: {ruinblocks[0].User.Name}";
                        break;
                    case 404:
                        DisplayArtService.PrintLabelStatus(errorView, "������ ����������", LabelStatus.Error);
                        await DisplayAlert("������ �����������", "�������� �����������!", "��");
                        break;

                }
                


            }
        }


    }

    public void ResetForm()
    {
        // ������� ����� �����
        AmountEntry.Text = string.Empty;
        DescriptionEntry.Text = string.Empty;

        // ����� ��������� ��������� � Picker
        pickerCategories.SelectedIndex = -1;
        pickerUsers.SelectedIndex = -1;

        // ������� ������ ��������� ���������� (���� ����������)
        errorView.Text = string.Empty;
        errorView.BackgroundColor = Colors.Transparent;
        sendView.Text = string.Empty;
        sendView.BackgroundColor = Colors.Transparent;
    }


}