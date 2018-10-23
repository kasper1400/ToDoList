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
	public partial class Map : ContentPage
	{

        private string parentsEmail;
        private string childrensEmail;

        public Map (string parentsEmail, string childrensEmail)
		{
			InitializeComponent ();

            this.parentsEmail = parentsEmail;
            this.childrensEmail = childrensEmail;

        }
    }
}