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
        private static readonly Regex FIOregex = new Regex(@"^[А-ЯЁ][а-яё]+$");
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static readonly Regex INNRegex = new Regex(@"^\d{10}$");
        private static readonly Regex QuantityRegex = new Regex(@"^\d+$");
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+?[1-9]\d{9,14}$");

        private Entities db;
        private Partner currentPartner;

        public AddPartner(Partner selectedPartner = null)
        {
            InitializeComponent();
            db = new Entities();
            LoadTypes();
            LoadProducts();
            LoadProductTypes();
            currentPartner = selectedPartner;

            if (currentPartner != null)
            {
                FillPartnerData();
            }
        }

        private void FillPartnerData()
        {
            NameOrg.Text = currentPartner.Name;
            EmailOrg.Text = currentPartner.Email;
            NumberOrg.Text = currentPartner.Number;
            DirectorOrg.Text = currentPartner.Director;
            AddressOrg.Text = currentPartner.Address;
            INNOrg.Text = currentPartner.INN;
            RatingOrg.Text = currentPartner.Rating.ToString();
            Type.SelectedValue = currentPartner.Type;

            var partnerProduct = db.Partner_products
                                   .FirstOrDefault(pp => pp.Partner == currentPartner.ID);

            if (partnerProduct != null)
            {
                TovariOrg.SelectedValue = partnerProduct.Product;
                QuantityOrg.Text = partnerProduct.Quantity.ToString();

                var product = db.Products.FirstOrDefault(p => p.ID == partnerProduct.Product);
                if (product != null)
                {
                    TovariTypes.SelectedValue = product.Type;
                }
            }
        }

        private void LoadTypes()
        {
            var types = db.Partner_type.ToList();
            Type.ItemsSource = types;
            Type.DisplayMemberPath = "Type";
            Type.SelectedValuePath = "ID";
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
            TovariTypes.SelectedValuePath = "ID";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string NameOrganization = NameOrg.Text.Trim();
            string DirectorOrganization = DirectorOrg.Text.Trim();
            string EmailOrganization = EmailOrg.Text.Trim();
            string NumberOrganization = NumberOrg.Text.Trim();
            string INNOrganization = INNOrg.Text.Trim();
            string RatingOrganization = RatingOrg.Text.Trim();
            string AddressOrganization = AddressOrg.Text.Trim();

            int? selectedType = (int?)Type.SelectedValue;
            string Quantity = QuantityOrg.Text.Trim();
            int? selectedProductID = (int?)TovariOrg.SelectedValue;

            try
            {
                // Валидация данных
                if (!ValidateInputs(DirectorOrganization, EmailOrganization, NumberOrganization, INNOrganization, RatingOrganization, Quantity))
                    return;

                if (currentPartner != null)
                {
                    UpdateExistingPartner(NameOrganization, DirectorOrganization, EmailOrganization, NumberOrganization, INNOrganization, AddressOrganization, RatingOrganization, selectedType, selectedProductID, Quantity);
                }
                else
                {
                    CreateNewPartner(NameOrganization, DirectorOrganization, EmailOrganization, NumberOrganization, INNOrganization, AddressOrganization, RatingOrganization, selectedType, selectedProductID, Quantity);
                }

                NavigationService?.Navigate(new Discounts());
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Произошла ошибка: {ex.Message}\nОбратитесь к администратору, если проблема повторяется.",
                    "Критическая ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdateExistingPartner(string name, string director, string email, string number, string inn, string address, string rating, int? type, int? productID, string quantity)
        {
            var attachedPartner = db.Partners.Find(currentPartner.ID);
            if (attachedPartner == null)
            {
                MessageBox.Show("Партнёр не найден в текущем контексте.");
                return;
            }

            attachedPartner.Name = name;
            attachedPartner.Director = director;
            attachedPartner.Email = email;
            attachedPartner.Number = number;
            attachedPartner.INN = inn;
            attachedPartner.Address = address;
            attachedPartner.Rating = int.Parse(rating);
            attachedPartner.Type = (int)type;

            var partnerProduct = db.Partner_products.FirstOrDefault(pp => pp.Partner == attachedPartner.ID);
            if (partnerProduct != null)
            {
                partnerProduct.Product = productID ?? partnerProduct.Product;
                partnerProduct.Quantity = int.TryParse(quantity, out int qty) ? qty : partnerProduct.Quantity;
            }
            else if (productID.HasValue && int.TryParse(quantity, out int qty))
            {
                db.Partner_products.Add(new Partner_products
                {
                    Partner = attachedPartner.ID,
                    Product = productID.Value,
                    Quantity = qty,
                    SaleDATE = DateTime.Now
                });
            }

            db.SaveChanges();
            MessageBox.Show("Данные партнёра успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CreateNewPartner(string name, string director, string email, string number, string inn, string address, string rating, int? type, int? productID, string quantity)
        {
            var newPartner = new Partner
            {
                Name = name,
                Director = director,
                Email = email,
                Number = number,
                INN = inn,
                Address = address,
                Rating = int.Parse(rating),
                Type = (int)type
            };

            db.Partners.Add(newPartner);
            db.SaveChanges();

            if (int.TryParse(quantity, out int qty) && productID.HasValue)
            {
                db.Partner_products.Add(new Partner_products
                {
                    Partner = newPartner.ID,
                    Product = productID.Value,
                    Quantity = qty,
                    SaleDATE = DateTime.Now
                });
                db.SaveChanges();
            }

            MessageBox.Show("Партнёр и продукция успешно добавлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool ValidateInputs(string director, string email, string number, string inn, string rating, string quantity)
        {
            if (!FIOValidation(director))
            {
                MessageBox.Show("ФИО директора некорректно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!PhoneNumberRegex.IsMatch(number))
            {
                MessageBox.Show("Некорректный номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!EmailRegex.IsMatch(email))
            {
                MessageBox.Show("Некорректный адрес электронной почты.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!INNRegex.IsMatch(inn))
            {
                MessageBox.Show("ИНН должен содержать ровно 10 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(rating, out int ratingValue) || ratingValue < 1 || ratingValue > 10)
            {
                MessageBox.Show("Рейтинг должен быть числом от 1 до 10.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!QuantityRegex.IsMatch(quantity))
            {
                MessageBox.Show("Количество должно быть числовым значением.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool FIOValidation(string FIO)
        {
            var splitFIO = FIO.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitFIO.Length < 2 || splitFIO.Length > 5)
                return false;

            return splitFIO.All(part => FIOregex.IsMatch(part) && part.Length <= 50);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            ClearFields();
        }

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