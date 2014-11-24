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

        public async Task<int> Add(string dbName, Node node)
        {
            // Add rows to the Article Table
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.InsertAsync(node);
            int id = node.Id;

            return id;
        }

        public async Task DeleteAsync(string dbName, int nodeId)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);

            // Retrieve Article
            var node = await conn.Table<Node>()
                .Where(x => x.Id == nodeId)
                .FirstOrDefaultAsync();

            if (node != null)
            {

                // Update record
                await conn.DeleteAsync(node);
            }
        }

        public async Task<ICollection<Node>> FindNodesForParent(string dbName, int parentId)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            var query = conn.Table<Node>().Where(n => n.ParentId == parentId);
            var result = await query.ToListAsync();

            return result;
        }

        public async Task UpdateArticleTitleAsync(string dbName, int nodeId, string newTitle, string newContent)
        {

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);

            // Retrieve Article
            var node = await conn.Table<Node>()
                .Where(x => x.Id == nodeId)
                .FirstOrDefaultAsync();

            if (node != null)
            {
                // modify node
                node.Title = (newTitle == null) ? node.Title : newTitle;
                node.Content = (newContent == null) ? node.Content : newContent;

                // Update record
                await conn.UpdateAsync(node);
            }
        }

        public async Task DropTableAsync(string dbName)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbName);
            await conn.DropTableAsync<Node>();
        }
    }
}
