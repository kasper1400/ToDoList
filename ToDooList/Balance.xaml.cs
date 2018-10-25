using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using System.Collections;
using System.ComponentModel;
using Plugin.Messaging;

namespace ToDooList
{
 	public partial class Balance : ContentPage
	{
        // Track whether the user has authenticated.
        bool authenticated = true;

        private string parentsEmail;
        private string childrensEmail;
        private int prices;

        static TodoItemManager defaultInstance = new TodoItemManager();
        MobileServiceClient client;

        IMobileServiceTable<TodoItem> todoTable;

        TodoItemManager manager;

        public Balance(string childrensEmail, string parentsEmail)
        {
            InitializeComponent();
            this.parentsEmail = parentsEmail;
            this.childrensEmail = childrensEmail;

            manager = TodoItemManager.DefaultManager;

            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.todoTable = client.GetTable<TodoItem>();
        }

        private void SendEmail_Clicked(object sender, EventArgs e)
        {
            var emailTask = CrossMessaging.Current.EmailMessenger;
            if (emailTask.CanSendEmail)
            {
                // Simple e-mail to single receiver without attachments, CC, or BCC.
                emailTask.SendEmail(childrensEmail, "Ilmoitus kotitöiden maksusta", "Hei, sinulle on lähetetty seuraava maksu: " +
                    balanceLabel.Text + ". Terveisin, " + parentsEmail);

                // More complex emails can also be sent:
                //var email = new EmailMessageBuilder()
                //  .To("plugins@xamarin.com")
                //  .Cc("plugins.cc@xamarin.com")
                //  .Bcc(new[] { "plugins.bcc@xamarin.com", "plugins.bcc2@xamarin.com" })
                //  .Subject("Xamarin Messaging Plugin")
                //  .Body("Hello from your friends at Xamarin!")
                //  .Build();

                //emailTask.SendEmail(email);
            }
        }

        async Task GetPrices(int prices)
        {
            IEnumerable<TodoItem> items = await todoTable
            .Where(todoItem => todoItem.ChildrensEmail == childrensEmail && todoItem.Done == true && !todoItem.SoftDelete)
            .ToEnumerableAsync();

            foreach (TodoItem item in items)
            {
                prices += item.Price;
            }
            balanceLabel.Text = "Tilin " + childrensEmail + " balanssi: " + prices.ToString() + "€";
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();


            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                // Set syncItems to true in order to synchronize the data
                // on startup when running in offline mode.
                await RefreshItems(true, syncItems: false);

                // Hide the Sign-in button.
                //this.loginButton.IsVisible = false;

                // Combined ListView item prices to label
                await GetPrices(prices);
            }
        }
 
        // Data methods
        async Task DeleteItem(TodoItem item)
        {
            item.SoftDelete  = true;
            await manager.SaveTaskAsync(item);
            todoList.ItemsSource = await GetTodoItemsAsyncBalanceView();
            await GetPrices(prices);
        }

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsyncBalanceView(bool syncItems = false)
        {
            try
            {

                IEnumerable<TodoItem> items = await todoTable
                    .Where(todoItem => todoItem.ChildrensEmail == childrensEmail && todoItem.TaskReady == true && todoItem.Done == true && !todoItem.SoftDelete )
                    .ToEnumerableAsync();

                List<TodoItem> newItems = new List<TodoItem>();
                foreach (TodoItem modifytodoItem in items)
                {
                    if (modifytodoItem.Done)
                    {
                        modifytodoItem.imageSource = "checkmark.png";
                    }
                    else
                    {
                        modifytodoItem.imageSource = "rasti.png";
                    }
                    newItems.Add(modifytodoItem);
                }

                return new ObservableCollection<TodoItem>(newItems);

            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Sync error: {0}", new[] { e.Message });
            }
            return null;
        }

        // Event handlers
        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var todo = e.SelectedItem as TodoItem;
            if (Device.RuntimePlatform != Device.iOS && todo != null)
            {
                // Not iOS - the swipe-to-delete is discoverable there
                if (Device.RuntimePlatform == Device.Android)
                {
                    await DisplayAlert(todo.Task, "Paina ja pidä pohjassa kotityötä, jonka haluat merkata valmiiksi" + todo.Task, "Ymmärretty!");
                }
                else
                {
                    // Windows, not all platforms support the Context Actions yet
                    if (await DisplayAlert("Mark completed?", "Do you wish to delete " + todo.Task + "?", "Delete", "Cancel"))
                    {
                        await DeleteItem(todo);
                    }
                }
            }

            // prevents background getting highlighted
            todoList.SelectedItem = null;
        }

        public async void OnComplete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var todo = mi.CommandParameter as TodoItem;
            await DeleteItem(todo);
        }

        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await RefreshItems(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await RefreshItems(true, true);
        }

        public async void OnRefreshItems(object sender, EventArgs e)
        {
            await RefreshItems(true, false);
        }

        private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                todoList.ItemsSource = await GetTodoItemsAsyncBalanceView(syncItems);
            }
        }

        private class ActivityIndicatorScope : IDisposable
        {
            private bool showIndicator;
            private ActivityIndicator indicator;
            private Task indicatorDelay;

            public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
            {
                this.indicator = indicator;
                this.showIndicator = showIndicator;

                if (showIndicator)
                {
                    indicatorDelay = Task.Delay(2000);
                    SetIndicatorActivity(true);
                }
                else
                {
                    indicatorDelay = Task.FromResult(0);
                }
            }

            private void SetIndicatorActivity(bool isActive)
            {
                this.indicator.IsVisible = isActive;
                this.indicator.IsRunning = isActive;
            }

            public void Dispose()
            {
                if (showIndicator)
                {
                    indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }
    }
}
