using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Product_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            HomeStartup();
        }

        /// <summary>
        /// HomeStartup Function
        /// --------------------
        /// 1. Sets button background white for selected.
        /// 2. Sets source of main frame to home page.
        /// </summary>
        private void HomeStartup()
        {
            HomeButton.Background = Brushes.White;
            MainWindowFrame.Source = new Uri("Pages/Home.xaml", UriKind.Relative);
        }

        /// <summary>
        /// Button_Click Event
        /// ------------------
        /// 1. ClearButtonSelection() Function called to clear all selection in navigation buttons.
        /// 2. Sets button backgrounds and main frame sources according to the selected button content using switch case.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var buttonContent = button.Content;

            ClearButtonSelection();

            switch (buttonContent)
            {
                case "Home":
                    HomeButton.Background = Brushes.White;
                    MainWindowFrame.Source = new Uri("Pages/Home.xaml",UriKind.Relative);
                    break;
                case "Products Management":
                    ProductsButton.Background = Brushes.White;
                    MainWindowFrame.Source = new Uri("Pages/ProductsManagement.xaml",UriKind.Relative);
                    break;
                case "Categories Management":
                    CategoriesButton.Background = Brushes.White;
                    MainWindowFrame.Source = new Uri("Pages/CategoriesManagement.xaml", UriKind.Relative);
                    break;
                case "Tags Management":
                    TagButton.Background = Brushes.White;
                    MainWindowFrame.Source = new Uri("Pages/TagsManagement.xaml", UriKind.Relative);
                    break;
            }
        }

        /// <summary>
        /// ClearButtonSelection Function
        /// -----------------------------
        /// 1. Sets all buttons background to transparent to avoid selection.
        /// </summary>
        private void ClearButtonSelection()
        {
            HomeButton.Background = Brushes.Transparent;
            ProductsButton.Background = Brushes.Transparent;
            CategoriesButton.Background = Brushes.Transparent;
            TagButton.Background = Brushes.Transparent;
        }
    }
}
