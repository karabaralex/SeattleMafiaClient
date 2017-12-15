using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Xamarin.Forms;
using Plugin.Permissions.Abstractions;

namespace SeattleMafiaClub
{
    public class ItemViewModel
    {

        public ItemViewModel(Table item)
        {
            this.Item = item;
            switch(item.PlayerStatusOnTable)
            {
                case PlayerStatusOnTable.QUEUED:
                    PlayerStatus = "In queue";
                    break;
                default:
                case PlayerStatusOnTable.NON_QUEUED:
                    PlayerStatus = "";
                    break;
                case PlayerStatusOnTable.SEATED:
                    PlayerStatus = "In the game";
                    break;
            }
        }

        public Table Item { get; }

        public string FullName
        {
            get
            {
                string fName = this.Item.Host.FirstName != null ? this.Item.Host.FirstName : "";
                string sName = this.Item.Host.LastName != null ? this.Item.Host.LastName : "";
                return fName + " " + sName;
            }
        }

        public string GameStatus
        {
            get
            {
                switch(Item.GameStatus)
                {
                    case GameStatusValue.PENDING_START:
                        return "Pending";
                    case GameStatusValue.IN_PROGRESS:
                        return "In progress";
                    case GameStatusValue.FINISHED:
                        return "Finished";
                    default:
                    case GameStatusValue.NONE:
                        return "";
                }
            }
        }

        public string PlayerStatus
        {
            get; set;
        }
    }

    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<ItemViewModel> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel()
        {
            Title = "Games";
            Items = new ObservableCollection<ItemViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Table>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Table;
                Items.Add(new ItemViewModel(_item));
                await DataStore.AddItemAsync(_item);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(new ItemViewModel(item));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
