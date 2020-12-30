using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class GroceriesVM : BaseVM
    {
        ///    MEMBERS      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\      

        Kind kind { get; set; }

        //~~ Titles & Styles...
        string TitleText { get; set; }
        string FooterText
        {
            get => App.V.FooterText;
            set
            {
                FooterText = value;
                OnPropertyChanged("FooterText");
            }
        }

        //~~ Data Source...        
        ObservableCollection<Grocery> _groceries;
        public ObservableCollection<Grocery> Groceries
        {
            get => _groceries;
            set
            {
                _groceries = value;
                OnPropertyChanged("Groceries");
            }
        }

        //~~ Filter by Category...
        public List<string> Categories { get; set; }
        object selectedCategory { get; set; }
        public object SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                Groceries = GetGroceries(kind, searchQuery);                
                OnPropertyChanged("SelectedCategory");
                
            }
        }

        //~~ Selections, Search, & Filters...
        ObservableCollection<object> _mySelectedItems; //To capture View's binding input
        public ObservableCollection<object> MySelectedItems
        {
            get => _mySelectedItems;
            set
            {
                if (_mySelectedItems != value)
                {
                    _mySelectedItems = value;
                    OnPropertyChanged("MySelectedItems");
                }
            }
        }        
        private List<int> priorSelections { get; set; }
        public ICommand MySelectionChangedCommand { get; set; }

        public ICommand ShowSelectedItemsCommand { get; set; }
        public bool IsShowingSelectedItems { get; set; }
        string showSelectedItemsButtonIcon { get; set; }
        public string ShowSelectedItemsButtonIcon
        {
            get => showSelectedItemsButtonIcon;
            set
            {
                showSelectedItemsButtonIcon = value;
                OnPropertyChanged("ShowSelectedItemsButtonIcon");
            }
        }
        public void SetShowSelectedItemsButton()
        {
            ShowSelectedItemsButtonIcon = MySelectedItems.Count > 0 ?
                (IsShowingSelectedItems ? "show_all_18dp.png" : "show_selected_18dp.png")
                : "no_icon_18dp.png";
        }

        public ICommand SearchCommand { get; }
        

        ///      CTOR       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public GroceriesVM(Kind myKind)
        {
            App.isLoading = true;
            App.Groceries ??= new ObservableCollection<Grocery>(App.MySQLite.GetAllGroceries().OrderBy(g => g.Name1));

            RecipeVM.IngredientScratchList ??= new Dictionary<int, Kind>();

            //~~ Filters...
            this.kind = myKind;
            Groceries = new ObservableCollection<Grocery>();           

            //~~ Data Source...
            foreach (var item in App.Groceries.Where(g => g.Kind == kind))
            {
               Groceries.Add(item);
            }

            //~~ Filter by Category...
            Categories = new List<string>();
            Categories.Insert(0, App.V.CategoriesPickerDefault);
            foreach (var c in Groceries.Select(g => g.Category).Distinct().ToList())
            {
                Categories.Add(c);
            }
            SelectedCategory = Categories[0]; // = "Show All"

            //~~ Search Text...
            SearchCommand = new Command<string>(OnSearchCommand);

            ShowSelectedItemsCommand = new Command(OnShowSelectedItemsCommand);

            //~~ Manage Item Selections...
            MySelectionChangedCommand = new Command<object>(OnMySelectionChangedCommand);
            MySelectedItems = new ObservableCollection<object>() { };

            priorSelections = new List<int>();
            foreach (KeyValuePair<int, Kind> entry in RecipeVM.IngredientScratchList)
            {
                if (entry.Value == myKind)
                {
                    priorSelections.Add(entry.Key);
                }
            }

            var preselected = new List<Grocery>();
            for (int i = 0; i < priorSelections.Count ; i++)
            {
                var g = App.Groceries.FirstOrDefault(g => g.CatalogNumber == priorSelections[i]);
                preselected.Add(g);
            }
            if (preselected.Count() > 0)
            {
                foreach (var ing in preselected)
                {
                    foreach (var g in Groceries)
                    {
                        if (ing.CatalogNumber == g.CatalogNumber)
                        {
                            var index = Groceries.IndexOf(g);
                            MySelectedItems.Add(Groceries[index]);
                        }
                    }
                }
                SetShowSelectedItemsButton();
            }
            App.isLoading = false;

            /// App.isLoading = false; moved to end of GroceriesPage CTOR.
        }




        ///    METHODS      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\        



        private void OnMySelectionChangedCommand(object obj)
        {
            StackTrace stackTrace = new StackTrace();
            Console.WriteLine("~~~"+stackTrace.GetFrame(1).GetMethod().Name);
            Console.WriteLine("~~~" + stackTrace.GetFrame(1).GetMethod().Name);

            if (!App.isLoading && priorSelections.Count != MySelectedItems.Count)
            {
                App.isLoading = true;
                var updatedSelections = new List<int>();
                foreach (var item in MySelectedItems) { updatedSelections.Add(((Grocery)item).CatalogNumber); }

                int AffectedCatalogNumber;

                if (priorSelections.Count == 0) // If there was only 1 item to select...
                { 
                    AffectedCatalogNumber = updatedSelections[0];
                    priorSelections.Add(AffectedCatalogNumber);
                    App.isLoading = false;

                    int[] data = new int[] { AffectedCatalogNumber, MySelectedItems.Count };
                    MessagingCenter.Send<GroceriesVM, object>(this, "UpdateRecipeIngredients", data);
                    //Console.WriteLine("~~~ Groceries LastSelectionDate " + LastSelectionDate.ToString());
                }
                else  // or Find the 1 AffectedCatalogNumber out of the lists...
                {                    
                    if (updatedSelections.Count > priorSelections.Count) // add item ...
                    {
                        AffectedCatalogNumber = updatedSelections.Except(priorSelections).ToList()[0]; // ... should only be 1 item
                        priorSelections.Add(AffectedCatalogNumber);
                    }
                    else // (updatedSelections.Count < priorSelections.Count) // remove item ...
                    {
                        AffectedCatalogNumber = priorSelections.Except(updatedSelections).ToList()[0]; // ... should only be 1 item
                        priorSelections.Remove(AffectedCatalogNumber);
                    }
                    App.isLoading = false;

                    int[] data = new int[] { AffectedCatalogNumber, MySelectedItems.Count };
                    MessagingCenter.Send<GroceriesVM, object>(this, "UpdateRecipeIngredients", data);
                }                
                SetShowSelectedItemsButton();
                
            }
        }


        private void OnShowSelectedItemsCommand(object obj)
        {
            if (MySelectedItems.Count > 0)
            {
                if (!IsShowingSelectedItems)
                {
                    Groceries.Clear();
                    foreach (var s in MySelectedItems)
                    {
                        Groceries.Add((Grocery)s);
                    }
                    IsShowingSelectedItems = true;
                    SetShowSelectedItemsButton();
                }
                else
                {
                    Groceries = GetGroceries(kind, searchQuery);
                    IsShowingSelectedItems = false;
                    SetShowSelectedItemsButton();
                }
            }

        }


        //~~ Search Text...
        private string searchQuery { get; set; }
        public string SearchQuery
        {
            get => searchQuery;            
            set
            {
                searchQuery = value;
                OnPropertyChanged("SearchQuery");
                OnSearchCommand(searchQuery);
            }
        }
        public void OnSearchCommand(string query)
        {
            try
            {
                Groceries = GetGroceries(kind, query);
            }
            catch (Exception e)
            {
                ReportException(e);
            }
        }


        public ObservableCollection<Grocery> GetGroceries(Kind kind, string query = "") 
        {
            var result = new ObservableCollection<Grocery>();

            if(string.IsNullOrEmpty(query))
            {
                if(SelectedCategory.ToString() == Categories[0])
                {
                    // by Kind only ....
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == kind
                        ).ToList().OrderBy(g => g.Name1));
                }
                else // by Category ...
                {
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind     == kind 
                                              && g.Category == SelectedCategory.ToString() 
                                              ).ToList().OrderBy(g => g.Name1));
                }
            }
            else // by Query ...
            {
                query = query.ToLower().Trim();

                if (SelectedCategory.ToString() == Categories[0])
                {
                    // by Query ...
                    result = new ObservableCollection<Grocery>(
                        App.Groceries.Where(g => g.Kind == kind
                                            && ( g.Name1.ToLowerInvariant().Contains(query) ||
                                                 g.Name2.ToLowerInvariant().Contains(query) ||
                                                 g.Desc1.ToLowerInvariant().Contains(query))
                                                 ).ToList().OrderBy(g => g.Name1));
                }
                else // by Query & Category ...
                {
                    result = new ObservableCollection<Grocery>(                                                 
                        App.Groceries.Where(g => g.Kind == kind
                                            &&   g.Category == SelectedCategory.ToString()
                                            && ( g.Name1.ToLowerInvariant().Contains(query) ||
                                                 g.Name2.ToLowerInvariant().Contains(query) ||
                                                 g.Desc1.ToLowerInvariant().Contains(query))
                                                 ).ToList().OrderBy(g => g.Name1));
                }

            }
            return result.Count > 0 ? result : new ObservableCollection<Grocery>(App.Groceries.Where(g => g.Kind == kind).ToList().OrderBy(g => g.Name1));
        }

        



    }

}
