using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace MindMap.Helpers
{
    public static class NodeManager
    {
        public static UIElement CreateNode()
        {

            var grid_container = CreateGrid();
            var cnvs_container = CreateCanvas();
            var stackPanel_innerContainer = new StackPanel();
            stackPanel_innerContainer.Margin = new Thickness(20, 30, 0, 0);
            var tb_title = CreateTextBlock("node title", 30, 25, Windows.UI.Text.FontWeights.ExtraBold);
            var tb_text = CreateTextBlock("content of the root node that's going to show on the screen", 120, 20, Windows.UI.Text.FontWeights.Light);
            var rect_node = CreateEllipse();

            stackPanel_innerContainer.Children.Add(tb_title);
            stackPanel_innerContainer.Children.Add(tb_text);
            cnvs_container.Children.Add(stackPanel_innerContainer);
            cnvs_container.Children.Add(rect_node);
            grid_container.Children.Add(cnvs_container);

            return grid_container;
        }

        private static Canvas CreateCanvas()
        {
            var canvas = new Canvas();
            Grid.SetColumn(canvas, 1);

            return canvas;
        }
        private static Grid CreateGrid()
        {
            var grid_container = new Grid();
            grid_container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid_container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid_container.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            return grid_container;
        }
        private static Ellipse CreateEllipse()
        {
            // Create a Rectangle
            var rect = new Ellipse();
            rect.Width = 230;
            rect.Height = 200;
            rect.Stretch = Stretch.Fill;
            rect.Opacity = 0.5;
            rect.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            rect.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;

            // Create a blue and a black Brush
            SolidColorBrush blueBrush = new SolidColorBrush();
            blueBrush.Color = Colors.Aquamarine;
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.White;

            // Set Rectangle's width and color
            rect.StrokeThickness = 2;
            rect.Stroke = blackBrush;

            // Fill rectangle with blue color
            rect.Fill = blueBrush;

            return rect;
        }
        private static TextBlock CreateTextBlock(string text, double height, double fontSize, Windows.UI.Text.FontWeight fontWeight)
        {
            TextBlock tb = new TextBlock();
            tb.TextAlignment = TextAlignment.Center;
            tb.Text = text;
            tb.Width = 200;
            tb.Height = height;
            tb.FontSize = fontSize;
            tb.FontWeight = fontWeight;
            tb.TextWrapping = TextWrapping.Wrap;

            return tb;
        }
    }
}
