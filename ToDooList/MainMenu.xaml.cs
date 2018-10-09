using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDooList
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : ContentPage
	{
        bool authenticated = false;

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        public MainMenu ()
		{
			InitializeComponent ();
		}

        private void ParentsView(object sender, EventArgs e)
        {
            if (authenticated == true)
            {
                Navigation.PushAsync(new Parent());
            }
            else
            {
                DisplayAlert("Pääsy estetty", "Tunnistaudu ensin!", "OK");
            }
        }

        private void ChildrensView(object sender, EventArgs e)
        {           
            if (authenticated == true)
            {
               Navigation.PushAsync(new Children());
            }
            else
            {
                DisplayAlert("Pääsy estetty", "Tunnistaudu ensin!", "OK");
            }
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();
        }
    }
}