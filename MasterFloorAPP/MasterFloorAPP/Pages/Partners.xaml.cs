using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MasterFloorAPP.Pages
{
    /// <summary>
    /// Логика взаимодействия для Partners.xaml
    /// </summary>
    public partial class Partners : Page
    {
        private readonly Entities db; // Контекст базы данных для работы с таблицей Partners

        public Partners()
        {
            InitializeComponent();
            db = new Entities(); // Инициализация контекста базы данных
            LoadPartners(); // Загрузка списка партнёров в DataGrid
        }

        // Метод для загрузки списка партнёров
        private void LoadPartners()
        {
            try
            {
                // Загружаем список партнёров из базы и привязываем к DataGrid
                DataGridPartners.ItemsSource = db.Partners.ToList();
            }
            catch (Exception ex)
            {
                // Обработка ошибки при загрузке данных
                MessageBox.Show($"Ошибка при загрузке списка партнёров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик события нажатия на кнопку удаления партнёров
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridPartners.SelectedItems.Count > 0)
            {
                // Получаем список ID выбранных партнёров
                var partnerToDelete = DataGridPartners.SelectedItems.Cast<Partner>().Select(x => x.ID).ToList();

                // Подтверждение удаления через окно сообщения
                if (MessageBox.Show(
                    $"Вы действительно хотите удалить {partnerToDelete.Count} партнёра(-ов)? Это действие необратимо.",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Получаем партнёров из базы, которых нужно удалить
                        var partnersToDelete = db.Partners.Where(u => partnerToDelete.Contains(u.ID)).ToList();

                        // Удаляем выбранных партнёров
                        db.Partners.RemoveRange(partnersToDelete);
                        db.SaveChanges(); // Сохраняем изменения в базе

                        // Уведомляем пользователя об успешном удалении
                        MessageBox.Show($"Удалено {partnersToDelete.Count} партнёра(-ов).", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Обновляем список партнёров в DataGrid
                        LoadPartners();
                    }
                    catch (Exception ex)
                    {
                        // В случае ошибки при удалении показываем сообщение об ошибке
                        MessageBox.Show($"Ошибка при удалении партнёров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                // Если не выбран ни один партнёр, показываем сообщение о необходимости выбора
                MessageBox.Show("Пожалуйста, выберите одного или нескольких партнёров для удаления.", "Ошибка выбора", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обработчик события нажатия на кнопку добавления нового партнёра
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Переход на страницу добавления нового партнёра
                NavigationService?.Navigate(new AddPartner(null));
            }
            catch (Exception ex)
            {
                // Обработка ошибки навигации
                MessageBox.Show($"Ошибка при переходе на страницу добавления партнёра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обработчик события нажатия на кнопку редактирования выбранного партнёра
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridPartners.SelectedItems.Count == 1)
            {
                try
                {
                    // Получаем выбранного партнёра и передаём его в страницу редактирования
                    var selectedPartner = DataGridPartners.SelectedItem as Partner;
                    NavigationService?.Navigate(new AddPartner(selectedPartner));
                }
                catch (Exception ex)
                {
                    // Обработка ошибки навигации
                    MessageBox.Show($"Ошибка при переходе на страницу редактирования партнёра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (DataGridPartners.SelectedItems.Count > 1)
            {
                // Если выбрано несколько партнёров, выводим предупреждение
                MessageBox.Show("Пожалуйста, выберите только одного партнёра для редактирования.", "Ошибка выбора", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Если не выбран ни один партнёр, выводим предупреждение
                MessageBox.Show("Пожалуйста, выберите партнёра для редактирования.", "Ошибка выбора", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}