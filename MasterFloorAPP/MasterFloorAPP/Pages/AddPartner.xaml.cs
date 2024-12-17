using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MasterFloorAPP.Pages
{
    public partial class AddPartner : Page
    {
        // Регулярные выражения для проверки форматов
        private static readonly Regex FIOregex = new Regex(@"^[А-ЯЁ][а-яё]+$"); // Для ФИО директора
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"); // Для электронной почты
        private static readonly Regex INNRegex = new Regex(@"^\d{10}$"); // Для ИНН (10 цифр)
        private static readonly Regex QuantityRegex = new Regex(@"^\d+$");
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+?[1-9]\d{9,14}$"); // Для номера телефона с международным кодом

        private readonly Entities db; // Контекст базы данных
        private readonly Partner currentPartner; // Текущий партнер, если редактирование

        public AddPartner(Partner selectedPartner = null)
        {
            InitializeComponent();
            db = new Entities(); // Инициализация контекста базы данных
            LoadTypes(); // Загрузка типов партнеров в комбобокс
            LoadProducts();
            LoadProductTypes();
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

                var partnerProduct = db.Partner_products
                               .FirstOrDefault(pp => pp.Partner == currentPartner.ID);

                if (partnerProduct != null)
                {
                    // Выбираем соответствующий товар и тип товара, если такой есть
                    TovariOrg.SelectedValue = partnerProduct.Product;
                    QuantityOrg.Text = partnerProduct.Quantity.ToString();

                    // Найдем тип товара и установим его в комбобокс
                    var product = db.Products.FirstOrDefault(p => p.ID == partnerProduct.Product);
                    if (product != null)
                    {
                        var productType = db.Product_type.FirstOrDefault(pt => pt.ID == product.Type);
                        if (productType != null)
                        {
                            TovariTypes.SelectedValue = productType.ID; // Устанавливаем тип товара
                        }
                    }
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

        private void LoadProducts()
        {
            var products = db.Products.ToList();
            TovariOrg.ItemsSource = products;
            TovariOrg.DisplayMemberPath = "Name";
            TovariOrg.SelectedValuePath = "ID";
        }

        private void LoadProductTypes()
        {
            var productTypes = db.Product_type.ToList();
            TovariTypes.ItemsSource = productTypes;
            TovariTypes.DisplayMemberPath = "Type";
            TovariTypes.SelectedValuePath= "ID";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Организация
            string NameOrganization = NameOrg.Text;
            string DirectorOrganization = DirectorOrg.Text;
            string EmailOrganization = EmailOrg.Text;
            string NumberOrganization = NumberOrg.Text;
            string INNOrganization = INNOrg.Text;
            string RatingOrganization = RatingOrg.Text;
            string AddressOrganization = AddressOrg.Text;

            int? selectedType = (int?)Type.SelectedValue;

            // Товар
            string Quantity = QuantityOrg.Text;
            int? selectedProductTypeID = (int?)TovariTypes.SelectedValue;
            int? selectedProductID = (int?)TovariOrg.SelectedValue;

            try
            {
                // Валидация данных
                if (!FIOValidation(DirectorOrganization))
                {
                    MessageBox.Show("ФИО директора введено неверно.", "Ошибка: Некорректное ФИО", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!PhoneNumberRegex.IsMatch(NumberOrganization))
                {
                    MessageBox.Show("Номер телефона введён в неверном формате.", "Ошибка: Некорректный номер телефона", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!EmailRegex.IsMatch(EmailOrganization))
                {
                    MessageBox.Show("Электронная почта введена неверно.", "Ошибка: Некорректная электронная почта", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!INNRegex.IsMatch(INNOrganization))
                {
                    MessageBox.Show("ИНН введён неверно.", "Ошибка: Некорректный ИНН", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(RatingOrganization, out int ratingValue) || ratingValue < 1 || ratingValue > 10)
                {
                    MessageBox.Show("Рейтинг должен быть числом от 1 до 10.", "Ошибка: Некорректный рейтинг", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!QuantityRegex.IsMatch(Quantity))
                {
                    MessageBox.Show("Количество должно быть числом.", "Ошибка: Некорректное количество", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (currentPartner != null)  // Если редактируем существующего партнера
                {
                    // Обновление данных партнера
                    currentPartner.Name = NameOrganization;
                    currentPartner.Director = DirectorOrganization;
                    currentPartner.Email = EmailOrganization;
                    currentPartner.Number = NumberOrganization;
                    currentPartner.INN = INNOrganization;
                    currentPartner.Address = AddressOrganization;
                    currentPartner.Rating = ratingValue;
                    currentPartner.Type = (int)selectedType;

                    db.SaveChanges();  // Сохраняем изменения в базе данных

                    int partnerId = currentPartner.ID;

                    // Обновляем или добавляем запись о товаре партнера
                    var partnerProduct = db.Partner_products
                                           .FirstOrDefault(pp => pp.Partner == partnerId);

                    if (partnerProduct != null)
                    {
                        partnerProduct.Product = selectedProductID ?? partnerProduct.Product;
                        partnerProduct.Quantity = int.TryParse(Quantity, out int quantityValue) ? quantityValue : partnerProduct.Quantity;
                    }
                    else if (selectedProductID.HasValue && int.TryParse(Quantity, out int quantityValue))
                    {
                        partnerProduct = new Partner_products
                        {
                            Partner = partnerId,
                            Product = selectedProductID.Value,
                            Quantity = quantityValue,
                            SaleDATE = DateTime.Now
                        };

                        db.Partner_products.Add(partnerProduct);
                    }

                    db.SaveChanges();  // Сохраняем изменения в базе данных

                    MessageBox.Show("Данные партнера обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else  // Если создаем нового партнера
                {
                    var newPartner = new Partner
                    {
                        Type = (int)selectedType,
                        Name = NameOrganization,
                        Director = DirectorOrganization,
                        Email = EmailOrganization,
                        Number = NumberOrganization,
                        INN = INNOrganization,
                        Address = AddressOrganization,
                        Rating = ratingValue
                    };

                    db.Partners.Add(newPartner);
                    db.SaveChanges();  // Сохраняем нового партнера в базе данных

                    int partnerId = newPartner.ID;

                    if (int.TryParse(Quantity, out int quantityValue) && selectedProductID.HasValue)
                    {
                        var partnerProduct = new Partner_products
                        {
                            Partner = partnerId,
                            Product = selectedProductID.Value,
                            Quantity = quantityValue,
                            SaleDATE = DateTime.Now
                        };

                        db.Partner_products.Add(partnerProduct);
                        db.SaveChanges();  // Сохраняем запись о продукции в базе данных

                        MessageBox.Show("Партнер и продукция были добавлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Некорректное количество или продукт не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                NavigationService?.Navigate(new Discounts());  // Переход на страницу скидок
                ClearFields();  // Очищаем форму
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  // Показываем сообщение об ошибке
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
            TovariTypes.SelectedItem = null;
            TovariOrg.SelectedItem = null;
            QuantityOrg.Text = string.Empty;
        }
    }
}