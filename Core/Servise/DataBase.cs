
using Cerebrum.Core.Model;
using SQLite;
using System.Diagnostics;
using System.Linq;

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

        public async Task<int> GetLastOblectIndex()
        {
            try
            {
                var list = await GetObjectsAsync();
                return list.Last().N;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<List<ObjectClass>> GetObjectsAsync()
        {
            return await objectDataBase.Table<ObjectClass>().ToListAsync();
        }

        public async Task<ObjectClass> GetObjectAsync(int _id)
        {
            return await objectDataBase.Table<ObjectClass>().Where(x => x.N == _id).FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetTypes()
        {
            List<string> result = new List<string>();
            try
            {
                foreach (var item in await GetObjectsAsync())
                {
                    if (!String.IsNullOrWhiteSpace(item.Type))
                    {
                        result.Add(item.Type);
                    }
                   
                }  
            }
            catch
            {
                return null;
            }

            if (result.Count > 0)
            {
                result = result.Distinct().ToList();
                result = result.OrderBy(x => x).ToList();
            }
            
            return result;
        }

        public async Task<List<string>> GetAuthorities()
        {
            List<string> result = new List<string>();
            try
            {
                foreach (var item in await GetObjectsAsync())
                {
                    if (!String.IsNullOrWhiteSpace(item.Authority))
                    {
                        result.Add(item.Authority);
                    }
                }
            }
            catch
            {
                return null;
            }

            if (result.Count > 0)
            {
                result = result.Distinct().ToList();
                result = result.OrderBy(x => x).ToList();
            }

            return result;
        }

        public async Task<bool> IsCaseNotExist(string _case)
        {
            var result = await objectDataBase.Table<ObjectClass>().Where(x => x.Identification == _case).ToListAsync();
            if (result.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> IsObjectExistByIdentification(string _auth, string _ident)
        {
            var result = await objectDataBase.Table<ObjectClass>().Where(x => x.Authority == _auth & x.Identification == _ident).ToListAsync();
            if (result.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
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

        public Task<List<TegClass>> GetTegsByIdAsync(int _id)
        {
            return tegDataBase.Table<TegClass>().Where(x => x.Id == _id).ToListAsync();
        }


        #endregion

    }
}
