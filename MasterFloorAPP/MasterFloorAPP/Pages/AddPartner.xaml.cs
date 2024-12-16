using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MasterFloorAPP.Pages
{
    public partial class AddPartner : Page
    {
        // Регулярные выражения для проверки форматов
        private static readonly Regex FIOregex = new Regex(@"^[А-ЯЁ][а-яё]+$"); // Для ФИО директора
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"); // Для электронной почты
        private static readonly Regex INNRegex = new Regex(@"^\d{10}$"); // Для ИНН (10 цифр)
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+?[1-9]\d{9,14}$"); // Для номера телефона с международным кодом

        private readonly Entities db; // Контекст базы данных
        private readonly Partner currentPartner; // Текущий партнер, если редактирование

        public AddPartner(Partner selectedPartner = null)
        {
            InitializeComponent();
            db = new Entities(); // Инициализация контекста базы данных
            LoadTypes(); // Загрузка типов партнеров в комбобокс
            currentPartner = selectedPartner; // Если передан партнер, значит редактируем его

            // Если текущий партнер не null, заполняем поля данными партнера
            if (currentPartner != null)
            {
                NameOrg.Text = currentPartner.Name;
                EmailOrg.Text = currentPartner.Email;
                NumberOrg.Text = currentPartner.Number;
                DirectorOrg.Text = currentPartner.Director;
                AddressOrg.Text = currentPartner.Address;
                INNOrg.Text = currentPartner.INN;  // Здесь могут быть проблемы с форматированием ИНН
                RatingOrg.Text = currentPartner.Rating.ToString();

                if (currentPartner.Type != null)
                {
                    Type.SelectedValue = currentPartner.Type; // Выбираем тип партнера
                }
            }
        }

        // Метод для загрузки типов партнеров в комбобокс
        private void LoadTypes()
        {
            var types = db.Partner_type.ToList(); // Получаем все типы из базы
            Type.ItemsSource = types; // Привязываем типы к источнику данных
            Type.DisplayMemberPath = "Type"; // Отображаем название типа
            Type.SelectedValuePath = "ID"; // ID будет значением, которое хранится в базе
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Считываем данные с формы
            string Name = NameOrg.Text;
            string Director = DirectorOrg.Text;
            string Email = EmailOrg.Text;
            string Number = NumberOrg.Text;
            string INN = INNOrg.Text;
            string Rating = RatingOrg.Text;
            string Address = AddressOrg.Text;

            try
            {
                // Проверка корректности данных
                if (!FIOValidation(Director))
                {
                    MessageBox.Show(
                        "ФИО директора введено неверно. Убедитесь, что оно состоит из 2-5 частей (имя, фамилия, и возможно отчество), каждая из которых начинается с заглавной буквы и не превышает 50 символов.",
                        "Ошибка: Некорректное ФИО",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!PhoneNumberRegex.IsMatch(Number))
                {
                    MessageBox.Show(
                        "Номер телефона введён в неверном формате. Убедитесь, что он начинается с международного кода (например, +7) и содержит от 10 до 15 цифр без пробелов и символов.",
                        "Ошибка: Некорректный номер телефона",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!EmailRegex.IsMatch(Email))
                {
                    MessageBox.Show(
                        "Электронная почта введена неверно. Убедитесь, что почта содержит '@' и доменное имя (например, example@domain.com).",
                        "Ошибка: Некорректная электронная почта",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!INNRegex.IsMatch(INN))
                {
                    MessageBox.Show(
                        "ИНН введён неверно. Он должен состоять из ровно 10 цифр. Проверьте и попробуйте снова.",
                        "Ошибка: Некорректный ИНН",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(Rating, out int ratingValue) || ratingValue < 1 || ratingValue > 10)
                {
                    MessageBox.Show(
                        "Рейтинг должен быть числом в диапазоне от 1 до 10. Введите корректное значение.",
                        "Ошибка: Некорректный рейтинг",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Если партнер редактируется (не null)
                if (currentPartner != null)
                {
                    var partnerToUpdate = db.Partners.FirstOrDefault(x => x.ID == currentPartner.ID);
                    var selectedTypeID = (int)Type.SelectedValue; // ID выбранного типа партнера

                    if (partnerToUpdate != null)
                    {
                        // Обновляем данные партнера
                        partnerToUpdate.Type = selectedTypeID;
                        partnerToUpdate.Name = Name;
                        partnerToUpdate.Email = Email;
                        partnerToUpdate.Number = Number;
                        partnerToUpdate.Address = Address;
                        partnerToUpdate.Director = Director;
                        partnerToUpdate.INN = INN;
                        partnerToUpdate.Rating = ratingValue;

                        db.SaveChanges(); // Сохраняем изменения в базе
                        MessageBox.Show("Данные партнера были изменены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields(); // Очищаем форму
                        NavigationService?.Navigate(new Partners()); // Навигация на страницу партнеров
                    }
                    else
                    {
                        MessageBox.Show("Партнер не найден в системе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Если партнер новый, создаем его
                    var selectedTypeID = (int)Type.SelectedValue;
                    var newPartner = new Partner
                    {
                        Type = selectedTypeID,
                        Name = Name,
                        Director = Director,
                        Email = Email,
                        Number = Number,
                        Address = Address,
                        INN = INN,
                        Rating = ratingValue
                    };

                    db.Partners.Add(newPartner); // Добавляем нового партнера в базу
                    db.SaveChanges();
                    MessageBox.Show("Партнер был добавлен в систему");
                    ClearFields(); // Очищаем поля
                    NavigationService?.Navigate(new Partners()); // Перенаправляем на страницу партнеров
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Возникла ошибка при сохранении данных партнёра. Убедитесь, что все поля заполнены корректно.\nОшибка: {ex.Message}",
                    "Критическая ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // Возвращаемся на предыдущую страницу
            ClearFields(); // Очищаем форму
        }

        // Валидация ФИО директора
        private bool FIOValidation(string FIO)
        {
            var splitFIO = FIO.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // Разделяем ФИО по пробелам

            // Проверка, что ФИО состоит из 2-5 частей (имя, фамилия, и возможно отчество)
            if (splitFIO.Length < 2 || splitFIO.Length > 5)
            {
                return false;
            }

            // Проверка каждой части ФИО на соответствие регулярному выражению и длину
            foreach (var part in splitFIO)
            {
                if (string.IsNullOrWhiteSpace(part) || !FIOregex.IsMatch(part) || part.Length > 50)
                {
                    return false;
                }
            }
            return true;
        }

        // Очистка всех полей формы
        private void ClearFields()
        {
            Type.SelectedItem = null;
            NameOrg.Text = string.Empty;
            DirectorOrg.Text = string.Empty;
            AddressOrg.Text = string.Empty;
            NumberOrg.Text = string.Empty;
            INNOrg.Text = string.Empty;
            RatingOrg.Text = string.Empty;
            EmailOrg.Text = string.Empty;
        }
    }
}