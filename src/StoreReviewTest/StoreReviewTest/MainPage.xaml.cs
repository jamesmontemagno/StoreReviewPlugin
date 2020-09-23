using Plugin.StoreReview;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StoreReviewTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

		async void Button_Clicked(object sender, EventArgs e)
		{
			await CrossStoreReview.Current.RequestReview(true);
		}
	}
}
