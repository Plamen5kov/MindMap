using MindMap.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MindMap.Helpers
{
    public class SQLiteCrud
    {
        private static SQLiteCrud instance;

        private SQLiteCrud()
        {
        }

        public static SQLiteCrud Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SQLiteCrud();
                }
                return instance;
            }
        }


        public async Task<bool> CheckDbAsync(string dbName)
        {
            bool dbExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
            }
            catch (Exception)
            {
                dbExist = false;
            }

            return dbExist;
        }

        public async Task CreateDatabaseAsync(string dbName)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.CreateTableAsync<Node>();
        }

        public async Task Add(string dbName, Node node)
        {
            // Add rows to the Article Table
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.InsertAsync(node);
        }
    }
}
