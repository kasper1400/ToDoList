﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using System.Collections;


namespace ToDooList
{
    public partial class Parent : ContentPage
    {

        // Track whether the user has authenticated.
        bool authenticated = true;

        private string parentsEmail;
        private string childrensEmail;
        
        static TodoItemManager defaultInstance = new TodoItemManager();
        MobileServiceClient client;

        IMobileServiceTable<TodoItem> todoTable;

        TodoItemManager manager;


        public Parent(string childrensEmail, string parentsEmail)
        {
            InitializeComponent();

            this.parentsEmail = parentsEmail;
            this.childrensEmail = childrensEmail;

            manager = TodoItemManager.DefaultManager;

            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.todoTable = client.GetTable<TodoItem>();
        }

        private void BalanceView(object sender, EventArgs e)
        {
             Navigation.PushAsync(new Balance(childrensEmail, parentsEmail));
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
            }
        }

        // Data methods
        async Task AddItem(TodoItem item)
        {
            await manager.SaveTaskAsync(item);
            todoList.ItemsSource = await GetTodoItemsAsyncParentsView();
        }

        async Task CompleteItem(TodoItem item)
        {
            item.Done = true;
            await manager.SaveTaskAsync(item);
            todoList.ItemsSource = await GetTodoItemsAsyncParentsView();
        }

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsyncParentsView(bool syncItems = false)
        {
            try
            {

                IEnumerable<TodoItem> items = await todoTable
                    .Where(todoItem => todoItem.ParentsEmail == parentsEmail && !todoItem.Done)
                    .ToEnumerableAsync();

                return new ObservableCollection<TodoItem>(items);
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

        public async void OnAdd(object sender, EventArgs e)
        {

            try
            {
                var todo = new TodoItem { Task = newItemName.Text, Price = Convert.ToInt32(newItemPrice.Text), ParentsEmail = parentsEmail };
                await AddItem(todo);

                newItemName.Text = string.Empty;
                newItemName.Unfocus();

                newItemPrice.Text = string.Empty;
                newItemPrice.Unfocus();
            }
            catch
            {
                await DisplayAlert("Ei voitu lisätä", "Käytä hinnassa vain numeroita", "OK");
            }


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
                    if (await DisplayAlert("Mark completed?", "Do you wish to complete " + todo.Task + "?", "Complete", "Cancel"))
                    {
                        await CompleteItem(todo);
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
            await CompleteItem(todo);
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
                todoList.ItemsSource = await GetTodoItemsAsyncParentsView(syncItems);
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

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}

