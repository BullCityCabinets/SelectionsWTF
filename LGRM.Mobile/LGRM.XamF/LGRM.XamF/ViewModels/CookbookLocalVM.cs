using LGRM.Model;
using LGRM.XamF.ViewModels.Framework;
using LGRM.XamF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LGRM.XamF.ViewModels
{
    public class CookbookLocalVM : BaseVM
    {
        ///    MEMBERS      \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\       

        //~~ Titles & Styles...
        public string TitleText { get; set; }
        public string FooterText { get; set; } = App.V.FooterText;

        public int IngredientLabelHeight = 100; //Standard height
        public int EmptyHeight = 60;
        private int _heightOfCollectionView { get; set; }
        public int HeightOfCollectionView
        {
            get
            {
                if (RecipesDisplayed.Count > 0)
                {
                    return (_heightOfCollectionView * IngredientLabelHeight);
                }
                else
                {
                    return EmptyHeight;
                }
            }
            set
            {
                _heightOfCollectionView = value;
                OnPropertyChanged("HeightOfCollectionView");
            }
        }

        //~~ Selecting Items...
        private object _mySelectedItem; //To capture View's binding input
        public object MySelectedItem
        {
            get
            {
                return _mySelectedItem;
            }
            set
            {
                if (_mySelectedItem != value)
                {
                    _mySelectedItem = value;
                }
            }
        }
        
        public ICommand CreateNewRecipeCommand { get; set; }
        public ICommand LoadSelectedRecipeCommand { get; set; }
        public ICommand DeleteSelectedRecipeCommand { get; set; }

        //~~ Data Source...
        public Kind MyKind;
        private List<Recipe> _recipesDisplayed;
        public List<Recipe> RecipesDisplayed
        {
            get => _recipesDisplayed;
            set
            {
                _recipesDisplayed = value;
                OnPropertyChanged("RecipesDisplayed");
            }
        }

        public int RecipeCount { get; set; }



        ///    CTOR         \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\       
        public CookbookLocalVM()
        {
            RecipesDisplayed = new List<Recipe>();
            var recipeMetas = App.MySQLite.GetAllRecipeMetas();
            foreach (var item in recipeMetas)
            {
                RecipesDisplayed.Add(item);
            }

            RecipeCount = RecipesDisplayed.Count;
            MessagingCenter.Subscribe<RecipeVM>(this, "NewRecipeSaved", OnNewRecipeSavedCommand);
            MessagingCenter.Subscribe<RecipeVM>(this, "UpdateSavedRecipesList", OnNewRecipeSavedCommand);   // ...from Lists of Groceries
            
            //DeleteSelectedRecipeCommand = new Command(OnDeleteSelectedRecipeCommand);

        }




        ///    METHODS       \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\       

        private void OnNewRecipeSavedCommand(RecipeVM obj) 
        {
            RecipesDisplayed = App.MySQLite.GetAllRecipeMetas(); 
        }

        private Recipe recipeToLoad { get; set; }
        public Recipe RecipeToLoad
        {
            get => recipeToLoad;
            set
            {
                recipeToLoad = value;
                OnPropertyChanged("RecipeToLoad");
            }
        }

    

        






    }



}
