using System;
using Xamarin.Forms;

namespace LGRM.XamF.Views.Controls
{




        public class IngredientUOMEntry : Entry
        {

            int widthReq = 60;
            int minWidthReq = 60;
            public double fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));

            public IngredientUOMEntry()
            {
                Keyboard = Keyboard.Numeric;

                FontSize = fontB;

                MinimumWidthRequest = minWidthReq;
                WidthRequest = widthReq;

                HorizontalOptions = LayoutOptions.End;
                HorizontalTextAlignment = TextAlignment.End;
            Margin = 0; //new Thickness(8, 0, 0, 0);
                HeightRequest = 38;
            BackgroundColor = Color.LightCyan;



            }

        }





    
}

