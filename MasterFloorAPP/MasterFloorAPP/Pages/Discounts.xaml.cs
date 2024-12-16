using System.Data;
using System.Linq;
using System.Windows.Controls;

namespace MasterFloorAPP.Pages
{
    public partial class Discounts : Page
    {
        private readonly Entities db; // Контекст базы данных

        public Discounts()
        {
            InitializeComponent();
            db = new Entities(); // Инициализация контекста базы данных
            LoadDiscounts(); // Загружаем данные о скидках при инициализации страницы
        }

        // Метод для загрузки скидок по партнерам
        private void LoadDiscounts()
        {
            // Группируем данные по партнерам и считаем общее количество товаров по каждому партнеру
            var partnerSales = db.Partner_products
                .GroupBy(p => p.Partner) // Группируем по партнеру
                .Select(g => new
                {
                    Partner = g.Key, // Идентификатор партнера
                    TotalQuantity = g.Sum(p => p.Quantity ?? 0) // Суммируем количество товаров для каждого партнера (с проверкой на null)
                })
                .ToList(); // Преобразуем результат в список

            // Для каждого партнера рассчитываем скидку на основе общего количества товаров
            var partnerDiscounts = partnerSales.Select(ps => new
            {
                Partner = db.Partners.FirstOrDefault(p => p.ID == ps.Partner), // Находим партнера по его ID
                TotalQuantity = ps.TotalQuantity, // Общее количество товаров
                Discount = CalculateDiscount(ps.TotalQuantity) // Рассчитываем скидку на основе количества товаров
            }).ToList(); // Преобразуем результат в список

            // Устанавливаем источник данных для списка, который отображает скидки
            ListUser.ItemsSource = partnerDiscounts;
        }

        // Метод для расчета скидки в зависимости от общего количества товаров
        private decimal CalculateDiscount(int totalQuantity)
        {
            // Логика определения скидки в зависимости от количества
            if (totalQuantity <= 10000)
                return 0; // Если общее количество товаров менее или равно 10000, скидка 0%
            else if (totalQuantity <= 50000)
                return 5; // Если общее количество товаров от 10001 до 50000, скидка 5%
            else if (totalQuantity <= 300000)
                return 10; // Если общее количество товаров от 50001 до 300000, скидка 10%
            else
                return 15; // Если общее количество товаров больше 300000, скидка 15%
        }
    }
}