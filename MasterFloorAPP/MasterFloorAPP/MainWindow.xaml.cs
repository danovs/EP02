using MasterFloorAPP.Pages;
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

namespace MasterFloorAPP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameManager.MainFrame = MainFrame;
            MainFrame.Navigate(new WelcomePage());
        }

        private void BtnPartners_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new Partners());
        }

        private void BtnSales_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new Sales());
        }

        private void BtnDiscounts_Click(object sender, RoutedEventArgs e)
        {
            FrameManager.MainFrame.Navigate(new Discounts());
        }
    }
}
