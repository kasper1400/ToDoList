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
	public partial class Children : ContentPage
	{
        

        public Children ()
		{
			InitializeComponent ();

		}


        public void Button_Clicked(object sender, EventArgs e)
        {
            Button btnCheckbox = new Button();
            this.btnCheckbox.BackgroundColor = Color.Blue;

        }
        public void Button_Clicked2(object sender, EventArgs e)
        {
            Button btnCheckbox2 = new Button();
            this.btnCheckbox2.BackgroundColor = Color.Blue;

        }
        public void Button_Clicked3(object sender, EventArgs e)
        {
            Button btnCheckbox3 = new Button();
            this.btnCheckbox3.BackgroundColor = Color.Blue;

        }
        public void Button_Clicked4(object sender, EventArgs e)
        {
            Button btnCheckbox4 = new Button();
            this.btnCheckbox4.BackgroundColor = Color.Blue;

        }
        public void Button_Clicked5(object sender, EventArgs e)
        {
            Button btnCheckbox5 = new Button();
            this.btnCheckbox5.BackgroundColor = Color.Blue;

        }
        public void Button_Clicked6(object sender, EventArgs e)
        {
            Button btnCheckbox6 = new Button();
            this.btnCheckbox6.BackgroundColor = Color.Blue;

        }
        public void Button_Clicked7(object sender, EventArgs e)
        {
            Button btnCheckbox7 = new Button();
            this.btnCheckbox7.BackgroundColor = Color.Blue;

        }
        public void Button_Clicked8(object sender, EventArgs e)
        {
            Button btnCheckbox8 = new Button();
            this.btnCheckbox8.BackgroundColor = Color.Blue;

        }
    }
}