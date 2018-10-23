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

        public string childrensEmail;
        public string parentsEmail;

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;

        }

        public MainMenu ()
		{
			InitializeComponent ();
            
            
        }

        private void ChildrensEmailToLabel_Clicked(object sender, EventArgs e)
        {
            ChildrensEmailLabel.Text = childrensEmailInput.Text;
            childrensEmail = ChildrensEmailLabel.Text;
        }

        private void ParentsEmailToLabel_Clicked(object sender, EventArgs e)
        {
           ParentsEmailLabel.Text = parentsEmailInput.Text;
           parentsEmail = ParentsEmailLabel.Text;
        }

        private void ClearEmailLabel_Clicked(object sender, EventArgs e)
        {
            ChildrensEmailLabel.Text = null;
        }

        private void ClearParentsEmailLabel_Clicked(object sender, EventArgs e)
        {
            ParentsEmailLabel.Text = null;
        }

        private void ParentsView(object sender, EventArgs e)
        {

            if (ParentsEmailLabel.Text == null)
            {
                DisplayAlert("Pääsy estetty", "Täytä vanhemman sähköposti", "OK");
            }
            else if (authenticated == true)
            {
                Navigation.PushAsync(new Parent(childrensEmail, parentsEmail));
            }          
            else
            {
                DisplayAlert("Pääsy estetty", "Tunnistaudu ensin!", "OK");
            }
        }


        private void ChildrensView(object sender, EventArgs e)
        {

            if (ChildrensEmailLabel.Text == null || ParentsEmailLabel.Text == null)
            {
                DisplayAlert("Pääsy estetty", "Täytä sähköpostisi ja vanhempasi sähköposti!", "OK");
            }
            else if (authenticated == true)
            {

                Navigation.PushAsync(new Children(childrensEmail, parentsEmail));
            }
            else
            {
                DisplayAlert("Pääsy estetty", "Tunnistaudu ensin", "OK");
            }

        }

        private void BalanceView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Balance(childrensEmail, parentsEmail));
        }

        private void MapView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Map(childrensEmail, parentsEmail));
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {

            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();
        }

    }
}