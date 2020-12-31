using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class GroceriesVM : BaseVM
    {
        ///    MEMBERS      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\      

        //~~ for CTOR...
        Kind kind { get; set; } //Determins "Kind of grocery" this patge displays

        //~~ Data Source...        
        //ObservableCollection<Grocery> _groceries;
        public ObservableCollection<Grocery> Groceries { get; set; }
        //{
        //    get => _groceries;
        //    set => _groceries = value;
        //}


        //ObservableCollection<object> _mySelectedItems;
        public ObservableCollection<object> MySelectedItems { get; set; } // for SelectedItemsProperty
        //{
        //    get => _mySelectedItems;
        //    set 
        //    {
        //        //if (_mySelectedItems != value)
        //        //{
        //            _mySelectedItems = value;
        //        //}
        //    }
        //}

        private List<int> priorSelections { get; set; } 

        public ICommand MySelectionChangedCommand { get; set; }


        ///      CTOR       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public GroceriesVM(Kind myKind)
        {
            //App.isLoading = true;
            App.Groceries ??= new ObservableCollection<Grocery>(App.MySQLite.GetAllGroceries().OrderBy(g => g.Name1));

            RecipeVM.IngredientScratchList ??= new Dictionary<int, Kind>();

            //~~ Data Source...
            this.kind = myKind;
            Groceries = new ObservableCollection<Grocery>();
            foreach (var item in App.Groceries.Where(g => g.Kind == kind))
            {
                Groceries.Add(item);
            }


            //~~ Manage Item Selections...
            MySelectionChangedCommand = new Command<object>(OnMySelectionChangedCommand);
            MySelectedItems = new ObservableCollection<object>() { };

            priorSelections = new List<int>(); // Tells GroceriesVM which items should be pre-selected
            foreach (KeyValuePair<int, Kind> entry in RecipeVM.IngredientScratchList)
            {
                if (entry.Value == myKind)
                {
                    priorSelections.Add(entry.Key);
                }
            }

            var preselected = new List<Grocery>();
            for (int i = 0; i < priorSelections.Count; i++)
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
            }

            ///    FOR DEBUGGING      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\       
            OnMySelectionChangedCommandCalled = 0;
        }
        private int onMySelectionChangedCommandCalled { get; set; }
        public int OnMySelectionChangedCommandCalled
        {
            get => onMySelectionChangedCommandCalled;
            set
            {
                onMySelectionChangedCommandCalled = value;
                OnPropertyChanged("OnMySelectionChangedCommandCalled");
            }
        }




        ///    METHODS      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\        

        private void OnMySelectionChangedCommand(object obj)
        {
            OnMySelectionChangedCommandCalled++;

            if ( priorSelections.Count != MySelectedItems.Count )
            {
                int AffectedCatalogNumber; // CatalogNumber is a unique product Id, so there should never be more than 1 of each in any list.  This number will be sent to the Recipe Page.
                if (priorSelections.Count == 0) // This must be an "Add"
                {
                    AffectedCatalogNumber = ((Grocery)MySelectedItems[0]).CatalogNumber;
                    priorSelections.Add(AffectedCatalogNumber);                    
                }
                else  // or Find the 1 AffectedCatalogNumber out of the lists...
                {
                    var updatedSelections = new List<int>();
                    foreach (var item in MySelectedItems) 
                    { 
                        updatedSelections.Add(((Grocery)item).CatalogNumber); 
                    } 

                    if (MySelectedItems.Count > priorSelections.Count) // add item ...
                    {
                        AffectedCatalogNumber = updatedSelections.Except(priorSelections).ToList()[0]; // ... list should only have 1 item
                        priorSelections.Add(AffectedCatalogNumber);
                    }
                    else // (MySelectedItems.Count < priorSelections.Count) remove item ...
                    {
                        AffectedCatalogNumber = priorSelections.Except(updatedSelections).ToList()[0]; // ... list should only have 1 item
                        priorSelections.Remove(AffectedCatalogNumber);
                    }
                }

                MessagingCenter.Send<GroceriesVM, object>(this, "UpdateRecipeIngredients", AffectedCatalogNumber);

            }
        }


        public ObservableCollection<Grocery> GetGroceries(Kind kind, string query = "")
        {
            return new ObservableCollection<Grocery>(App.Groceries.Where(g => g.Kind == kind).ToList().OrderBy(g => g.Name1));
        }


    }

}
 
