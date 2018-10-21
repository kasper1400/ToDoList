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

        public string myEmail;
        public string parentsEmail;

        public static IAuthenticate Authenticator { get; private set; }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;

        }

        public MainMenu ()
		{
			InitializeComponent ();
            string myEmail = childrensEmailInput.Text;
            string parentsEmail = parentsEmailInput.Text;

        }

        private void EmailToLabel_Clicked(object sender, EventArgs e)
        {
                EmailLabel.Text = childrensEmailInput.Text;
        }

        private void ParentsEmailToLabel_Clicked(object sender, EventArgs e)
        {
           ParentsEmailLabel.Text = parentsEmailInput.Text;


        }

        private void ClearEmailLabel_Clicked(object sender, EventArgs e)
        {
            EmailLabel.Text = null;
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
                string parentsEmail = ParentsEmailLabel.Text;


                Navigation.PushAsync(new Parent(parentsEmail));
            }          
            else
            {
                DisplayAlert("Pääsy estetty", "Tunnistaudu ensin!", "OK");
            }
        }


        private void ChildrensView(object sender, EventArgs e)
        {

            if (EmailLabel.Text == null || ParentsEmailLabel.Text == null)
            {
                DisplayAlert("Pääsy estetty", "Täytä sähköpostisi ja vanhempasi sähköposti!", "OK");
            }
            else if (authenticated == true)
            {
                string childrensEmail = EmailLabel.Text;
                string parentsEmail = ParentsEmailLabel.Text;

                Navigation.PushAsync(new Children(childrensEmail, parentsEmail));
            }
            else
            {
                DisplayAlert("Pääsy estetty", "Tunnistaudu ensin", "OK");
            }

        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {

            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();
        }

    }
}