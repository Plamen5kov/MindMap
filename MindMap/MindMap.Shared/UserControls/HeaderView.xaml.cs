using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MindMap.UserControls
{
    public sealed partial class HeaderView : UserControl
    {
        public HeaderView()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public event EventHandler SignOutEvent;
        public event EventHandler GoBackEvent;


        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        public static readonly DependencyProperty TitleTextProperty =
            DependencyProperty.Register("TitleText", typeof(string), typeof(HeaderView), new PropertyMetadata(null));

        private void OnGoBackClicked(object sender, RoutedEventArgs e)
        {
            if (this.GoBackEvent != null)
            {
                this.GoBackEvent(this, null);
            }
        }

        private void OnSignOutClicked(object sender, RoutedEventArgs e)
        {
            ParseUser.LogOut();
            if (this.SignOutEvent != null)
            {
                this.SignOutEvent(this, null);
            }
        }
    }
}
