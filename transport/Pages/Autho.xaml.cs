using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using System.Xml.Linq;
using transport.Model;

namespace transport.Pages
{
    public partial class Autho : Page
    {
        public Autho()
        {
            InitializeComponent();
            
        }
        
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateAcc());
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxLogin.Text) && !String.IsNullOrEmpty(pasboxPassword.Password))
            {
                LoginUser();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль");
            }
        }
        String server_name = "HWAYA\\HWAYA";
        String data_base = "21.101_10_Transportation";
        String table_name = "Users";

        private void LoginUser()
        {
            
            var db = new SqlConnection($"Data Source={server_name};Initial Catalog={data_base};Integrated Security=True");

            var user = new User();
            user.Login = tbxLogin.Text;
            user.Password = pasboxPassword.Password;

            if (!db.State.Equals(ConnectionState.Open))
            {
                db.Open();
            }

            string query = $"SELECT GroupID FROM {table_name} WHERE Login = '{user.Login}' AND Password = '{user.Password}'";
            SqlCommand cmd = new SqlCommand(query, db);
            cmd.Parameters.AddWithValue("Login", user.Login);
            cmd.Parameters.AddWithValue("Password", user.Password);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                user.GroupID = Convert.ToInt32(reader["GroupID"]);
                
                switch (user.GroupID)
                {
                    case 1:
                        NavigationService.Navigate(new Log(user));
                        MessageBox.Show("вы вошли как Пользователь: " + user.Login, "группа: " + user.GroupID);
                        break;
                    case 2:
                        NavigationService.Navigate(new Log(user));
                        MessageBox.Show("вы вошли как Оператор: " + user.Login, "группа: " + user.GroupID);
                        break;
                    case 3:
                        NavigationService.Navigate(new Log(user));
                        MessageBox.Show("вы вошли как Админ: " + user.Login, "группа: " + user.GroupID);
                        break;
                    case 4:
                        NavigationService.Navigate(new Log(user));
                        MessageBox.Show("вы вошли как Супер-админ: " + user.Login, "группа: " + user.GroupID);
                        break;
                }
                tbxLogin.Text = "";
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            
            db.Close();
        }
    }
}