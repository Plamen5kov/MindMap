using MindMap.Common;
using MindMap.Helpers;
using MindMap.Models;
using MindMap.ViewModels;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MindMap.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MindMapPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private const string DbName = "Nodes.db";
        private bool isParent = false;

        public MindMapPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.ViewModel = new MindMapViewModel();
            //this.AttachGestures();
        }

        //private void AttachGestures()
        //{
        //    this.LayoutRoot.
        //}

        public MindMapViewModel ViewModel
        {
            get
            {
                return this.DataContext as MindMapViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        public void OnSignOutCompleated(object sender, EventArgs args)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            LoadCurrentPageNodes(e);
        }

        private async Task LoadCurrentPageNodes(NavigationEventArgs e)
        {
            var passedParameter = e.Parameter;
            var count = this.ViewModel.NodesList.Count;
            if ((int)passedParameter == 0)
            {
                await CreateTableIfNeeded();
            }

            if ((int)passedParameter == 0)
            {
                isParent = true;
            }
            else
            {
                isParent = false;
            }
            this.ViewModel.ParentId = (int)passedParameter;
            this.ViewModel.FindNodesWithCurrentParent();
        }

        private async Task CreateTableIfNeeded()
        {
            if (! await SQLiteCrud.Instance.CheckDbAsync(DbName))
            {
                this.ViewModel.CreateDatabase(DbName);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void OnGridTap(object sender, TappedRoutedEventArgs e)
        {
            var currentSelectedObject = e.OriginalSource;
            if ((currentSelectedObject as Grid) != null && !isParent)
            {
                this.ViewModel.AddNode();
            }
            if ((currentSelectedObject as Rectangle) != null)
            {
                int selectedNodeId = this.ViewModel.SelectedNode.Id;
                this.Frame.Navigate(typeof(MindMapPage), selectedNodeId);
            }
        }

        private void Rectangle_Holding(object sender, HoldingRoutedEventArgs e)
        {
            var selectedNodeId = ((e.OriginalSource as Rectangle).DataContext as Node).Id;

            this.sb_grid.Begin();
            this.Frame.Navigate(typeof(NodeDetailsPage), selectedNodeId);
        }
    }
}