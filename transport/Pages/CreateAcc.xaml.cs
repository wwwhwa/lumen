using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Shapes;
using transport.Model;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace transport.Pages
{
    public partial class CreateAcc : Page
    {
        public CreateAcc()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxSurname.Text) && !String.IsNullOrEmpty(tbxName.Text) && !String.IsNullOrEmpty(tbxLogin.Text) && !String.IsNullOrEmpty(tbxPassword.Text))
            {
                SaveUser();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите все данные");
            }
        }
        String server_name = "HWAYA\\HWAYA";
        String data_base = "21.101_10_Transportation";
        String table_name = "Users";
        private void SaveUser()
        {
            var db = new SqlConnection($"Data Source={server_name};Initial Catalog={data_base};Integrated Security=True");

            // Создаем новый объект класса `User`
            var user = new User();

            // Заполняем данные пользователя
            user.Surname = tbxSurname.Text;
            user.Name = tbxName.Text;
            user.Login = tbxLogin.Text;
            user.Password = tbxPassword.Text;
            user.GroupID = 1;

            if (!db.State.Equals(ConnectionState.Open))
            {
                db.Open();
            }
            // Создаем подключение к базе данных
            string query = $"INSERT INTO {table_name} (Surname, Name, Login, Password, GroupID) VALUES ('{user.Surname}', '{user.Name}', '{user.Login}', '{user.Password}', {user.GroupID})";

            SqlCommand cmd = new SqlCommand(query, db);
            cmd.ExecuteNonQuery();

            // Закрываем подключение к базе данных
            db.Close();

            // Выводим сообщение об успешном сохранении пользователя
            MessageBox.Show("Пользователь успешно зарегистрирован");
            NavigationService.Navigate(new Autho());
        }
    }
}
