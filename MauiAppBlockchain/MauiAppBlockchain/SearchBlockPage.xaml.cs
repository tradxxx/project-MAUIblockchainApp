using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;

namespace MauiAppBlockchain;

public partial class SearchBlockPage : ContentPage
{
    private readonly �onnectionService �onnectionService;

    private SelectIndex index;
    enum SelectIndex
    {
        ���������,
        ������������
    }
    public IEnumerable<Block> Blocks { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }
    public SearchBlockPage(�onnectionService �onnection)
    {
        InitializeComponent();
        �onnectionService = �onnection;

        collectionView.SelectionChanged += (sender, e) =>
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedElement = e.CurrentSelection[0] as Block; // �������� ��������� �������
                DisplayFullInfoBlock(selectedElement);                                 // ��������� ������ �������� � ��������� ���������
            }
        };
    }
    private async void DisplayFullInfoBlock(Block model)
    {
        await DisplayAlert($"���� {model.Id}", $"C����: {model.Amount}\n��������: {model.Description}\n�����:{model.Date}\n���������: {model.Category.Title}\n������������: {model.User.Name}", "OK");
    }
    async Task InitializeAsync()
    {
        var chain = await �onnectionService.GetBlocks();
        Blocks = chain.Blocks;
        Categories = await �onnectionService.GetCategories();
        Users = await �onnectionService.GetUsers();


    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await InitializeAsync();
    }
    void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
        switch (pickerSelectSearchItem.SelectedIndex)
        {
            case 0:
                pickerItems.ItemsSource = (System.Collections.IList)Categories;
                pickerItems.ItemDisplayBinding = new Binding("Title");
                index = SelectIndex.���������;
                break;
            case 1:
                pickerItems.ItemsSource = (System.Collections.IList)Users;
                pickerItems.ItemDisplayBinding = new Binding("Name");
                index = SelectIndex.������������;
                break;
            default:
                break;
        }
    }

    void PickerSelectedItem(object sender, EventArgs e)
    {
        IEnumerable<Block> searchBlocks = null;

        switch (index)
        {
            case SelectIndex.���������:
                searchBlocks = Blocks.Where(block => block.Category.Title == (pickerItems.SelectedItem as Category).Title).ToList();
                break;
            case SelectIndex.������������:
                searchBlocks = Blocks.Where(block => block.User.Name == (pickerItems.SelectedItem as User).Name).ToList();
                break;
            default:
                break;
        }

        collectionView.ItemsSource = searchBlocks;


    }
}