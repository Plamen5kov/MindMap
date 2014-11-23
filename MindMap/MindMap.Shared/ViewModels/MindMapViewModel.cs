using GalaSoft.MvvmLight;
using MindMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MindMap.Helpers.Extensions;
using MindMap.Helpers;

namespace MindMap.ViewModels
{
    public class MindMapViewModel : ViewModelBase
    {
        private const string DbName = "Nodes.db";
        private ObservableCollection<Node> nodeModels;
        private SQLiteCrud db;

        public MindMapViewModel()
        {
            this.db = SQLiteCrud.Instance;
        }

        public int ParentId { get; set; }

        public ICollection<Node> NodesList
        {
            get
            {
                if (this.nodeModels == null)
                {
                    this.nodeModels = new ObservableCollection<Node>();
                }
                return this.nodeModels;
            }
            set
            {
                if (this.nodeModels == null)
                {
                    this.nodeModels = new ObservableCollection<Node>();
                }
                this.nodeModels.Clear();
                this.nodeModels.AddRange(value);
            }
        }

        public Node SelectedNode { get; set; }

        public async void AddNode()
        {
            var nodeToAdd = new Node() { Title = "random title", Content = "random content", ParentId = this.ParentId };
            var parId = this.ParentId;
            await SQLiteCrud.Instance.Add(DbName, nodeToAdd);
            this.NodesList.Add(nodeToAdd);
        }

        public async void CreateDatabase(string DbName)
        {
            // Create Db if not exist
            bool dbExists = await this.db.CheckDbAsync(DbName);
            if (!dbExists)
            {
                await this.db.CreateDatabaseAsync(DbName);
                //create root
                var root = new Node() { Title = "root", ParentId = 0 };
                await db.Add(DbName, root);
            }

            // get root
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<Node>();
            var rootInList = await query.ToListAsync();

            // to display in view
            this.NodesList = rootInList;
        }

    }
}
