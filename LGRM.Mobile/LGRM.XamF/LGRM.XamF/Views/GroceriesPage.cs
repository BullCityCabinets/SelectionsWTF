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


            //~~~ Main Stack...
            var MainStack = new StackLayout();
            Content = MainStack;

            

            var debugStack = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center };

            var updateCountLabelStart = new Label() { Text = "Update Called " };
            var updateCountLabel = new Label()
            {
                VerticalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(10, 0)
            };
            updateCountLabel.SetBinding(Label.TextProperty, "OnMySelectionChangedCommandCalled");
            var updateCountLabelEnd = new Label() { Text = " times !" };

            debugStack.Children.Add(updateCountLabelStart);
            debugStack.Children.Add(updateCountLabel);
            debugStack.Children.Add(updateCountLabelEnd);




            //~~~ CollectionView...
            var cvGroceries = new CollectionView()
            {
                ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(8, 4),
                SelectionMode = SelectionMode.Multiple,                
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 8 },
                EmptyView = new Label()
                {
                    Text = "If no items load, please uninstall and re-install the program, thank-you-very-much",
                    FontSize = fontM
                }
            };
            cvGroceries.SetBinding(CollectionView.ItemsSourceProperty, "Groceries");

            //cvGroceries.SetBinding(CollectionView.SelectedItemsProperty, "MySelectedItems", BindingMode.TwoWay);
            cvGroceries.SetBinding(CollectionView.SelectedItemsProperty, "MySelectedItems", BindingMode.OneWay);

            cvGroceries.SetBinding(CollectionView.SelectionChangedCommandProperty, "MySelectionChangedCommand");


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
                    Margin = 0,
                    BackgroundColor = Color.Transparent
                };


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

            //App.isLoading = false; // This is also set at the end of GroceriesVM (I assume this one is more important)
            
            MainStack.Children.Add(debugStack);
            MainStack.Children.Add(cvGroceries);


        }

        

    }
}

