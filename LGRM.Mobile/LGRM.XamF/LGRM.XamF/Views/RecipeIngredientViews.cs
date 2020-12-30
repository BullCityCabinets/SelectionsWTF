using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using LGRM.Model;
using LGRM.XamF.Views.Controls;
using System.Windows.Input;

namespace LGRM.XamF.Views
{
    public partial class RecipeIngredientViews : StackLayout
    {
        private ActivityIndicator indicator;

        public Color colorA2;
        public Color colorA1;

        public string headerLabelString;
        public string itemsSourcePropertyString;
        public string heightRequestPropertyString;
        public string buttonText;
               

        public RecipeIngredientViews(Kind kind)
        {
            

            #region Colors...
            Application.Current.Resources.TryGetValue("LocalConvertStringToHexColor", out var resourceValue);
            var fromHex = (IValueConverter)resourceValue;

            Application.Current.Resources.TryGetValue("LeansA1", out resourceValue);
            var colorLA1 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("LeansA2", out resourceValue);
            var colorLA2 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("GreensA1", out resourceValue);
            var colorGA1 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("GreensA2", out resourceValue);
            var colorGA2 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("HealthyFatsA1", out resourceValue);
            var colorHA1 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("HealthyFatsA2", out resourceValue);
            var colorHA2 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("CondimentsA1", out resourceValue);
            var colorCA1 = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("CondimentsA2", out resourceValue);
            var colorCA2 = (Color)resourceValue;

            var frameBorderColor = Color.Gray;
            Application.Current.Resources.TryGetValue("GeneralBG", out resourceValue);
            var colorGeneralBG = (Color)resourceValue;
            Application.Current.Resources.TryGetValue("DefaultTextColor", out resourceValue);
            var deafultTextColor = (Color)resourceValue;
            //Application.Current.Resources.TryGetValue("DefaultEntryBG", out resourceValue);
            //var defaultEntryBGColor = (Color)resourceValue;



            #endregion Colors... ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region Font Sizes...
            var fontL = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            var fontM = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            var fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));
            #endregion Font Sizes... ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            Application.Current.Resources.TryGetValue("LocalConvertStringToBool", out resourceValue);
            var ToBeVisible = (IValueConverter)resourceValue;

            #region Kind Switch...
            switch (kind)
            {
                case Kind.Lean:

                    colorA1 = colorLA1;
                    colorA2 = colorLA2;
                    headerLabelString = "Leans";
                    itemsSourcePropertyString = "MyLeans";
                    heightRequestPropertyString = "HeightL";
                    break;

                case Kind.Green:
                    colorA1 = colorGA1;
                    colorA2 = colorGA2;
                    headerLabelString = "Greens";
                    itemsSourcePropertyString = "MyGreens";
                    heightRequestPropertyString = "HeightG";
                    break;

                case Kind.HealthyFat:
                    colorA1 = colorHA1;
                    colorA2 = colorHA2;
                    headerLabelString = "Healthy Fats";
                    itemsSourcePropertyString = "MyHealthyFats";
                    heightRequestPropertyString = "HeightH";
                    break;

                case Kind.Condiment:
                    colorA1 = colorCA1;
                    colorA2 = colorCA2;
                    headerLabelString = "Condiments";
                    itemsSourcePropertyString = "MyCondiments";
                    heightRequestPropertyString = "HeightC";
                    break;

                default:
                    break;
            }
            #endregion Kind Switch... ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            // Main StackLayout...
            BackgroundColor = colorA1;


            #region //     HEADER      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            var skiaPainter = new SkiaPainter(kind);

            var header = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0, BackgroundColor = colorGeneralBG, HeightRequest = 40, Margin = 0 };

            var canvasL = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30 };
            canvasL.PaintSurface += skiaPainter.OnCanvasPaintSurfaceL;
            var headerLabel = new Label()
            {
                Text = headerLabelString,
                TextColor = Color.White,
                FontSize = fontL,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = colorA1,
                HeightRequest = 40,
                LineBreakMode = LineBreakMode.MiddleTruncation,
                Padding = new Thickness(10, 0)
            };
            var canvasR = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30 };
            canvasR.PaintSurface += skiaPainter.OnCanvasPaintSurfaceR;
            
            var headerButtonAddGroceries = new Button() { ImageSource = "baseline_add_circle_white_24x24.png", WidthRequest = 40, BackgroundColor = colorA2, Margin = new Thickness(0, 8, 0, 0), Padding = 3, CornerRadius = 0 };
            headerButtonAddGroceries.Command = new Command(async () => {
                indicator.IsRunning = true;
                App.isLoading = true;                                
                await Application.Current.MainPage.Navigation.PushAsync(new GroceriesPage(kind));                                                 
                indicator.IsRunning = false;
            });


            var canvasR2 = new SKCanvasView() { HorizontalOptions = LayoutOptions.Fill, WidthRequest = 30 };
            canvasR2.PaintSurface += skiaPainter.OnCanvasPaintSurfaceR2;
            indicator = new ActivityIndicator() { Color = colorA2, IsRunning = false, BackgroundColor = colorGeneralBG, Margin = new Thickness(0) };

            #endregion  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region//     COLLECTION CTOR      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            var cvIngredients = new CollectionView()
            {                
                ItemSizingStrategy = ItemSizingStrategy.MeasureAllItems,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(8, 4), //(8, 0, 8, 8),
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 8 },
                EmptyView = new Label()
                {
                    Text = "No items selected, yet... ",
                    FontSize = fontM,
                    FontAttributes = FontAttributes.Italic,
                    Padding = new Thickness(30, 0, 0, 0),
                    TextColor = Color.White
                }
            };
            cvIngredients.SetBinding(CollectionView.ItemsSourceProperty, itemsSourcePropertyString);
            cvIngredients.SetBinding(CollectionView.HeightRequestProperty, heightRequestPropertyString);
            #endregion//     COLLECTION CTOR     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region //     DATATEMPLATE (Start)   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            cvIngredients.ItemTemplate = new DataTemplate(() =>
            {
                var OutterStack = new StackLayout() { Margin = new Thickness(0, 0, 0, 8) }; //This creates the gaps between collection items
                var OutterFrame = new Frame()
                {                   
                    IsClippedToBounds = true,
                    CornerRadius = 3,
                    Padding = 0,
                    BorderColor = frameBorderColor,
                    Margin = 0

                };

                ///    GRID            \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 
                var dtGrid = new Grid
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0),
                    RowSpacing = 0
                };

                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });


                //These rows match GroceriesPage
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });








                #endregion //     DATATEMPLATE (Start)   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region   // DATATEMPLATE: ICON            \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

                //var iconBG = new BoxView { BackgroundColor = colorA2 };
                //dtGrid.Children.Add(iconBG, 0, 0);
                //Grid.SetRowSpan(iconBG, 4);

                var iIcon = new Image { Margin = 0, VerticalOptions = LayoutOptions.FillAndExpand };
                iIcon.SetBinding(Image.SourceProperty, "IconName");
                iIcon.SetBinding(Image.BackgroundColorProperty, "IconColor1", converter: fromHex);
                dtGrid.Children.Add(iIcon, 0, 0);
            Grid.SetRowSpan(iIcon, 4); // 3);

                var info1String = new Label() { 
                    FontSize = fontB, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                info1String.SetBinding(Label.TextProperty, "Info1String");
                info1String.SetBinding(Label.IsVisibleProperty, "Info1String", converter: ToBeVisible);
                dtGrid.Children.Add(info1String, 0, 2);

                #endregion //     DATATEMPLATE ICON   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region    // DATATEMPLATE: NAME & DESC     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                var slNamesEtc = new StackLayout() { Padding = new Thickness(3, 0, 0, 0), Spacing = -1 };
                Grid.SetRowSpan(slNamesEtc, 3);

                var name1Label = new Label { FontSize = fontM, FontAttributes = FontAttributes.Bold, LineBreakMode = LineBreakMode.MiddleTruncation };
                var name2Label = new Label { FontSize = fontM, LineBreakMode = LineBreakMode.MiddleTruncation };
                var etcLabel = new Label { FontSize = fontB, LineBreakMode = LineBreakMode.MiddleTruncation };

                name1Label.SetBinding(Label.TextProperty, "Name1");
                name2Label.SetBinding(Label.TextProperty, "Name2");
                etcLabel.SetBinding(Label.TextProperty, "EtcString");

                slNamesEtc.Children.Add(name1Label);
                slNamesEtc.Children.Add(name2Label);
                slNamesEtc.Children.Add(etcLabel);
                dtGrid.Children.Add(slNamesEtc, 1, 0);

                #endregion //    NAME & DESC     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region    // DATATEMPLATE: USER SERVING SIZES (Top Right Corner)    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                var userUOMFontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));

                var userUOMStack = new StackLayout() { HorizontalOptions = LayoutOptions.Start, Padding = new Thickness(0, 8, 0, 8) , Spacing = -2, };

                var userUOMP = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                var uomText = new String[2] { "QtyPortion", "UomPortion" };
                var entryP = new IngredientUOMEntry(); 
                    entryP.SetBinding(Entry.TextProperty, uomText[0]);
                var labelP = new IngredientUOMLabel() { TextColor = deafultTextColor };
                    labelP.SetBinding(Label.TextProperty, uomText[1]);


                var userUOMW = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                uomText = new String[2] { "QtyWeight", "UomWeight" };
                var entryW = new IngredientUOMEntry(); 
                    entryW.SetBinding(Entry.TextProperty, uomText[0]);
                    entryW.SetBinding(Entry.IsVisibleProperty, uomText[1], converter: ToBeVisible);
                var labelW = new IngredientUOMLabel() { TextColor = deafultTextColor };
                    labelW.SetBinding(Label.TextProperty, uomText[1]);
                    labelW.SetBinding(Label.IsVisibleProperty, uomText[1], converter: ToBeVisible);


                var userUOMV = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                uomText = new String[2] { "QtyVolume", "UomVolume" };
                var entryV = new IngredientUOMEntry(); 
                    entryV.SetBinding(Entry.TextProperty, uomText[0]);
                    entryV.SetBinding(Entry.IsVisibleProperty, uomText[1], converter: ToBeVisible);
                var labelV = new IngredientUOMLabel() { TextColor = deafultTextColor };
                    labelV.SetBinding(Label.TextProperty, uomText[1]);
                    labelV.SetBinding(Label.IsVisibleProperty, uomText[1], converter: ToBeVisible);


                var userUOMC = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
                uomText = new String[2] { "QtyCount", "UomCount" };
                var entryC = new IngredientUOMEntry(); 
                    entryC.SetBinding(Entry.TextProperty, uomText[0]);
                    entryC.SetBinding(Entry.IsVisibleProperty, uomText[1], converter: ToBeVisible);
                var labelC = new IngredientUOMLabel() { TextColor = deafultTextColor };
                    labelC.SetBinding(Label.TextProperty, uomText[1]);
                    labelC.SetBinding(Label.IsVisibleProperty, uomText[1], converter: ToBeVisible);

                userUOMP.Children.Add(entryP);
                userUOMP.Children.Add(labelP);
                userUOMStack.Children.Add(userUOMP);

                userUOMW.Children.Add(entryW);
                userUOMW.Children.Add(labelW);
                userUOMStack.Children.Add(userUOMW);

                userUOMV.Children.Add(entryV);
                userUOMV.Children.Add(labelV);
                userUOMStack.Children.Add(userUOMV);

                userUOMC.Children.Add(entryC);
                userUOMC.Children.Add(labelC);
                userUOMStack.Children.Add(userUOMC);

                dtGrid.Children.Add(userUOMStack, 2, 0);
                Grid.SetRowSpan(userUOMStack, 4);

                #endregion //    DATATEMPLATE: USER's SIZE INPUT (at Bottom)     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                #region// DATATEMPLATE: WRAP UP    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                ///dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(18) }); //BOTTOM MARGIN
                OutterFrame.Content = dtGrid;
                OutterStack.Children.Add(OutterFrame);
                return OutterStack;

            });
            #endregion//      END DATATEMPLATE        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region //     FOOTER      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            //var footer = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 0, HorizontalOptions = LayoutOptions.Center }; //, BackgroundColor = colorGeneralBG, HeightRequest = 40, Margin = 0 };
            //footer.Children.Add(new IngredientListSummaryLine(kind, isInHeader: false) {BackgroundColor = colorA1, Padding= new Thickness(0,0,0,8)});



            #endregion  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            #region //     COMPOSE ELEMENTS      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            header.Children.Add(canvasL);
            header.Children.Add(headerLabel);
            header.Children.Add(canvasR);
            header.Children.Add(headerButtonAddGroceries);
            header.Children.Add(canvasR2);
            header.Children.Add(indicator);


            Children.Add(header);
            Children.Add(cvIngredients);
            //Children.Add(footer);

            #endregion  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        }





    }
}

