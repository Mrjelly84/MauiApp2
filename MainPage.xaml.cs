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

        private const string DbPath = "items.db";
        private const string TableName = "Items";

        public MainPage()
        {
            Batteries.Init(); // Use Batteries_V2.Init() instead of Batteries.Init()
            InitializeComponent();
            BindingContext = this;
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
            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT Detail FROM {TableName};";
            using var reader = command.ExecuteReader();
            var details = new List<string>();
            while (reader.Read())
            {
                details.Add(reader.GetString(0));
            }
            SelectedItemDetail = string.Join("\n", details);
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
            // Add your logic here
        }

        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            // Add your logic here
        }
    }
}