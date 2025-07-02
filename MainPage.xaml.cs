using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System;
using System.IO;
using Microsoft.Maui.Storage;
using MauiApp2.Models;
using MauiApp2.Services;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        private readonly ItemRepository itemRepository;
        private readonly LogService logService;
        private static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "items.db");

        public ObservableCollection<string> Items { get; } = new();
        
        private string selectedItem;
        public string SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private Entry usernameEntry;
        private Entry passwordEntry;
        private Grid mainGrid;
        private Grid loginGrid;

        public MainPage()
        {
            SQLitePCL.Batteries.Init();
            InitializeComponent();
            BindingContext = this;

            usernameEntry = this.FindByName<Entry>("UsernameEntry");
            passwordEntry = this.FindByName<Entry>("PasswordEntry");
            mainGrid = this.FindByName<Grid>("MainGrid");
            loginGrid = this.FindByName<Grid>("LoginGrid");

            var tableName = "Items";
            var logFilePath = Path.Combine(FileSystem.AppDataDirectory, "useractions.log");

            itemRepository = new ItemRepository(DbPath, tableName);
            logService = new LogService(logFilePath);

            foreach (var item in itemRepository.LoadItems())
                Items.Add(item);
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(ItemEditor.Text))
                {
                    itemRepository.AddItem(ItemEditor.Text);
                    Items.Clear();
                    foreach (var item in itemRepository.LoadItems())
                        Items.Add(item);
                    logService.LogAction($"User added item: '{ItemEditor.Text}'");
                    ItemEditor.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Add failed: {ex.Message}", "OK");
            }
        }

        private void OnRemoveButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedItem))
            {
                itemRepository.RemoveItem(SelectedItem);
                Items.Clear();
                foreach (var item in itemRepository.LoadItems())
                    Items.Add(item);
                SelectedItem = null;
                logService.LogAction($"User removed item: '{SelectedItem}'");
            }
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedItem) && !string.IsNullOrWhiteSpace(ItemEditor.Text))
            {
                itemRepository.EditItem(SelectedItem, ItemEditor.Text);
                Items.Clear();
                foreach (var item in itemRepository.LoadItems())
                    Items.Add(item);
                SelectedItem = null;
                ItemEditor.Text = string.Empty;
                logService.LogAction($"User edited item: '{ItemEditor.Text}'");
            }
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            const string validUsername = "admin";
            const string validPassword = "password123";

            if (usernameEntry.Text == validUsername && passwordEntry.Text == validPassword)
            {
                loginGrid.IsVisible = false;
                mainGrid.IsVisible = true;
                logService.LogAction($"User '{usernameEntry.Text}' logged in.");
            }
            else
            {
                DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            loginGrid.IsVisible = true;
            mainGrid.IsVisible = false;
            usernameEntry.Text = string.Empty;
            passwordEntry.Text = string.Empty;
        }

        private async void OnLogFileTapped(object sender, EventArgs e)
        {
            if (File.Exists(logService.LogFilePath))
            {
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(logService.LogFilePath)
                });
            }
            else
            {
                await DisplayAlert("Log File", "Log file not found.", "OK");
            }
        }
    }
}