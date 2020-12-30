using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LGRM.XamF.ViewModels;
using LGRM.Model;

namespace LGRM.XamF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CookbookLocalPage : ContentPage
    {
        readonly Color frameBorderColor = Color.LightSlateGray;
        public int selectedRecipeId { get; set; }

        public CookbookLocalPage()
        {
            BindingContext = new CookbookLocalVM();

            ///    COLORS          \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            this.Visual = VisualMarker.Material;
            #region Colors...
            Application.Current.Resources.TryGetValue("Grey50", out var resourceValue);
            var colorG50 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("Grey100", out resourceValue);
            var colorG100 = (Color)resourceValue;
            #endregion Colors...
            #region Font Sizes...
            var fontL = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            var fontM = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            var fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));
            #endregion Font Sizes...

            ///    TOOLBAR         \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            Title = "My Cookbook ";

            ///    SCROLL VIEW     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var scrollView = new ScrollView() { BackgroundColor = colorG50 };
            Content = scrollView;
            var outterStackLayout = new StackLayout() { };
            scrollView.Content = outterStackLayout;


            ///       HEADER       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var header = new StackLayout() { Padding = new Thickness(6, 6, 6, 0), BackgroundColor = Color.White };

            //~~~ Load / Delete Buttons...
            var controlsStack = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(12, 0) }; //, BackgroundColor = Color.Orchid };





            var CreateNewRecipe = new Button() { Text = "Create New Recipe", HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = colorG100 };
            CreateNewRecipe.Command = new Command(async () => {

                await Application.Current.MainPage.Navigation.PushAsync(new RecipePage());
            });
            controlsStack.Children.Add(CreateNewRecipe);





            header.Children.Add(controlsStack);

            //~~~ Saved Recipe Count...
            var stackName = new StackLayout() { Orientation = StackOrientation.Horizontal };
            var nameLabel = new Label() { Text = "Total Recipes :", FontSize = fontM, HorizontalOptions = LayoutOptions.FillAndExpand };
            stackName.Children.Add(nameLabel);

            header.Children.Add(stackName);





            ///    RECIPE COLLECTION     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var recipeCollection = new CollectionView()
            {
                ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(8, 4),
                SelectionMode = SelectionMode.Single,
                SelectedItem = selectedRecipeId,
                //~~~~ Empty View...
                EmptyView = new Label()
                {
                    Text = "Your cookbook is empty, for now... ",
                    FontSize = fontM,
                    TextColor = Color.White,
                    FontAttributes = FontAttributes.Italic,
                    Padding = new Thickness(30, 0, 0, 0),
                    HeightRequest = 60,
                    VerticalOptions = LayoutOptions.StartAndExpand
                }
            };

            recipeCollection.SelectionChanged += OnCollectionViewSelectionChanged;

            recipeCollection.SetBinding(CollectionView.ItemsSourceProperty, "RecipesDisplayed");
            recipeCollection.SetBinding(CollectionView.HeightRequestProperty, "HeightOfCollectionView");


            //~~~~ Item Template...
            recipeCollection.ItemTemplate = new DataTemplate(() =>
            {
                var OutterStack = new StackLayout() { Margin = new Thickness(0, 0, 0, 8) };
                var OutterFrame = new Frame()
                {
                    IsClippedToBounds = true,
                    CornerRadius = 1,
                    Padding = 0,
                    BorderColor = frameBorderColor
                };


                ///    GRID            \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\       
                var dtGrid = new Grid
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0),
                    RowSpacing = 0
                };

                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(78) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(78) });

                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22) });


                /////    NAME & DESC     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                var name1Label = new Label { FontSize = fontM, Padding = new Thickness(0), FontAttributes = FontAttributes.Bold };
                name1Label.SetBinding(Label.TextProperty, "Name");
                dtGrid.Children.Add(name1Label, 1, 0);



                ///    WRAP UP    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 
                OutterFrame.Content = dtGrid;
                OutterStack.Children.Add(OutterFrame);
                return OutterStack;

            }); ///    ... end DataTemplate      //////////////////////////////////////////////////////////





            ///    WRAP UP PAGE     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            outterStackLayout.Children.Add(header);
            outterStackLayout.Children.Add(recipeCollection);


        }





        void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRecipeId = (e.CurrentSelection.FirstOrDefault() as Recipe).Id;
            OnPropertyChanged(nameof(selectedRecipeId));
        }


    }
}