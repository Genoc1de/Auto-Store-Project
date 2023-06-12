using Auto_Store_Project.DataBase;
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

namespace Auto_Store_Project.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderView.xaml
    /// </summary>
    public partial class OrderView : Page
    {
        Orders orders;
        List<ProductsInOrder> productsInOrders;

        decimal AllProductsPrice;
        decimal AllProductsDiscount;

        public OrderView(List<Products> products)
        {
            InitializeComponent();

            orders = new Orders();
            productsInOrders = new List<ProductsInOrder>();
            orders.ID = user9Entities.GetContext().Orders.Max(o => o.ID) + 1;
            PickUpPointsCB.ItemsSource = user9Entities.GetContext().PickUpPoints.ToList();

            foreach (var product in products)
            {
                var existingProductInOrder = user9Entities.GetContext().ProductsInOrder.FirstOrDefault(p => p.ProductID == p.Products.ID);
                if (existingProductInOrder != null)
                {
                    existingProductInOrder.QuantityProductsInOrder++;
                }
                var productInOrder = new ProductsInOrder()
                {
                    OrderID = orders.ID,
                    Products = product,
                    ProductID = product.ID,
                    QuantityProductsInOrder = 1
                };
                productsInOrders.Add(productInOrder); 
            }
            ListProductsInOrder.ItemsSource = productsInOrders;
            Calcul(productsInOrders);
        }

        private void PLusQuantity_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is ProductsInOrder orderProduct)
            {
                var productInOrder = productsInOrders.Find(p => p.ProductID == orderProduct.ProductID);
                if (productInOrder != null)
                {
                    productInOrder.QuantityProductsInOrder++;
                }
            }
            ListProductsInOrder.ItemsSource = productsInOrders;
            Calcul(productsInOrders);
            ListProductsInOrder.Items.Refresh();
        }

        private void MinusQuantity_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is ProductsInOrder orderProduct)
            {
                productsInOrders.Where(p => p.ProductID == orderProduct.ProductID).FirstOrDefault().QuantityProductsInOrder--;
                if (productsInOrders.Where(p => p.ProductID == orderProduct.ProductID).FirstOrDefault().QuantityProductsInOrder == 0)
                {
                    var result = MessageBox.Show("Вы действительно хотите удалить продукт?", "Удаление продукта из заказа", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        productsInOrders.Remove(productsInOrders.Where(p => p.ProductID == orderProduct.ProductID).FirstOrDefault());

                    }
                    else
                    {
                        productsInOrders.Where(p => p.ProductID == orderProduct.ProductID).FirstOrDefault().QuantityProductsInOrder = 1;
                    }

                }
            }
            ListProductsInOrder.ItemsSource = productsInOrders;
            Calcul(productsInOrders);
            ListProductsInOrder.Items.Refresh();
        }


        private void FormOrder_Click(object sender, RoutedEventArgs e)
        {
            if (PickUpPointsCB.SelectedItem != null)
            {
                var MaxGetCode = user9Entities.GetContext().Orders.Max(p => p.PickUpCode);
                var newOrder = new Orders()
                {
                    DateDelivery = DateDelivery(productsInOrders),
                    StatusOrderID = 0,
                    PickUpPointID = ((PickUpPoints)PickUpPointsCB.SelectedItem).ID,
                    PickUpCode = MaxGetCode + 1,
                };
                user9Entities.GetContext().Orders.Add(newOrder);

                foreach (var item in productsInOrders)
                {
                    var productInOrder = new ProductsInOrder()
                    {
                        OrderID = newOrder.ID,
                        ProductID = item.ProductID,
                        QuantityProductsInOrder = item.QuantityProductsInOrder
                    };
                    user9Entities.GetContext().ProductsInOrder.Add(productInOrder);
                }

                try
                {
                    user9Entities.GetContext().SaveChanges();

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append($"Заказ оформлен!\n");
                    stringBuilder.Append($"Номер заказа: {newOrder.ID}\n");
                    stringBuilder.Append($"Дата доставки: {newOrder.DateDelivery}\n");
                    stringBuilder.Append($"Пункт Выдачи: {((PickUpPoints)PickUpPointsCB.SelectedItem).Name}\n");
                    stringBuilder.Append($"Код Получения: {newOrder.PickUpCode}\n");
                    stringBuilder.Append($"Сумма заказа: {TotalPrice.Text.ToString()}\n");
                    stringBuilder.Append($"Сумма заказа: {TotalDiscount.Text.ToString()}\n");
                    stringBuilder.Append($"\nСписок продуктов:\n");

                    foreach (var item in productsInOrders)
                    {
                        var product = user9Entities.GetContext().Products.FirstOrDefault(p => p.ID == item.ProductID);
                        stringBuilder.Append($"{item.Products.Name} - {item.QuantityProductsInOrder} шт\n");
                    }

                    string message = stringBuilder.ToString();
                    MessageBox.Show(message, "Информация о заказе", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Укажите Пункт Выдачи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        public static DateTime DateDelivery(List<ProductsInOrder> qProductInOrder)
        {
            if (qProductInOrder.Count <= 3)
            {
                return DateTime.Now.AddDays(6);
            }
            else
            {
                foreach (var item in qProductInOrder)
                {
                    if (item.Products.Quantity < item.QuantityProductsInOrder)
                    {
                        return DateTime.Now.AddDays(6);
                    }
                }
            }
            return DateTime.Now.AddDays(3);
        }

        private void Calcul(List<ProductsInOrder> productinOrder)
        {
            AllProductsPrice = 0;
            AllProductsDiscount = 0;
            foreach (var item in productinOrder) 
            {
                decimal PriceProducts = item.Products.Price * item.QuantityProductsInOrder;
                AllProductsPrice += PriceProducts;

                if (item.Products.Discount != null)
                {
                    decimal DiscountProducts = (decimal)((item.Products.Price * item.Products.Discount * item.QuantityProductsInOrder) / 100);
                    AllProductsDiscount += DiscountProducts;
                    AllProductsPrice -= AllProductsDiscount;
                }
            }
            TotalPrice.Text = $"{AllProductsPrice.ToString()}";
            TotalDiscount.Text = $"{AllProductsDiscount.ToString()}";

        }
    }
}
