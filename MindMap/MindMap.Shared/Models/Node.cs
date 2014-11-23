using MindMap.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace MindMap.Models
{
    [Table("Nodes")]
    public class Node
    {
        [PrimaryKey, AutoIncrement]
        public int ParentId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        //public BitmapImage Image { get; set; }
    }
}
