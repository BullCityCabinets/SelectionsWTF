using LGRM.XamF.ViewModels.Framework;
using LGRM.XamF.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class LogInVM : BaseVM
    {
        public Command LoginCommand { get; }

        public LogInVM()
        {
            //LoginCommand = new Command(OnLoginCommand);
        }


        //private async void OnLoginCommand(object obj)
        //{
        //    // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
        //    //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");


        //    try
        //    {
        //        await Shell.Current.GoToAsync($"about");
        //    }
        //    catch (Exception exc)
        //    {   
        //        Console.WriteLine(exc.Message);
        //        Console.WriteLine(exc.InnerException);
        //        Console.WriteLine(exc.StackTrace);
        //    }

        //    //await Shell.Current.GoToAsync($"//{nameof(GroceriesPage)}");
        //}
    }
}
