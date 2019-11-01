namespace Sales.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Interfaces;
    using SQLite;
    using Xamarin.Forms;

    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            _ = this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {
            var databasePath = DependencyService.Get<IPathDatabase>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasePath);
            await connection.CreateTableAsync<Product>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T vObjModel)
        {
            await this.connection.InsertAsync(vObjModel);
        }

        public async Task Insert<T>(List<T> vObjModels)
        {
            await this.connection.InsertAllAsync(vObjModels);
        }

        public async Task Update<T>(T vObjModel)
        {
            await this.connection.UpdateAsync(vObjModel);
        }

        public async Task Update<T>(List<T> vObjModels)
        {
            await this.connection.UpdateAllAsync(vObjModels);
        }

        public async Task Delete<T>(T vObjModel)
        {
            await this.connection.DeleteAsync(vObjModel);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var query = await this.connection.QueryAsync<Product>("select * from Product");
            var array = query.ToArray();
            var vObjList = array.Select(Prod => new Product
            {
                Description = Prod.Description,
                ImagePath = Prod.ImagePath,
                IsAvailable = Prod.IsAvailable,
                Price = Prod.Price,
                ProductID = Prod.ProductID,
                PublishOn = Prod.PublishOn,
                Notes = Prod.Notes,
            }).ToList();
            return vObjList;
        }

        public async Task DeleteAllProducts()
        {
            var vObjQuery = await this.connection.QueryAsync<Product>("delete from Product");
        }
    }
}