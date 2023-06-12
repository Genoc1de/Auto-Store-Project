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
using Auto_Store_Project.DataBase;
using Auto_Store_Project.Scripts;

namespace Auto_Store_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        List<Products> products;
        public ProductPage()
        {
            InitializeComponent();
            ListProducts.ItemsSource = user9Entities.GetContext().Products.ToList();
        }

        private void AddProductInOrderMouse_Click(object sender, RoutedEventArgs e)
        {
            if (products == null)
            {
                products = new List<Products>();
            }
            if ((sender as MenuItem).DataContext is Products product)
            {
                products.Add(product);
            }
            ShowOrder.Visibility = Visibility.Visible;
        }

        private void AddProductInOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (products == null)
            {
                products = new List<Products>();
            }
            var selectedProduct = (Products)ListProducts.SelectedItem;
            products.Add(selectedProduct);
            ShowOrder.Visibility = Visibility.Visible;
        }

        private void ShowOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderView orderView = new Pages.OrderView(products);
            Manager.MainFrame.Navigate(new Pages.OrderView(products));
        }
    }
}
