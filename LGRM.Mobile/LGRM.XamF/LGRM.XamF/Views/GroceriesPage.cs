using LGRM.Model;
using LGRM.XamF.ViewModels;
using Xamarin.Forms;

namespace LGRM.XamF.Views
{
    public class GroceriesPage : ContentPage
    {
        public Kind kind;

        Color frameBorderColor = Color.LightSlateGray;
        public Color colorA1;
        public Color colorA2;
        public Color colorEntryBG;

        public GroceriesPage(Kind kind)
        {
            BindingContext = new GroceriesVM(kind);

            #region Colors...
            Application.Current.Resources.TryGetValue("LocalConvertStringToHexColor", out var resourceValue);
            var fromHex = (IValueConverter)resourceValue;
            Application.Current.Resources.TryGetValue("LocalConvertStringToBool", out resourceValue);
            var ToBeVisible = (IValueConverter)resourceValue;
            Application.Current.Resources.TryGetValue("GeneralBG", out resourceValue);
            var colorGeneralBG = (Color)resourceValue;
            frameBorderColor = Color.LightGray;
            
            switch (kind)
            {
                case Kind.Lean:                    
                    Title = "Add Leans...";
                    Application.Current.Resources.TryGetValue("LeansA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("LeansA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    break;
                case Kind.Green:
                    Title = "Add Greens...";
                    Application.Current.Resources.TryGetValue("GreensA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("GreensA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    break;
                case Kind.HealthyFat:
                    Title = "Add Healthy Fats...";
                    Application.Current.Resources.TryGetValue("HealthyFatsA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("HealthyFatsA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    break;
                case Kind.Condiment:
                    Title = "Add Condiments...";
                    Application.Current.Resources.TryGetValue("CondimentsA1", out resourceValue);
                    colorA1 = (Color)resourceValue;
                    Application.Current.Resources.TryGetValue("CondimentsA2", out resourceValue);
                    colorA2 = (Color)resourceValue;
                    break;
                default:
                    break;
            }
            Application.Current.Resources.TryGetValue("DefaultEntryBG", out resourceValue);
            colorEntryBG = (Color)resourceValue;
            #endregion 
            #region Font Sizes...
            var fontL = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            var fontM = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            var fontB = Device.GetNamedSize(NamedSize.Body, typeof(Label));
            #endregion Font Sizes...

            //~~~ Toolbar...
            var ShowSelectedItemsToolbar = new ToolbarItem() { IconImageSource = ImageSource.FromFile("no_icon_18dp.png") }; 
            ShowSelectedItemsToolbar.SetBinding(ToolbarItem.IconImageSourceProperty, "ShowSelectedItemsButtonIcon");
            ShowSelectedItemsToolbar.SetBinding(ToolbarItem.CommandProperty, "ShowSelectedItemsCommand");

            ToolbarItems.Add(ShowSelectedItemsToolbar);


            //~~~ Main Stack...
            var MainStack = new StackLayout();
            MainStack.BackgroundColor = colorA1;
            Content = MainStack;


            //~~~ Search & Sort Header...
            var searchStack = new StackLayout() { 
                Orientation = StackOrientation.Horizontal, 
            };

            var pCategory = new Picker {
                BackgroundColor = colorEntryBG,
                Margin = new Thickness(0,0,4,0)
            };
            pCategory.SetBinding(Picker.ItemsSourceProperty, "Categories");
            pCategory.SetBinding(Picker.SelectedItemProperty, "SelectedCategory");


            var searchBar = new SearchBar
            {
                Placeholder = "Search...",
                WidthRequest = 205,
                Margin = new Thickness(4,0,0,0),
                TextTransform = TextTransform.Lowercase,
                BackgroundColor = colorEntryBG
            };
            searchBar.SetBinding(SearchBar.TextProperty, "SearchQuery");


            searchStack.Children.Add(pCategory);
            searchStack.Children.Add(searchBar);

            MainStack.Children.Add(searchStack);

            //~~~ CollectionView...
            var cvGroceries = new CollectionView()
            {
                ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(8, 4),
                SelectionMode = SelectionMode.Multiple,                
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 8 }

            };
            cvGroceries.SetBinding(CollectionView.ItemsSourceProperty, "Groceries");
            cvGroceries.SetBinding(CollectionView.SelectedItemsProperty, "MySelectedItems", BindingMode.TwoWay);
            cvGroceries.SetBinding(CollectionView.SelectionChangedCommandProperty, "MySelectionChangedCommand");
            cvGroceries.SetBinding(CollectionView.FooterProperty, "FooterText");


            //     DATATEMPLATE    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            cvGroceries.ItemTemplate = new DataTemplate(() =>
            {
                var OutterStack = new StackLayout() { Margin = new Thickness(0, 0, 0, 8) };
                var OutterFrame = new Frame()
                {
                    IsClippedToBounds = true,
                    CornerRadius = 3,
                    Padding = 0,
                    BorderColor = frameBorderColor,
                    Margin = 0
                };

                ///    VISUAL STATES   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ https://forums.xamarin.com/discussion/179464/visual-state-manager-setter-in-code-behind

                var vsgF = new VisualStateGroup() { Name = "vsgF" };
                var vsNormalF = new VisualState { Name = "Normal" };
                var vsSelectedF = new VisualState { Name = "Selected" };

                vsNormalF.Setters.Add(new Setter { Property = Frame.BackgroundColorProperty, Value = Color.White });
                vsSelectedF.Setters.Add(new Setter { Property = Frame.BackgroundColorProperty, Value = Color.LightBlue });

                vsgF.States.Add(vsNormalF);
                vsgF.States.Add(vsSelectedF);

                VisualStateManager.GetVisualStateGroups(OutterFrame).Add(vsgF);

                ///    GRID            \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\       
                var dtGrid = new Grid
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0),
                    RowSpacing = 0
                };

                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });//(78) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                dtGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(28) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22) });
                dtGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });


                ///    ICON         \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\       
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


                #region    //    DATATEMPLATE: NAME & DESC     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
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


                ///    STANDARD SERVING SIZE     \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 
                var slServingStandards = new StackLayout() { HorizontalOptions = LayoutOptions.Center, Padding = new Thickness(0, 8, 0, 0) }; //Spacing = -1, 
                dtGrid.Children.Add(slServingStandards, 2, 0);
                Grid.SetRowSpan(slServingStandards, 3);

                var standardServingFontSize = Device.GetNamedSize(NamedSize.Body, typeof(Label));


                var standardServing1 = new Label() { FontSize = standardServingFontSize, HorizontalTextAlignment = TextAlignment.Start };
                standardServing1.SetBinding(Label.TextProperty, "WeightServing");
                standardServing1.SetBinding(Label.IsVisibleProperty, "WeightServing", converter: ToBeVisible);

                var standardServing2 = new Label() { FontSize = standardServingFontSize, HorizontalTextAlignment = TextAlignment.Start };
                standardServing2.SetBinding(Label.TextProperty, "VolumeServing");
                standardServing2.SetBinding(Label.IsVisibleProperty, "VolumeServing", converter: ToBeVisible);

                var standardServing3 = new Label() { FontSize = standardServingFontSize, HorizontalTextAlignment = TextAlignment.Start };
                standardServing3.SetBinding(Label.TextProperty, "CountServing");
                standardServing3.SetBinding(Label.IsVisibleProperty, "CountServing", converter: ToBeVisible);

                slServingStandards.Children.Add(standardServing1);
                slServingStandards.Children.Add(standardServing2);
                slServingStandards.Children.Add(standardServing3);

                ///    WRAP UP    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ 
                ///    
                OutterFrame.Content = dtGrid;
                return OutterFrame;

            });
            ///    ... end DataTemplate      //////////////////////////////////////////////////////////

            App.isLoading = false; // This is also set at the end of GroceriesVM (I assume this one is more important)
            MainStack.Children.Add(cvGroceries);


        }

        

    }
}

