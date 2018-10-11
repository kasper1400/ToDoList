using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace ToDooList
{
    public partial class TodoItemManager
    {
        static TodoItemManager defaultInstance = new TodoItemManager();
        MobileServiceClient client;

        IMobileServiceTable<TodoItem> todoTable;

        const string offlineDbPath = @"localstore.db";

        public TodoItemManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

            this.todoTable = client.GetTable<TodoItem>();

 
        }

        public static TodoItemManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return todoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<TodoItem>; }
        }

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(bool syncItems = false  )
        {
            
            try
            {
                IEnumerable<TodoItem> items = await todoTable
                    .Where(todoItem => !todoItem.Done)
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


        //public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsyncChildrensView(bool syncItems = false)
        //{

        //    try
        //    {

        //        IEnumerable<TodoItem> items = await todoTable
        //            .Where(todoItem => todoItem.ParentsEmail == parentsEmail)
        //            .ToEnumerableAsync();


        //        return new ObservableCollection<TodoItem>(items);
        //    }
        //    catch (MobileServiceInvalidOperationException msioe)
        //    {
        //        Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine("Sync error: {0}", new[] { e.Message });
        //    }
        //    return null;
        //}

        public async Task SaveTaskAsync(TodoItem item)
        {
            try
            {
                if (item.Id == null)
                {
                    await todoTable.InsertAsync(item);
                }
                else
                {
                    await todoTable.UpdateAsync(item);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }
    }
}
