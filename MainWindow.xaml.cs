using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Product_Manager.Pages;
using Product_Manager.ViewModels;

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
        /// 2. 
        /// </summary>
        private void HomeStartup()
        {
            HomeButton.Background = Brushes.White;

            ProductViewModel productViewModel = new ProductViewModel();

            Home home = new Home();
            home.DataContext = productViewModel;
            MainWindowFrame.Content = home;

            Pages.Home_Pages.Products products = new Pages.Home_Pages.Products();
            products.DataContext = productViewModel;
            home.ProductsFrame.Content = products;
        }

        /// <summary>
        /// Button_Click Event
        /// ------------------
        /// 1. ClearButtonSelection() Function called to clear all selection in navigation buttons.
        /// 2. 
        /// 3. Sets button backgrounds and main frame sources according to the selected button content using switch case.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            object buttonContent = button.Content;

            ClearButtonSelection();

            switch (buttonContent)
            {
                case "Home":
                    HomeButton.Background = Brushes.White;

                    ProductViewModel productViewModel = new ProductViewModel();

                    Home home = new Home();
                    home.DataContext = productViewModel;
                    MainWindowFrame.Content = home;

                    Pages.Home_Pages.Products products = new Pages.Home_Pages.Products();
                    products.DataContext = productViewModel;
                    home.ProductsFrame.Content = products;

                    break;
                case "Products Management":
                    ProductsButton.Background = Brushes.White;
                    MainWindowFrame.Source = new Uri("Pages/ProductsManagement.xaml", UriKind.Relative);
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
