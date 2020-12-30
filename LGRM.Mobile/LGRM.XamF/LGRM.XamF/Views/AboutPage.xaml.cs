using LGRM.XamF.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LGRM.XamF.Views
{
    [QueryProperty("UserNameText", "UserNameText")]
    public partial class AboutPage : ContentPage
    {

        string _name { get; set; }
        public string UserNameText
        {
            get => _name;
            set
            {
                _name = value;
                //BindingContext = ElephantData.Elephants.FirstOrDefault(m => m.Name == Uri.UnescapeDataString(value));
            }
        }



        public AboutPage()
        {

            this.BindingContext = this;


            if (UserNameText != null)
            {
                TestLabel.Text = UserNameText;
            }
            else
            {
                TestLabel.Text = "No luck!";
            }


            InitializeComponent();
        }
    }
}