using LGRM.Model;
using LGRM.XamF.ViewModels;
using Xamarin.Forms;

namespace LGRM.XamF.Views
{
    public partial class RecipePage : ContentPage
    {

        public RecipePage()//Recipe recipe)
        {
            BindingContext = new RecipeVM();
            
            ///   TITLE & TOOLBAR   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            Title = "Create a new recipe...";


            ///    SCROLL VIEW     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var scrollView = new ScrollView() {  };
            //Content = scrollView;
            var outterStackLayout = new StackLayout() { Spacing = 0 };



            var debugStack1 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center };

            var updateCountLabelStart = new Label() { Text = "Update Called " };
            var updateCountLabel = new Label()
            {
                VerticalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(10, 0)
            };
            updateCountLabel.SetBinding(Label.TextProperty, "OnUpdateRecipeIngredientsCalled");
            var updateCountLabelEnd = new Label() { Text = " times !" };

            debugStack1.Children.Add(updateCountLabelStart);
            debugStack1.Children.Add(updateCountLabel);
            debugStack1.Children.Add(updateCountLabelEnd);




            ///    INGREDIENT LISTS...    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var ingListMain = new StackLayout() { Margin = new Thickness(0,8) };
            var ingListL = new RecipeIngredientViews(Kind.Lean);
            var ingListG = new RecipeIngredientViews(Kind.Green);
            var ingListH = new RecipeIngredientViews(Kind.HealthyFat);
            var ingListC = new RecipeIngredientViews(Kind.Condiment);


            ///    COMPOSE PAGE     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            ingListMain.Children.Add(ingListL);
            ingListMain.Children.Add(ingListG);
            ingListMain.Children.Add(ingListH);
            ingListMain.Children.Add(ingListC);


            outterStackLayout.Children.Add(debugStack1);


            outterStackLayout.Children.Add(ingListMain);

            scrollView.Content = outterStackLayout;
            Content = scrollView;
        }

 

    }
    







}
