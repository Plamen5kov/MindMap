using GalaSoft.MvvmLight;
using MindMap.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MindMap.ViewModels
{
    public class NodeDetailsViewModel : ViewModelBase
    {
        private ICommand commandSave;

        public string Title { get; set; }

        public string Content { get; set; }

        public event EventHandler SaveDetailsForNodeEvent;

        //need helper class for database

        public ICommand Save
        {
            get
            {
                if (this.commandSave == null)
                {
                    this.commandSave = new RelayCommand(this.SaveNode);
                }
                return this.commandSave;
            }
        }

        private void SaveNode()
        {
            // TODO: save properties to sqlite

            if (this.SaveDetailsForNodeEvent != null)
            {
                this.SaveDetailsForNodeEvent(this, null);
            }
        }
    }
}
