using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
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
                OnPropertyChanged("Recipe");
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
                OnPropertyChanged("MyLeans");
            }
        }
        public ObservableCollection<Ingredient> MyGreens
        {
            get => _myGreens;
            set
            {
                _myGreens = value;
                OnPropertyChanged("MyGreens");
            }

        }
        public ObservableCollection<Ingredient> MyHealthyFats
        {
            get => _myHealthyFats;
            set
            {
                _myHealthyFats = value;
                OnPropertyChanged("MyHealthyFats");
            }

        }
        public ObservableCollection<Ingredient> MyCondiments
        {
            get => _myCondiments;
            set
            {
                _myCondiments = value;
                OnPropertyChanged("MyCondiments");
            }

        }

        #endregion        

        #region //~~ For adjusting collections' heights per items added...

        public int EmptyHeight = 60; 
        public int HeightOf1UOM = 100;  // 102
        public int HeightOf2UOMs = 138; // 150

        public int IngredientLabelHeight = 180; //Standard Ingredient label height ... 150 was too short
        public int CollViewMargin = 12; //Standard Collection Margins

        private int heightL { get; set; }
        public int HeightL
        {
            get => heightL;
            set
            {
                if (MyLeans.Count > 0)
                {
                    int h = 0;
                    foreach (var ing in MyLeans)
                    {                        
                        var x = (byte)ing.UOMs;
                        h += x > 2 ? HeightOf2UOMs : HeightOf1UOM;                        
                    }
                    heightL = h;
                }
                else
                {
                    heightL = EmptyHeight;
                }                
                OnPropertyChanged("HeightL");
            }
        }
        private int heightG { get; set; }
        public int HeightG
        {
            get => heightG;
            set
            {
                if (MyGreens.Count > 0)
                {
                    int h = 0;
                    foreach (var ing in MyGreens)
                    {
                        var x = (byte)ing.UOMs;
                        h += x > 2 ? HeightOf2UOMs : HeightOf1UOM;
                    }
                    heightG = h;
                }
                else
                {
                    heightG = EmptyHeight;
                }
                OnPropertyChanged("HeightG");
            }
        }
        private int heightH { get; set; }
        public int HeightH
        {
            get => heightH;
            set
            {
                if (MyHealthyFats.Count > 0)
                {
                    int h = 0;
                    foreach (var ing in MyHealthyFats)
                    {
                        var x = (byte)ing.UOMs;
                        h += x > 2 ? HeightOf2UOMs : HeightOf1UOM;
                    }
                    heightH = h;
                }
                else
                {
                    heightH = EmptyHeight;
                }
                OnPropertyChanged("HeightH");
            }
        }
        private int heightC { get; set; }
        public int HeightC
        {
            get => heightC;
            set
            {
                if (MyCondiments.Count > 0)
                {
                    int h = 0;
                    foreach (var ing in MyCondiments)
                    {
                        var x = (byte)ing.UOMs;
                        h += x > 2 ? HeightOf2UOMs : HeightOf1UOM;
                    }
                    heightC = h;
                }
                else
                {
                    heightC = EmptyHeight;
                }
                OnPropertyChanged("HeightC");
            }
        }
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
                OnPropertyChanged("RecipeName");

            }
        }

        private int _currentRecipeId { get; set; }
        public int CurrentRecipeId
        {
            get => _currentRecipeId;
            set
            {
                _currentRecipeId = value;
                OnPropertyChanged("CurrentRecipeId");
            }
        }

        #endregion

        #region //~~ For Recipe summary of contents in header...       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        private float _recipeServes { get; set; }
        public float RecipeServes
        {
            get => _recipeServes;
            set
            {
                _recipeServes = value;
                UpdatePerRecipeServings();
                OnPropertyChanged("RecipeServes");
            }
        }

        private float _totalLs { get; set; }
        private float _totalGs { get; set; }
        private float _totalHs { get; set; }
        private float _totalCs { get; set; }

        public float TotalLs
        {
            get => _totalLs;
            set
            {
                _totalLs = GetPortionByKind(Kind.Lean);
                SetRecommendedHs();
                BGStateL = SetSummaryBackgroundColor(Kind.Lean);
                OnPropertyChanged("TotalLs");
            }

        }
        public float TotalGs
        {
            get => _totalGs;
            set
            {
                _totalGs = GetPortionByKind(Kind.Green);
                BGStateG = SetSummaryBackgroundColor(Kind.Green);
                OnPropertyChanged("TotalGs");
            }

        }
        public float TotalHs
        {
            get => _totalHs;
            set
            {
                _totalHs = GetPortionByKind(Kind.HealthyFat);
                BGStateH = SetSummaryBackgroundColor(Kind.HealthyFat);
                OnPropertyChanged("TotalHs");
            }

        }
        public float TotalCs
        {
            get => _totalCs;
            set
            {
                _totalCs = GetPortionByKind(Kind.Condiment);
                BGStateC = SetSummaryBackgroundColor(Kind.Condiment);
                OnPropertyChanged("TotalCs");
            }

        }

        private float _recommendedLs { get; set; }
        private float _recommendedGs { get; set; }
        private float _recommendedHs { get; set; }
        private float _recommendedCs { get; set; }

        public float RecommendedLs
        {
            get => _recommendedLs;
            set
            {
                _recommendedLs = value;
                BGStateL = SetSummaryBackgroundColor(Kind.Lean);
                OnPropertyChanged("RecommendedLs");
            }

        }
        public float RecommendedGs
        {
            get => _recommendedGs;
            set
            {
                _recommendedGs = value;
                BGStateG = SetSummaryBackgroundColor(Kind.Green);
                OnPropertyChanged("RecommendedGs");
            }

        }
        public float RecommendedHs
        {
            get => _recommendedHs;
            set
            {
                _recommendedHs = value;
                BGStateH = SetSummaryBackgroundColor(Kind.HealthyFat);
                OnPropertyChanged("RecommendedHs");
            }

        }
        public float RecommendedCs
        {
            get => _recommendedCs;
            set
            {
                _recommendedCs = value;
                BGStateC = SetSummaryBackgroundColor(Kind.Condiment);
                OnPropertyChanged("RecommendedCs");
            }

        }

        private float bgStateL { get; set; }
        public float BGStateL
        {
            get => bgStateL;
            set
            {
                bgStateL = value;
                OnPropertyChanged("BGStateL");
            }
        }
        private float bgStateG { get; set; }
        public float BGStateG
        {
            get => bgStateG;
            set
            {
                bgStateG = value;
                OnPropertyChanged("BGStateG");
            }
        }
        private float bgStateH { get; set; }
        public float BGStateH
        {
            get => bgStateH;
            set
            {
                bgStateH = value;
                OnPropertyChanged("BGStateH");
            }
        }
        private float bgStateC { get; set; }
        public float BGStateC
        {
            get => bgStateC;
            set
            {
                bgStateC = value;
                OnPropertyChanged("BGStateC");
            }
        }

        private float SetSummaryBackgroundColor(Kind kind) //ConverterToEvaluateState sets actual colors
        {
            var x = kind switch
            {
                Kind.Lean =>        new float[] { TotalLs, RecommendedLs },
                Kind.Green =>       new float[] { TotalGs, RecommendedGs },
                Kind.HealthyFat =>  new float[] { TotalHs, RecommendedHs },
                Kind.Condiment =>   new float[] { TotalCs, RecommendedCs },
                _ => throw new NotImplementedException(),
            };

            return x[0] - x[1];  //ConverterToEvaluateState uses this to determine background color
        }

        #endregion

        public ICommand VerifyClearRecipeCommand { get; set; }
        public ICommand SaveRecipeCommand { get; set; }

        public static IDictionary<int, Kind> IngredientScratchList { get; set; }
        private int nextIngredientId { get; set; }

        


        ///     CTORs      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


        public RecipeVM()
        {
            Recipe = new Recipe();
            RecipeName = Recipe.Name;
            RecipeServes = Recipe.Serves;

            App.Groceries ??= new ObservableCollection<Grocery>(App.MySQLite.GetAllGroceries().OrderBy(g => g.Name1));
            MyLeans = new ObservableCollection<Ingredient>();
            MyGreens = new ObservableCollection<Ingredient>();
            MyHealthyFats = new ObservableCollection<Ingredient>();
            MyCondiments = new ObservableCollection<Ingredient>();

            ReportCounts("CTOR 1");
            Console.WriteLine($"~~~");

            MyLeans.Clear();
            MyGreens.Clear();
            MyHealthyFats.Clear();
            MyCondiments.Clear();

            _myLeans.Clear();
            _myGreens.Clear();
            _myHealthyFats.Clear();
            _myCondiments.Clear();

            ReportCounts("CTOR 2");

            IngredientScratchList = new Dictionary<int, Kind>();
            nextIngredientId = 1;

            //~~ To alert lists when their contents change (Xamarin doens't do this, naturally) https://stackoverflow.com/questions/1427471/observablecollection-not-noticing-when-item-in-it-changes-even-with-inotifyprop
            MyLeans.CollectionChanged += MyCollectionChanged;
            MyGreens.CollectionChanged += MyCollectionChanged;
            MyHealthyFats.CollectionChanged += MyCollectionChanged;
            MyCondiments.CollectionChanged += MyCollectionChanged;

            RecommendedLs = 1;
            RecommendedGs = 3;
            RecommendedHs = SetRecommendedHs();
            RecommendedCs = 3;

            //~~ To Receive Updates
            MessagingCenter.Unsubscribe<GroceriesVM, object>(this, "UpdateRecipeIngredients");
            MessagingCenter.Subscribe<GroceriesVM, object>(this, "UpdateRecipeIngredients", OnUpdateRecipeIngredients);   // ...from Lists of Groceries

            VerifyClearRecipeCommand = new Command(OnVerifyClearRecipeCommand);
            //SaveRecipeCommand = new Command(OnSaveRecipeCommand);            

        }







        //public RecipeVM(int RecipeToLoadId)        
        //{
        //    Recipe = new Recipe();
        //    Recipe = App.MySQLite.GetRecipeById(RecipeToLoadId);
        //    RecipeName = Recipe.Name;
        //    RecipeServes = Recipe.Serves;

        //    App.Groceries ??= new ObservableCollection<Grocery>(App.MySQLite.GetAllGroceries().OrderBy(g => g.Name1));
        //    MyLeans = new ObservableCollection<Ingredient>();
        //    MyGreens = new ObservableCollection<Ingredient>();
        //    MyHealthyFats = new ObservableCollection<Ingredient>();
        //    MyCondiments = new ObservableCollection<Ingredient>();

        //    MyLeans.Clear();
        //    MyGreens.Clear();
        //    MyHealthyFats.Clear();
        //    MyCondiments.Clear();

        //    IngredientScratchList = new Dictionary<int, Kind>();
        //    nextIngredientId = Recipe.Ingredients.Count + 1;

        //    HeightL = MyLeans.Count;
        //    HeightG = MyGreens.Count;
        //    HeightH = MyHealthyFats.Count;
        //    HeightC = MyCondiments.Count;

        //    UpdatePerRecipeServings();

        //    //~~ To alert lists when their contents change (Xamarin doens't do this, naturally) https://stackoverflow.com/questions/1427471/observablecollection-not-noticing-when-item-in-it-changes-even-with-inotifyprop
        //    MyLeans.CollectionChanged += MyCollectionChanged;
        //    MyGreens.CollectionChanged += MyCollectionChanged;
        //    MyHealthyFats.CollectionChanged += MyCollectionChanged;
        //    MyCondiments.CollectionChanged += MyCollectionChanged;
            
        //    //~~ To Receive Updates
        //    MessagingCenter.Subscribe<GroceriesVM, object>(this, "UpdateRecipeIngredients", OnUpdateRecipeIngredients);   // ...from Lists of Groceries

        //    VerifyClearRecipeCommand = new Command(OnVerifyClearRecipeCommand);
        //    SaveRecipeCommand = new Command(OnSaveRecipeCommand);

        //}







        ///    METHODS    \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


        public void ReportCounts(string region)
        {
            Console.WriteLine($"~~~{ region                                      }");
            Console.WriteLine($"~~~ MyLeans      ...   {   MyLeans.Count()          }");
            Console.WriteLine($"~~~ MyGreens     ...   {   MyGreens.Count()         }");
            Console.WriteLine($"~~~ MyHealthyFats...   {   MyHealthyFats.Count()    }");
            Console.WriteLine($"~~~ MyCondiments ...   {   MyCondiments.Count()     }");

        }
      
        public void OnUpdateRecipeIngredients(object sender, object data) //affectedCatalogNumber)
        {            
            if (!App.isLoading) // && isAbleToSelect)            
            {
                ReportCounts("OnUpdateRecipeIngredients Start");
                Console.WriteLine($"~~~");

                var myData = data as int[];

                var ingredientEffected = new Ingredient(App.Groceries.FirstOrDefault(g => g.CatalogNumber == myData[0] )); //affectedCatalogNumber));
                int countFromGroceries = myData[1];
                int countFromRecipe = 0;

                

                switch (ingredientEffected.Kind)
                {
                    case Kind.Lean:
                        countFromRecipe = this.MyLeans.Count;
                        break;
                    case Kind.Green:
                        countFromRecipe = MyGreens.Count;
                        break;
                    case Kind.HealthyFat:
                        countFromRecipe = MyHealthyFats.Count;
                        break;
                    case Kind.Condiment:
                        countFromRecipe = MyCondiments.Count;
                        break;
                }



                if (countFromRecipe != countFromGroceries)
                {

                    ReportCounts($"countFromRecipe != countFromGroceries ... {countFromRecipe != countFromGroceries}");
                    Console.WriteLine($"~~~");


                    bool included = IngredientScratchList.Any(entry => entry.Key == ingredientEffected.CatalogNumber);

                    if (!included) // Add new ingredient...
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
                        //Console.WriteLine("~~~ Recipe LastSelectionDate " + LastSelectionDate.ToString());
                    }
                    else  // Remove existing ingredient...
                    {
                        //list.Remove(list.FirstOrDefault(ing => ing.CatalogNumber == ingredientEffected.CatalogNumber));
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
                        //Console.WriteLine("~~~ Recipe LastSelectionDate " + LastSelectionDate.ToString());
                    }

                    // Update view's bindings...
                    switch (ingredientEffected.Kind)
                    {
                        case Kind.Lean:
                            foreach (var ing in MyLeans) { TotalLs += ing.QtyPortion; }
                            HeightL = MyLeans.Count;
                            RecommendedHs = SetRecommendedHs();
                            break;
                        case Kind.Green:
                            foreach (var ing in MyGreens) { TotalGs += ing.QtyPortion; }
                            HeightG = MyGreens.Count;
                            break;
                        case Kind.HealthyFat:
                            foreach (var ing in MyHealthyFats) { TotalHs += ing.QtyPortion; }
                            HeightH = MyHealthyFats.Count;
                            break;
                        case Kind.Condiment:
                            foreach (var ing in MyCondiments) { TotalCs += ing.QtyPortion; }
                            HeightC = MyCondiments.Count;
                            break;
                    }

                    ReportCounts("OnUpdateRecipeIngredients End (currentCount != updatedCount)");
                } //  if (currentCount != updatedCount)
                ReportCounts("OnUpdateRecipeIngredients End (!App.isLoading)");
            }

        }





        private async void OnVerifyClearRecipeCommand()
        {
            var answer = await App.Current.MainPage.DisplayAlert("Clear All", "Start a new recipe?", "Yes", "No");
            if (answer == true) // "Yes"
            {
                //Save New Recipe to Db...
                OnClearRecipeCommand();
            }
        }


        private void OnClearRecipeCommand()
        {
            Recipe = new Recipe();
            Recipe.Serves = 1;

            IngredientScratchList.Clear();
            nextIngredientId = 1;

            MyLeans.Clear();
            MyGreens.Clear();
            MyHealthyFats.Clear();
            MyCondiments.Clear();

            TotalLs = 0;
            TotalGs = 0;
            TotalHs = 0;
            TotalCs = 0;
            HeightL = 0;
            HeightG = 0;
            HeightH = 0;
            HeightC = 0;

        }


        //public async void OnSaveRecipeCommand()
        //{
        //    //await Application.Current.MainPage.DisplayAlert("AA", "BB", "Ok"); //https://forums.xamarin.com/discussion/146204/how-to-use-displayalert-from-a-viewmodel-without-additional-frameworks

        //    ObservableCollection<Ingredient>[] myIngs = new ObservableCollection<Ingredient>[] { MyLeans, MyGreens, MyHealthyFats, MyCondiments};
        //    Recipe.Ingredients.Clear();
        //    foreach (var list in myIngs)
        //    {
        //        foreach (var ing in list)
        //        {
        //            Recipe.Ingredients.Add(ing);
        //        }
        //    }
        //    //if (App.Recipe.Ingredients.Count > 0) // There are ingredients, so save action is valid...                
        //    if (Recipe.Ingredients.Count > 0) // There are ingredients, so save action is valid...
        //        {
        //        if (RecipeName != Recipe.defaultName) //This recipe is already saved... update or as save new?
        //        {
        //            var answer = await App.Current.MainPage.DisplayAlert("Save Recipe", "Update or Save as New? ", "Update Recipe", "Save As New");
        //            if (answer == true) // "Update"
        //            {
        //                await App.MySQLite.UpdateRecipeAsync(Recipe);
        //            }
        //            else // "Save As New"
        //            {
        //                Recipe.Id = 0; // Id# 0 means the Db will save as new... it will assign a new number.
        //                string result = await App.Current.MainPage.DisplayPromptAsync(
        //                    "Save Recipe", "Name your new recipe...", accept: "Save", cancel: "Cancel", placeholder: "Name your new recipe...", maxLength: 25, keyboard: Keyboard.Plain, null);

        //                if (string.IsNullOrWhiteSpace(result) || result.Trim() == Recipe.defaultName) // If no name was entered... ask again
        //                {
        //                    await App.Current.MainPage.DisplayPromptAsync(
        //                        "Save Recipe", "Name your new recipe,\n     or update the existing.", accept: "Save", cancel: "Cancel", placeholder: "Recipe name required...", maxLength: 25, keyboard: Keyboard.Plain, null);
        //                }
        //                else // Valid name entered... Save New Recipe to Db...
        //                {
        //                    Recipe.Name = result.Trim();
        //                    Recipe.Serves = RecipeServes;
        //                    await App.MySQLite.SaveRecipeAsync(Recipe); //Add task check?
        //                }
        //            }

        //        }

        //        else // This is a new, fresh recipe to save without a name...
        //        {
        //            string result = await App.Current.MainPage.DisplayPromptAsync(
        //                    "Save Recipe", "Name your new recipe...", accept: "Save", cancel: "Cancel", placeholder: "Name your new recipe...", maxLength: 25, keyboard: Keyboard.Plain, null);

        //            if (string.IsNullOrWhiteSpace(result) || result.Trim() == Recipe.defaultName) // If no name was entered... ask again
        //            {
        //                await App.Current.MainPage.DisplayPromptAsync(
        //                    "Save Recipe", "Name your new recipe,\n     or update the existing.", accept: "Save", cancel: "Cancel", placeholder: "Recipe name required...", maxLength: 25, keyboard: Keyboard.Plain, null);
        //            }
        //            else // Valid name entered... Save New Recipe to Db...
        //            {
        //                Recipe.Name = result.Trim();
        //                Recipe.Serves = RecipeServes;
        //                await App.MySQLite.SaveRecipeAsync(Recipe);
        //                MessagingCenter.Send<RecipeVM>(this, "UpdateSavedRecipesList");
        //            }
        //        }
        //    }
        //    else // (App.Recipe.IngredientList.Count < 0) There are no ingredients to save...
        //    {
        //        var answer = await App.Current.MainPage.DisplayAlert("Your Recipe is Empty", "Add ingriedients to create a new recipe...", "See Tutorial", "Continue");
        //        if (answer == true) // "See Tutorial"
        //        {
        //            await App.Current.MainPage.DisplayAlert("Tutorial", "Coming soon...", "Continue");
        //        }
        //    }
        //}
        
        //public async void SaveRecipe()
        //{
        //    await App.MySQLite.SaveRecipeAsync(Recipe);
        //}




        public void MyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)        
        {
            if (!App.isLoading)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (Ingredient item in e.NewItems)
                    {
                        item.PropertyChanged += IngredientPropertyChanged;
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (Ingredient item in e.OldItems)
                    {
                        item.PropertyChanged -= IngredientPropertyChanged;
                    }
                }
            }
        }   //Updates from Ingredient items on Recipe View


        public void IngredientPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnUpdateAllPortions();
        }   // called when the property of an object inside the collection changes


        public void OnUpdateAllPortions()
        {
            if (MyLeans.Count > 0)
            {
                TotalLs = GetPortionByKind(Kind.Lean);
                RecommendedHs = SetRecommendedHs();
            }
            if (MyGreens.Count > 0)
            {
                TotalGs = GetPortionByKind(Kind.Green);
            }
            if (MyHealthyFats.Count > 0)
            {
                TotalHs = GetPortionByKind(Kind.HealthyFat);
            }
            if (MyCondiments.Count > 0)
            {
                TotalCs = GetPortionByKind(Kind.Condiment);
            }
        } //Updates from Ingredient items on Recipe View


        public void UpdatePortionByKind(Kind kind)
        {
            switch (kind)
            {
                case Kind.Lean:
                    if (MyLeans.Count > 0)
                    {
                        TotalLs = GetPortionByKind(Kind.Lean);
                        RecommendedHs = SetRecommendedHs();
                    }
                    break;
                case Kind.Green:
                    if (MyGreens.Count > 0)
                    {
                        TotalGs = GetPortionByKind(Kind.Green);
                    }
                    break;
                case Kind.HealthyFat:
                    if (MyHealthyFats.Count > 0)
                    {
                        TotalHs = GetPortionByKind(Kind.HealthyFat);
                    }
                    break;
                case Kind.Condiment:
                    if (MyCondiments.Count > 0)
                    {
                        TotalCs = GetPortionByKind(Kind.Condiment);
                    }
                    break;
            }

        } //Updates from Ingredient items on Recipe View


        public float GetPortionByKind(Kind kind) // ... counts bound in header
        {
            var list = GetListOfMyKindToChange(kind);
            float x = 0;
            foreach (var ing in list)
            {
                x += ing.QtyPortion;
            }
            return x;
        }


        public void UpdatePerRecipeServings()
        {
            if (RecipeServes > 0)
            {
                RecommendedLs = 1 * RecipeServes;
                RecommendedGs = 3 * RecipeServes;
                RecommendedHs = SetRecommendedHs();
                RecommendedCs = 3 * RecipeServes;
            }
            else
            {
                RecommendedLs = 1;
                RecommendedGs = 3;
                RecommendedHs = SetRecommendedHs();
                RecommendedCs = 3;
            }
        }


        public float SetRecommendedHs()
        {
            {
                float x = 0;
                if (MyLeans != null)
                {
                    foreach (var ing in MyLeans)
                    {
                        switch (ing.Info1)
                        {
                            case 3://case 3: return "Leanest " = +2 Healthy Fats
                                x += (2 * ing.QtyPortion);
                                break;
                            case 2://case 2: return "Leaner " = +1 Healthy Fats
                                x += (1 * ing.QtyPortion);
                                break;
                            default://case 1: return "Lean " += 0
                                break;
                        }
                    }
                    return x < 99 ? x : 99;
                }
                else
                {
                    return 0; // 0 = there would be no Leans, anyhow.
                };
            }
        }





        public ObservableCollection<Ingredient> GetListOfMyKindToChange(Kind kind)
        {
            return kind switch
            {
                Kind.Lean => MyLeans,
                Kind.Green => MyGreens,
                Kind.HealthyFat => MyHealthyFats,
                Kind.Condiment => MyCondiments,
                _ => throw new NotImplementedException()
            };
        }

    }

}
