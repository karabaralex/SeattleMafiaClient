using System;

using Xamarin.Forms;

namespace SeattleMafiaClub
{
    public partial class NewItemPage : ContentPage
    {
        public Table Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Table
            {
                Status = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
