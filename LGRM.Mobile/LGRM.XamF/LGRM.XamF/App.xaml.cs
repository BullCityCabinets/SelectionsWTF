using LGRM.Model;
using LGRM.XamF.Services;
using LGRM.XamF.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LGRM.XamF
{
    public partial class App : Application
    {
        public static SQLiteDataService MySQLite { get; } = new SQLiteDataService();
        public static VersionService V { get; } = new VersionService(new MockRemoteDataService());

        public static ObservableCollection<Grocery> Groceries { get; set; }

        public static bool isLoading = false;

        public App()
        {
            InitializeComponent();
            //CompareVersion();

            var createTable = Task.Run(() => MySQLite.CreateTableOfGroceriesAsync());
            createTable.Wait();

            var populateTableFromJson = Task.Run(() => MySQLite.PopulateTableOfGroceriesAsync(V.ShippedCatalog));
            populateTableFromJson.Wait();

            MainPage = new NavigationPage(new LogInPage());
            
        }

        //public void CompareVersion()
        //{
        //    if (!V.DbIsUpdated) // Install SQLites Groceries catalog
        //    {
        //        var createTable = Task.Run(() => MySQLite.CreateTableOfGroceriesAsync());
        //        createTable.Wait();

        //        var populateTableFromJson = Task.Run(() => MySQLite.PopulateTableOfGroceriesAsync(V.ShippedCatalog));
        //        populateTableFromJson.Wait();

        //        V.UpdateVersion();

        //    }

        //}
    }
}
