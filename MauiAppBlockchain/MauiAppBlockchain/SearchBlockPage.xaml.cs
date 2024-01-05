using MauiAppBlockchain.Models;
using MauiAppBlockchain.Service;

namespace MauiAppBlockchain;

public partial class SearchBlockPage : ContentPage
{
    private readonly СonnectionService сonnectionService;

    private SelectIndex index;
    enum SelectIndex
    {
        Категория,
        Пользователь
    }
    public IEnumerable<Block> Blocks { get; set; }
    public IEnumerable<Category> Categories { get; set; }
    public IEnumerable<User> Users { get; set; }
    public SearchBlockPage(СonnectionService сonnection)
    {
        InitializeComponent();
        сonnectionService = сonnection;

        collectionView.SelectionChanged += (sender, e) =>
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var selectedElement = e.CurrentSelection[0] as Block; // Получите выбранный элемент
                DisplayFullInfoBlock(selectedElement);                                 // Выполните нужные действия с выбранным элементом
            }
        };
    }
    private async void DisplayFullInfoBlock(Block model)
    {
        await DisplayAlert($"Блок {model.Id}", $"Cумма: {model.Amount}\nОписание: {model.Description}\nВремя:{model.Date}\nКатегория: {model.Category.Title}\nПользователь: {model.User.Name}", "OK");
    }
    async Task InitializeAsync()
    {
        var chain = await сonnectionService.GetBlocks();
        Blocks = chain.Blocks;
        Categories = await сonnectionService.GetCategories();
        Users = await сonnectionService.GetUsers();


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
                index = SelectIndex.Категория;
                break;
            case 1:
                pickerItems.ItemsSource = (System.Collections.IList)Users;
                pickerItems.ItemDisplayBinding = new Binding("Name");
                index = SelectIndex.Пользователь;
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
            case SelectIndex.Категория:
                searchBlocks = Blocks.Where(block => block.Category.Title == (pickerItems.SelectedItem as Category).Title).ToList();
                break;
            case SelectIndex.Пользователь:
                searchBlocks = Blocks.Where(block => block.User.Name == (pickerItems.SelectedItem as User).Name).ToList();
                break;
            default:
                break;
        }

        collectionView.ItemsSource = searchBlocks;


    }
}