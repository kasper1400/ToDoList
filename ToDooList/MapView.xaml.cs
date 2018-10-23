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

        public MapView (string parentsEmail, string childrensEmail)
		{
			//InitializeComponent ();

            this.parentsEmail = parentsEmail;
            this.childrensEmail = childrensEmail;


            var map = new Map(
            MapSpan.FromCenterAndRadius(
                    new Position(60.97735755, 24.4751485), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }
    }
}