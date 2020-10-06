namespace mikoba
{
    public class TempCode
    {
        // foreach (var item in results.unprocessedItems)
        // {
            // Console.WriteLine(item.Data);
            // if (!Preferences.ContainsKey(item.Id))
            // {
            //     var message = await MessageDecoder.ProcessPackedMessage(context.Wallet, item, null);
            //     if (message != null)
            //     {
            //         Device.BeginInvokeOnMainThread(
            //             async () => { await _actionDispatcher.DispatchMessage(message); });
            //     }
            //     else
            //     {
            //         Preferences.Set(item.Id, false);
            //     }
            // }
        // }


        //TODO: Not supported by Mediator it seems.
        //Asked question in community and StackOverflow
        // if (itemsToDelete.Any())
        // {
        // var deleteMessage = new DeleteInboxItemsMessage() {InboxItemIds = itemsToDelete};
        // var response =
        //     await _messageService.SendReceiveAsync(context.Wallet, deleteMessage, this.Entry.Connection.Record);
        // //     Console.WriteLine(response.Payload);  
        // // }
        
        
        
        // if (Preferences.Get(AppConstant.EnableFirstActionsView, true) &&
        //     Preferences.Get(AppConstant.LocalWalletFirstView, false))
        // {
        //     Console.WriteLine("Navigating to Wallet First Actions Sequence");
        //     var page = Navigation.NavigationStack.Last();
        //     await Navigation.PushAsync(new FirstActionsPage());
        //     Navigation.RemovePage(page);
        // }
        // else
        // {
        //     Console.WriteLine("Navigating to Wallet Page");
        //     var page = Navigation.NavigationStack.Last();
        //     await Navigation.PushAsync(new WalletPage());
        //     Navigation.RemovePage(page);                    
        // }
        // <CollectionView
        //     HeightRequest="{Binding MenuOptionsHeight}"
        // ItemsSource="{Binding WalletActions}"
        // HorizontalOptions="Start"
        // VerticalOptions="Start"
        // VerticalScrollBarVisibility="Never">
        // <CollectionView.ItemTemplate>
        // <DataTemplate>
        // <components:MenuOption
        //     Margin="0,10,0,0"
        // LeftSvgImage="{Binding LeftIcon}"
        // RightSvgImage="{Binding RightIcon}"
        // MenuOptionText="{Binding ActionLabel}"
        // Command="{Binding ActionCommand}" />
        // </DataTemplate>
        // </CollectionView.ItemTemplate>
        // </CollectionView>
    }
}
