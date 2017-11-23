using System;

namespace SeattleMafiaClub
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemViewModel Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Description;
            Item = new ItemViewModel(item);
        }
    }
}
