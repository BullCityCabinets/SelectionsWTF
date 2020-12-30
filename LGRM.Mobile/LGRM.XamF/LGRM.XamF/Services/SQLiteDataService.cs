using System.Collections.Generic;
using LGRM.Data;
using LGRM.Model;
using System.Threading.Tasks;
using SQLite;

namespace LGRM.XamF.Services
{
    public class SQLiteDataService
    {
        private static SQLiteConnector db;
        public static SQLiteConnector Db
        {
            get
            {
                if (db == null)
                {

                    db = new SQLiteConnector();
                }
                return db;
            }
        }

        #region ///    GROCERIES...  \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public List<Grocery> GroceryList;

        public async Task<CreateTablesResult> CreateTableOfGroceriesAsync()
        {
            return await Db.CreateTableOfGroceriesAsync();
        }

        public async Task<int> PopulateTableOfGroceriesAsync(string catalog)
        {
            return await Db.PopulateTableOfGroceriesAsync(catalog);
        }

        public async Task<int> DropTableOfGroceriesAsync()
        {
            return await Db.DropTableOfGroceriesAsync();
        }

        //public async Task<List<Grocery>> GetAllGroceriesAsync() => await Db.GetAllGroceriesAsync();
        public List<Grocery> GetAllGroceries()
        {
            return GroceryList ??= Db.GetAllGroceries();
        }

        #endregion \\\ ...GROCERIES    ////////////////////////////////////////////////////////////////
        // |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        #region    ///    RECIPES...   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public List<Recipe> GetAllRecipeMetas() 
        {
            return Db.GetAllRecipeMetas();
        }



        #endregion  \\\ ...RECIPES    ////////////////////////////////////////////////////////////////

    }
}

