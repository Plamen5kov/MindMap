using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MindMap.ViewModels
{
    public class MindMapViewModel : ViewModelBase
    {
        private ObservableCollection<NodeViewModel> nodeViewModels;

        public ICollection<NodeViewModel> NodesList
        {
            get
            {
                if (this.nodeViewModels == null)
                {
                    this.nodeViewModels = new ObservableCollection<NodeViewModel>();
                }
                return this.nodeViewModels;
            }
            set
            {
                if (this.nodeViewModels == null)
                {
                    this.nodeViewModels = new ObservableCollection<NodeViewModel>();
                }
                this.nodeViewModels.Clear();
                foreach (var item in value)
                {
                    this.nodeViewModels.Add(item);
                }
            }
        }
    }
}
