using Microsoft.Maui.Controls;
using System.ComponentModel;

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

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }

        // Event handler for the "Add" button
        private void OnAddButtonClicked(object sender, EventArgs e)
        {
            // Add your logic here
        }

        // Event handler for the "Remove" button
        private void OnRemoveButtonClicked(object sender, EventArgs e)
        {
            // Add your logic here
        }

        // Event handler for the "Edit" button
        private void OnEditButtonClicked(object sender, EventArgs e)
        {
            // Add your logic here
        }
    }
}