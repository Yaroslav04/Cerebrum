
using Cerebrum.Core.Model;
using SQLite;

namespace Cerebrum.Core.Servises
{
    public class DataBase
    {
        readonly SQLiteAsyncConnection objectDataBase;
        readonly SQLiteAsyncConnection tegDataBase;

        public DataBase(string _connectionString, List<string> _dataBaseName)
        {

            objectDataBase = new SQLiteAsyncConnection(Path.Combine(_connectionString, _dataBaseName[0]));
            objectDataBase.CreateTableAsync<ObjectClass>().Wait();

            tegDataBase = new SQLiteAsyncConnection(Path.Combine(_connectionString, _dataBaseName[1]));
            tegDataBase.CreateTableAsync<TegClass>().Wait();

        }

        #region Object

        public Task<int> SaveObjectAsync(ObjectClass _object)
        {
            try
            {
                return objectDataBase.InsertAsync(_object);
            }
            catch
            {
                return null;
            }
        }
        public Task<int> DeleteObjectAsync(ObjectClass _object)
        {
            try
            {
                return objectDataBase.DeleteAsync(_object);
            }
            catch
            {
                return null;
            }

        }
        public Task<int> UpdateObjectAsync(ObjectClass _object)
        {
            try
            {
                return objectDataBase.UpdateAsync(_object);
            }
            catch
            {
                return null;
            }

        }
        public Task<List<ObjectClass>> GetObjectsAsync()
        {
            return objectDataBase.Table<ObjectClass>().ToListAsync();
        }

        #endregion

        #region Teg

        public Task<int> SaveTegAsync(TegClass _object)
        {
            try
            {
                return tegDataBase.InsertAsync(_object);
            }
            catch
            {
                return null;
            }
        }
        public Task<int> DeleteTegAsync(TegClass _object)
        {
            try
            {
                return tegDataBase.DeleteAsync(_object);
            }
            catch
            {
                return null;
            }

        }
        public Task<int> UpdateTegAsync(TegClass _object)
        {
            try
            {
                return tegDataBase.UpdateAsync(_object);
            }
            catch
            {
                return null;
            }

        }
        public Task<List<TegClass>> GetTegsAsync()
        {
            return tegDataBase.Table<TegClass>().ToListAsync();
        }


        #endregion

    }
}
