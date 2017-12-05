using System;
using System.Collections.Generic;
using SeattleMafiaClub.ViewModels;
using Xamarin.Forms;

namespace SeattleMafiaClub.Views
{
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel viewModel;

        public SettingsPage()
        {
            InitializeComponent();

            viewModel = new SettingsViewModel(Navigation);
            BindingContext = viewModel;
        }
    }
}
