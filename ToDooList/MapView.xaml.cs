using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ToDooList
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapView : ContentPage
	{
        private string parentsEmail;
        private string childrensEmail;

        MobileServiceClient client;

        IMobileServiceTable<TodoItem> todoTable;

        public MapView (string childrensEmail, string parentsEmail)
		{
			//InitializeComponent ();

            this.parentsEmail = parentsEmail;
            this.childrensEmail = childrensEmail;

            this.client = new MobileServiceClient(Constants.ApplicationURL);
            this.todoTable = client.GetTable<TodoItem>();

            var map = new Map(
            MapSpan.FromCenterAndRadius(
            new Position(60.97735755, 24.4751485), Distance.FromKilometers(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;

            GetLocations(map);
        }

        async Task GetLocations(Map map)
        {
            IEnumerable<TodoItem> items = await todoTable
            .Where(todoItem => todoItem.ParentsEmail == parentsEmail)
            .ToEnumerableAsync();

            foreach (TodoItem item in items)
            {
                var position = new Position(item.Latitude, item.Longitude); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = item.Task,
                    Address = "Suorittaja: "+item.ChildrensEmail
                };
                map.Pins.Add(pin);
            }
        }
    }
}