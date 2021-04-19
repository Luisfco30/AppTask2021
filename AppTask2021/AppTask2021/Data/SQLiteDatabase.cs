using AppTask2021.Extensions;
using AppTask2021.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTask2021.Data
{
    public class SQLiteDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Connection => LazyInitializer.Value;

        static bool IsInitialized = false;

        async Task InitializeAsync()
        {
            if (!IsInitialized)
            {
                if (!Connection.TableMappings.Any(m => m.MappedType.Name == typeof(TaskModel).Name))
                {
                    await Connection.CreateTablesAsync(CreateFlags.None, typeof(TaskModel)).ConfigureAwait(false);
                    IsInitialized = true;
                }
            }
        }

        public SQLiteDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        public void OnInitializeError()
        {
            throw new NotImplementedException();
        }

        public Task<List<TaskModel>> GetAllTasksAsync()
        {
            return Connection.Table<TaskModel>().ToListAsync();
        }

        public Task<List<TaskModel>> GetNotDoneTasksAsync()
        {
            return Connection.QueryAsync<TaskModel>($"SELECT * FROM [{typeof(TaskModel).Name}] WHERE Done = 0");
        }

        public Task<TaskModel> GetTasksAsync(int id)
        {
            return Connection.Table<TaskModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveTaskAsync(TaskModel item)
        {
            if (item.ID != 0)
            {
                return Connection.UpdateAsync(item);
            }
            else
            {
                return Connection.InsertAsync(item);
            }
        }
        public Task<int> DeleteTaskAsync(TaskModel item)
        {
            return Connection.DeleteAsync(item);
        }

    }
}
