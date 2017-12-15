using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeattleMafiaClub
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Command LeaveJoinCommand { get; set; }
        public ItemViewModel Item { get; set; }
        public String ButtonTitle { get; set; }

        private Table table;

        public ItemDetailViewModel(Table item = null)
        {
            Title = item?.Description;
            Item = new ItemViewModel(item);
            LeaveJoinCommand = new Command(async () => {
                table = await ExecuteLeaveJoinCommand();
                updateStatus();
            });
            this.table = item;
            updateStatus();
        }

        async Task<Table> ExecuteLeaveJoinCommand()
        {
            if (IsBusy)
                return table;

            IsBusy = true;
            try
            {
                updateStatus();
                if (this.table.PlayerStatusOnTable == PlayerStatusOnTable.NON_QUEUED)
                    return await DataStore.JoinGame(this.table);
                else
                    return await DataStore.LeaveGame(this.table);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void updateStatus()
        {
            if (IsBusy)
            {
                ButtonTitle = "...";
            }
            else
            {
                if (this.table.PlayerStatusOnTable == PlayerStatusOnTable.NON_QUEUED)
                    ButtonTitle = "Join";
                else
                    ButtonTitle = "Leave";
            }

            OnPropertyChanged("ButtonTitle");
        }
    }
}
