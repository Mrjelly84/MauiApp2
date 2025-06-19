using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp2
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _selectedItemDetail = "";

        public string SelectedItemDetail
        {
            get { return _selectedItemDetail; }
            set
            {
                if (_selectedItemDetail == value) return;
                _selectedItemDetail = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string
propertyName = null)
        {
            PropertyChanged?.Invoke(this, new
PropertyChangedEventArgs(propertyName));
        }
    }
}