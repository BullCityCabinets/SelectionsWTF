using Xamarin.Forms;

namespace LGRM.XamF.Views.Controls
{

        public class IngredientUOMLabel : Label
        {
            public double fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));

            public IngredientUOMLabel()
            {
            FontSize = fontB;
            VerticalTextAlignment = TextAlignment.Center;                        
            HorizontalTextAlignment = TextAlignment.End;
            LineBreakMode = LineBreakMode.TailTruncation;

            Margin = 0;
            Padding = new Thickness(0, 0, 8, 0); // -10);                       
            
            
            }
        }





    
}

