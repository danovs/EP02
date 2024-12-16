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

namespace MasterFloorAPP.Pages
{
    /// <summary>
    /// Логика взаимодействия для Partners.xaml
    /// </summary>
    public partial class Partners : Page
    {
        private readonly Entities db;
        public Partners()
        {
            InitializeComponent();
            db = new Entities();
            DataGridPartners.ItemsSource = db.Partners.ToList();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridPartners.SelectedItem != null)
            {
                var partnerToDelete = DataGridPartners.SelectedItems.Cast<Partner>().Select(x => x.ID).ToList();
                if (MessageBox.Show($"Вы действительно хотите удалить {partnerToDelete.Count()} партнеров?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var partnersToDelete = db.Partners.Where(u => partnerToDelete.Contains(u.ID)).ToList();
                        db.Partners.RemoveRange(partnersToDelete);
                        db.SaveChanges();
                        DataGridPartners.ItemsSource = db.Partners.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Партнеры не были удалены.", ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите партнеров для удаления.");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddPartner());
        }
    }
}
