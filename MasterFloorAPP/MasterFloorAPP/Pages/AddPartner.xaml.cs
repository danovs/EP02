using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterFloorAPP.Pages
{
    public partial class AddPartner : Page
    {
        private static readonly Regex FIOregex = new Regex(@"^[А-ЯЁ][а-яё]+$");
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static readonly Regex INNRegex = new Regex(@"^\d{20}$");
        private static readonly Regex PhoneNumberRegex = new Regex(@"^\+?[1-9]\d{9,14}$");

        private readonly Entities db;
        public AddPartner()
        {
            InitializeComponent();
            db = new Entities();
            LoadTypes();
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
                }

                if (!EmailRegex.IsMatch(Email))
                {
                    MessageBox.Show("Электронная почта была введена неверно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (!INNRegex.IsMatch(INN))
                {
                    MessageBox.Show("ИНН должжен содержать 20 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(Rating, out int ratingValue) || ratingValue < 1 || ratingValue > 10)
                {
                    MessageBox.Show("Рейтинг должен быть числом от 1 до 10.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

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
                NavigationService.GoBack();
                ClearFields();
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
