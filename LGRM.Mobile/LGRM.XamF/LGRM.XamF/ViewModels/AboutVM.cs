using LGRM.XamF.ViewModels.Framework;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class AboutVM : BaseVM
    {
        public AboutVM()
        {            
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}