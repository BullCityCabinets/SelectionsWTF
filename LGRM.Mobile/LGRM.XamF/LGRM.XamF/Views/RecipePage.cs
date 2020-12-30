using LGRM.Model;
using LGRM.XamF.ViewModels;
using LGRM.XamF.Views.Controls;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace LGRM.XamF.Views
{
    public partial class RecipePage : ContentPage
    {

        public RecipePage()//Recipe recipe)
        {
            BindingContext = new RecipeVM();
            
            Application.Current.Resources.TryGetValue("Grey100", out var resourceValue);
            var colorG100 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("GeneralBG", out resourceValue);
            var colorGeneralBG = (Color)resourceValue;
            #region Font Sizes...
            var fontL = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            var fontM = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            var fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));
            #endregion Font Sizes...

            ///   TITLE & TOOLBAR   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            Title = "Create a new recipe...";
            var ClearRecipeToolbar = new ToolbarItem() { IconImageSource = ImageSource.FromFile("baseline_clear_white_18dp.png"), Text = "Clear" };
            ClearRecipeToolbar.SetBinding(ToolbarItem.CommandProperty, "VerifyClearRecipeCommand");
            ToolbarItems.Add(ClearRecipeToolbar);



            ///    SCROLL VIEW     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var scrollView = new ScrollView() { BackgroundColor = colorGeneralBG };
            Content = scrollView;
            var outterStackLayout = new StackLayout() { Spacing = 0 };



            ///    RECIPE NAME     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var headerRecipeNameStack = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions=LayoutOptions.StartAndExpand, BackgroundColor = colorGeneralBG, Margin = new Thickness(0,8,0,0), Padding = 0, Spacing = 0 };

            var headerRecipeName = new Label() { FontSize = fontL, FontAttributes = FontAttributes.Bold, BackgroundColor = Color.White, VerticalTextAlignment = TextAlignment.Center };
            headerRecipeName.SetBinding(Label.TextProperty, "Recipe.Name");

            headerRecipeNameStack.Children.Add(headerRecipeName);




            ///    RECIPE SUMMARY  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var headerSummary = new StackLayout() { HorizontalOptions = LayoutOptions.Fill, BackgroundColor = Color.White, Padding = new Thickness(12,2) }; //, Margin = 0, Padding = 0, Spacing = 0 };

            var headerRecipeServes = new StackLayout() { Orientation = StackOrientation.Horizontal };

            var headerRecipeServesLabel = new Label() { Text = "Recipe Serves :", FontSize = fontM, VerticalTextAlignment = TextAlignment.Center, WidthRequest = 130 };

            var headerRecipeServesEntry = new Entry() { Keyboard = Keyboard.Numeric, FontSize = fontM, WidthRequest = 60, HorizontalOptions = LayoutOptions.StartAndExpand };
            headerRecipeServesEntry.SetBinding(Entry.TextProperty, "RecipeServes");
            headerRecipeServes.Children.Add(headerRecipeServesLabel);
            headerRecipeServes.Children.Add(headerRecipeServesEntry);
                       

            headerSummary.Children.Add(new IngredientListSummaryLine(Kind.Lean));
            headerSummary.Children.Add(new IngredientListSummaryLine(Kind.Green));
            headerSummary.Children.Add(new IngredientListSummaryLine(Kind.HealthyFat));
            headerSummary.Children.Add(new IngredientListSummaryLine(Kind.Condiment));
            headerSummary.Children.Add(headerRecipeServes);



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
                        
            outterStackLayout.Children.Add(headerRecipeNameStack);
            outterStackLayout.Children.Add(headerSummary);
            outterStackLayout.Children.Add(ingListMain);

            scrollView.Content = outterStackLayout;
            Content = scrollView;
        }

 

    }
    







}
