using System.Linq;
using System.Windows.Controls;

namespace MasterFloorAPP.Pages
{
    public partial class Sales : Page
    {
        private readonly Entities db;
        public Sales()
        {
            InitializeComponent();
            db = new Entities();
            DataGridSales.ItemsSource = db.Partner_products.ToList();
        }
    }
}
