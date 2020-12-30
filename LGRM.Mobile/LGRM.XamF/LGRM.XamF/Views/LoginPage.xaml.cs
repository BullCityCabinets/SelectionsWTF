using LGRM.XamF.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LGRM.XamF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();
            this.BindingContext = new LogInVM();
        }

        async void buttonLogIn_ClickedAsync(object sender, EventArgs e)
        {                
            await Navigation.PushAsync(new CookbookLocalPage());
        }



    }
}