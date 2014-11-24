using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace MindMap
{
    public class NavigationProperties
    {


        public static String GetNavigateTo(DependencyObject obj)
        {
            return (String)obj.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(DependencyObject obj, String value)
        {
            obj.SetValue(NavigateToProperty, value);
        }

        // Using a DependencyProperty as the backing store for NavigateTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(String), typeof(object), new PropertyMetadata(null, OnNavigateToRropertyValueChanged));

        private static void OnNavigateToRropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ////get Frame

            ////OtherPage
            //var args = e.NewValue;
            //var pageName = args.PageName;
            //var navigationParams = args.Params;

            //frame.NavigateTo(pageName, navigationParams);
            

        }


    }
}
