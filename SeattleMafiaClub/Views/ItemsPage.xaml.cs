﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SeattleMafiaClub.Services;
using SeattleMafiaClub.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SeattleMafiaClub
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ItemViewModel;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item.Item)));

            //// Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void SettingsItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.Items.Clear();
        }
    }
}
public class GoogleOAuthToken
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
}