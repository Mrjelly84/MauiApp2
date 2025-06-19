using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.Sqlite; // Add this NuGet package if not present
using System.Collections.ObjectModel;

namespace MauiApp2
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private string selectedItemDetail;
        public string SelectedItemDetail
        {
            get => selectedItemDetail;
            set
            {
                if (selectedItemDetail != value)
                {
                    selectedItemDetail = value;
                    OnPropertyChanged(nameof(SelectedItemDetail));
                }
            }
        }

        public ObservableCollection<string> Items { get; set; } = new();
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

        private const string DbPath = "items.db";
        private const string TableName = "Items";

        private Entry usernameEntry;
        private Entry passwordEntry;
        private Grid mainGrid;  // Rename the field to avoid ambiguity
        private Grid loginGrid; // Rename the field to avoid ambiguity

        public MainPage()
        {
            SQLitePCL.Batteries.Init();
            InitializeComponent();
            BindingContext = this;

            // Initialize usernameEntry and passwordEntry
            usernameEntry = this.FindByName<Entry>("UsernameEntry");
            passwordEntry = this.FindByName<Entry>("PasswordEntry");

            // Initialize MainGrid and LoginGrid
            mainGrid = this.FindByName<Grid>("MainGrid"); // Use the renamed field
            loginGrid = this.FindByName<Grid>("LoginGrid"); // Use the renamed field

            InitializeDatabase();
            LoadItems();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                $"CREATE TABLE IF NOT EXISTS {TableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT, Detail TEXT NOT NULL);";
            command.ExecuteNonQuery();
        }

        private void LoadItems()
        {
            Items.Clear();
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT Detail FROM {TableName};";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Items.Add(reader.GetString(0));
            }
        }

        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ItemEditor.Text))
            {
                using var connection = new SqliteConnection($"Data Source={DbPath}");
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO {TableName} (Detail) VALUES (@detail);";
                command.Parameters.AddWithValue("@detail", ItemEditor.Text);
                command.ExecuteNonQuery();
                LoadItems();
                ItemEditor.Text = string.Empty;
            }
        }

        private void OnRemoveButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedItem))
            {
                using var connection = new SqliteConnection($"Data Source={DbPath}");
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM {TableName} WHERE Detail = @detail;";
                command.Parameters.AddWithValue("@detail", SelectedItem);
                command.ExecuteNonQuery();
                LoadItems();
                SelectedItem = null;
            }
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedItem) && !string.IsNullOrWhiteSpace(ItemEditor.Text))
            {
                using var connection = new SqliteConnection($"Data Source={DbPath}");
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"UPDATE {TableName} SET Detail = @newDetail WHERE Detail = @oldDetail;";
                command.Parameters.AddWithValue("@newDetail", ItemEditor.Text);
                command.Parameters.AddWithValue("@oldDetail", SelectedItem);
                command.ExecuteNonQuery();
                LoadItems();
                SelectedItem = null;
                ItemEditor.Text = string.Empty;
            }
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            // Example: Hardcoded credentials
            const string validUsername = "admin";
            const string validPassword = "password123";

            if (usernameEntry.Text == validUsername && passwordEntry.Text == validPassword)
            {
                loginGrid.IsVisible = false;
                mainGrid.IsVisible = true;
            }
            else
            {
                DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            // Logic to handle logout
            loginGrid.IsVisible = true;
            mainGrid.IsVisible = false;
        }
    }
}