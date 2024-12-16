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
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+?[1-9]\d{9,14}$");

        private readonly Entities db;
        private readonly Partner currentPartner;
        public AddPartner(Partner selectedPartner = null)
        {
            InitializeComponent();
            db = new Entities();
            LoadTypes();
            currentPartner = selectedPartner;

            if (currentPartner != null)
            {
                NameOrg.Text = currentPartner.Name;
                EmailOrg.Text = currentPartner.Email;
                NumberOrg.Text = currentPartner.Number;
                DirectorOrg.Text = currentPartner.Director;
                AddressOrg.Text = currentPartner.Address;
                INNOrg.Text = currentPartner.INN;  // Здесь могут быть проблемы
                RatingOrg.Text = currentPartner.Rating.ToString();

                if (currentPartner.Type != null)
                {
                    Type.SelectedValue = currentPartner.Type;
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameOrg.Text;
            string Director = DirectorOrg.Text;
            string Email = EmailOrg.Text;
            string Number = NumberOrg.Text;
            string INN = INNOrg.Text;
            string Rating = RatingOrg.Text;
            string Address = AddressOrg.Text;

            try
            {
                if (!FIOValidation(Director))
                {
                    MessageBox.Show("ФИО директора было введено неверно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                if (!PhoneNumberRegex.IsMatch(Number))
                {
                    MessageBox.Show("Введите корректный формат номера телефона. К примеру: +78765431212", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!EmailRegex.IsMatch(Email))
                {
                    MessageBox.Show("Электронная почта была введена неверно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!INNRegex.IsMatch(INN))
                {
                    MessageBox.Show("ИНН должжен содержать 10 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(Rating, out int ratingValue) || ratingValue < 1 || ratingValue > 10)
                {
                    MessageBox.Show("Рейтинг должен быть числом от 1 до 10.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (currentPartner != null)
                {
                    var partnerToUpdate = db.Partners.FirstOrDefault(x => x.ID == currentPartner.ID);
                    var selectedTypeID = (int)Type.SelectedValue;


                    if (partnerToUpdate != null)
                    {
                        partnerToUpdate.Type = selectedTypeID;
                        partnerToUpdate.Name = Name;
                        partnerToUpdate.Email = Email;
                        partnerToUpdate.Number = Number;
                        partnerToUpdate.Address = Address;
                        partnerToUpdate.Director = Director;
                        partnerToUpdate.INN = INN;
                        partnerToUpdate.Rating = ratingValue;

                        db.SaveChanges();
                        MessageBox.Show("Данные партнера были изменены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearFields();
                        NavigationService?.Navigate(new Partners());
                    }
                    else
                    {
                        MessageBox.Show("Партнер не найден в системе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
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
                    db.Partners.Add(newPartner);
                    db.SaveChanges();
                    MessageBox.Show("Партнер был добавлен в систему");
                    ClearFields();
                    NavigationService?.Navigate(new Partners());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не получилось добавить партнера.", ex.Message);
            }

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            ClearFields();
        }

        private bool FIOValidation(string FIO)
        {
            var splitFIO = FIO.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (splitFIO.Length < 2 || splitFIO.Length > 5)
            {
                return false;
            }

            foreach (var part in splitFIO)
            {
                if (string.IsNullOrWhiteSpace(part) || !FIOregex.IsMatch(part) || part.Length > 50)
                {
                    return false;
                }
            }
            return true;
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
        }
    }
}
