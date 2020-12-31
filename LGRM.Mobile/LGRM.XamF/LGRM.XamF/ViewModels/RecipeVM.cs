using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{

    public class RecipeVM : BaseVM
    {
        ///    MEMBERS    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\  

 
        #region //~~ ObservableCollection<Ingredient> Lists...
        //~~ For Binding Static Lists on this View... 

        private Recipe _recipe { get; set; }
        public Recipe Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
                //OnPropertyChanged("Recipe");
            }

        }

        private ObservableCollection<Ingredient> _myLeans { get; set; }
        private ObservableCollection<Ingredient> _myGreens { get; set; }
        private ObservableCollection<Ingredient> _myHealthyFats { get; set; }
        private ObservableCollection<Ingredient> _myCondiments { get; set; }
        public ObservableCollection<Ingredient> MyLeans
        {
            get => _myLeans;
            set
            {
                _myLeans = value;
                //OnPropertyChanged("MyLeans");
            }
        }
        public ObservableCollection<Ingredient> MyGreens
        {
            get => _myGreens;
            set
            {
                _myGreens = value;
                //OnPropertyChanged("MyGreens");
            }

        }
        public ObservableCollection<Ingredient> MyHealthyFats
        {
            get => _myHealthyFats;
            set
            {
                _myHealthyFats = value;
                //OnPropertyChanged("MyHealthyFats");
            }

        }
        public ObservableCollection<Ingredient> MyCondiments
        {
            get => _myCondiments;
            set
            {
                _myCondiments = value;
                //OnPropertyChanged("MyCondiments");
            }

        }

        #endregion        

        #region //~~ For adjusting collections' heights per items added...
        public int EmptyHeight = 60; 
        //public int IngredientLabelHeight = 180; //Standard Ingredient label height
        //public int HeightOf1UOM = 100;  
        //public int HeightOf2UOMs = 138;         

        #endregion //~~ For adjusting View's collection heights per items added...

        #region //~~ For Recipe Name, Saving, & Updating in header... \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        //private string _recipeName { get; set; }
        public string RecipeName
        {
            get => Recipe.Name;  //_recipeName;
            set
            {
                //_recipeName = value;
                Recipe.Name = value;
                //OnPropertyChanged("RecipeName");

            }
        }

        private int _currentRecipeId { get; set; }
        public int CurrentRecipeId
        {
            get => _currentRecipeId;
            set
            {
                _currentRecipeId = value;
                //OnPropertyChanged("CurrentRecipeId");
            }
        }

        #endregion


        public static IDictionary<int, Kind> IngredientScratchList { get; set; } // This replaced an App-level Recipe helps track what items are already on the RecipeVM instance.
        private int nextIngredientId { get; set; }


        private int ingredientScratchListCount { get; set; }
        public int IngredientScratchListCount 
        {
            get => ingredientScratchListCount;
            set
            {
                IngredientScratchList ??= new Dictionary<int, Kind>();
                ingredientScratchListCount = IngredientScratchList.Count;
                OnPropertyChanged("IngredientScratchListCount");
            }
        }

        ///     CTORs      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


        public RecipeVM()
        {
            Recipe = new Recipe();
            RecipeName = Recipe.Name;
            //RecipeServes = Recipe.Serves;

            App.Groceries ??= new ObservableCollection<Grocery>(App.MySQLite.GetAllGroceries().OrderBy(g => g.Name1));
            MyLeans = new ObservableCollection<Ingredient>();
            MyGreens = new ObservableCollection<Ingredient>();
            MyHealthyFats = new ObservableCollection<Ingredient>();
            MyCondiments = new ObservableCollection<Ingredient>();


            MyLeans.Clear();
            MyGreens.Clear();
            MyHealthyFats.Clear();
            MyCondiments.Clear();

            _myLeans.Clear();
            _myGreens.Clear();
            _myHealthyFats.Clear();
            _myCondiments.Clear();


            IngredientScratchList = new Dictionary<int, Kind>(); // Tells GroceriesVM which items should be pre-selected
            IngredientScratchList.Clear();
            IngredientScratchListCount = IngredientScratchList.Count();



            nextIngredientId = 1;

            //~~ To Receive Updates
            MessagingCenter.Unsubscribe<GroceriesVM, object>(this, "UpdateRecipeIngredients"); // Remove "loose ends" that may be causing redundant calls ???
            MessagingCenter.Subscribe<GroceriesVM, object>(this, "UpdateRecipeIngredients", OnUpdateRecipeIngredients);   // ...from Lists of Groceries

            OnUpdateRecipeIngredientsCalled = 0;
            
            
            ReportCounts("End of RecipeVM() CTOR");
        }






        ///    METHODS    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        private int onUpdateRecipeIngredientsCalled { get; set; }
        public int OnUpdateRecipeIngredientsCalled 
        { 
            get => onUpdateRecipeIngredientsCalled;
            set 
            {
                onUpdateRecipeIngredientsCalled = value;
                OnPropertyChanged("OnUpdateRecipeIngredientsCalled");
            } 
        } 



        public void ReportCounts(string message)
        {
            Console.WriteLine($"~~~{ message                                      }");
            Console.WriteLine($"~~~ Scratch list...  { IngredientScratchList.Count}");
            Console.WriteLine($"~~~ MyLeans      ... {   MyLeans.Count()          }");
            Console.WriteLine($"~~~ MyGreens     ... {   MyGreens.Count()         }");
            Console.WriteLine($"~~~ MyHealthyFats... {   MyHealthyFats.Count()    }");
            Console.WriteLine($"~~~ MyCondiments ... {   MyCondiments.Count()     }");
            Console.WriteLine($"~~~ Update Called... {OnUpdateRecipeIngredientsCalled}");
            
        }
        



        public void OnUpdateRecipeIngredients(object sender, object affectedCatalogNumber)
        {
            OnUpdateRecipeIngredientsCalled++;  // This number will show as all calls that were ever made, but binding only shows the current instance's (watch in debugger)
            ReportCounts("Begin RecipeVM Update... ");
            StackTrace stackTrace = new StackTrace();
            Console.WriteLine("~~~ stackTrace.GetFrame(1).GetMethod().Name... " + stackTrace.GetFrame(1).GetMethod().Name);           


                var ingredientEffected = new Ingredient(App.Groceries.FirstOrDefault(g => g.CatalogNumber == (int)affectedCatalogNumber)); //affectedCatalogNumber));
                int countGroceries = (sender as GroceriesVM).MySelectedItems.Count();
                            
                var countRecipe = ingredientEffected.Kind switch
                {  
                    // These list.Counts will show as total of all selections that were ever made, not just curent Recipe instance!
                    Kind.Lean => this.MyLeans.Count,
                    Kind.Green => MyGreens.Count,
                    Kind.HealthyFat => MyHealthyFats.Count,
                    Kind.Condiment => MyCondiments.Count,
                    _ => throw new NotImplementedException()
                };

                ReportCounts($"~~~ countRecipe({countRecipe}) != countGroceries({countGroceries}) ... {countRecipe != countGroceries}");
                Console.WriteLine($"~~~");

                if (countRecipe != countGroceries)
                {
                    bool included = IngredientScratchList.Any(entry => entry.Key == ingredientEffected.CatalogNumber);

                    if (!included) // Add ingredient...
                    {
                        ingredientEffected.IngredientId = nextIngredientId;
                        nextIngredientId++;
                        switch (ingredientEffected.Kind)
                        {
                            case Kind.Lean:
                                MyLeans.Add(ingredientEffected);
                                break;
                            case Kind.Green:
                                MyGreens.Add(ingredientEffected);
                                break;
                            case Kind.HealthyFat:
                                MyHealthyFats.Add(ingredientEffected);
                                break;
                            case Kind.Condiment:
                                MyCondiments.Add(ingredientEffected);
                                break;
                        }
                        IngredientScratchList.Add(ingredientEffected.CatalogNumber, ingredientEffected.Kind);

                    }
                    else  // Remove ingredient...
                    {
                        switch (ingredientEffected.Kind)
                        {
                            case Kind.Lean:
                                MyLeans.Remove(MyLeans.FirstOrDefault(ing => ing.CatalogNumber == ingredientEffected.CatalogNumber));
                                break;
                            case Kind.Green:
                                MyGreens.Remove(MyGreens.FirstOrDefault(ing => ing.CatalogNumber == ingredientEffected.CatalogNumber));
                                break;
                            case Kind.HealthyFat:
                                MyHealthyFats.Remove(MyHealthyFats.FirstOrDefault(ing => ing.CatalogNumber == ingredientEffected.CatalogNumber));
                                break;
                            case Kind.Condiment:
                                MyCondiments.Remove(MyCondiments.FirstOrDefault(ing => ing.CatalogNumber == ingredientEffected.CatalogNumber));
                                break;
                        }
                        IngredientScratchList.Remove(ingredientEffected.CatalogNumber);

                    }


                    ReportCounts("OnUpdateRecipeIngredients End (currentCount != updatedCount)");
                } //  if (currentCount != updatedCount)


        }






    }

}
