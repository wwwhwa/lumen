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
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Collections;

namespace transport.Pages
{
    public partial class Log : Page
    {
        private User currentUser;
        public Log(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            RefreshDataGrid();
        }
        String server_name = "HWAYA\\HWAYA";
        String data_base = "21.101_10_Transportation";

        private void RefreshDataGrid()
        {
            using (SqlConnection connection = new SqlConnection($"Data Source={server_name};Initial Catalog={data_base};Integrated Security=True"))
            {
                connection.Open();

                string query = "";

                switch (currentUser.GroupID)
                {
                    // Пользователь
                    case 1:
                        panel.Visibility= Visibility.Hidden;
                        query = "SELECT ArrivalTime as Отправление, DepartureTime as Прибытие, Date as Дата, Route.NumRoutes as Маршрут, s.NameStop as Начало, e.NameStop as Конец " +
                        "FROM Schedule " +
                        "JOIN Flight ON Schedule.FlightID = Flight.IDFlight " +
                        "JOIN Route ON Flight.RouteID = Route.IDRoutes " +
                        "JOIN Stop s ON Route.StartStopID = s.IDStop " +
                        "JOIN Stop e ON Route.EndStopID = e.IDStop";
                        break;

                    // Оператор
                    case 2:
                        query = "SELECT IDSchedule, ArrivalTime as Отправление, DepartureTime as Прибытие, Date as Дата, FlightID as idflight, Route.NumRoutes as 'Маршрут'" +
                        "FROM Schedule " +
                        "JOIN Flight ON Schedule.FlightID = Flight.IDFlight " +
                        "JOIN Route ON Flight.RouteID = Route.IDRoutes";
                        break;
                    // Админ
                    case 3:
                        query = "SELECT IDSchedule, ArrivalTime as Отправление, DepartureTime as Прибытие, Date as Дата, FlightID as idflight, Route.NumRoutes as 'Маршрут'" +
                        "FROM Schedule " +
                        "JOIN Flight ON Schedule.FlightID = Flight.IDFlight " +
                        "JOIN Route ON Flight.RouteID = Route.IDRoutes";
                        break;
                    // Супер-админ
                    case 4:
                        query = "SELECT IDSchedule, ArrivalTime as Отправление, DepartureTime as Прибытие, Date as Дата, FlightID as idflight, Route.NumRoutes as 'Маршрут'" +
                        "FROM Schedule " +
                        "JOIN Flight ON Schedule.FlightID = Flight.IDFlight " +
                        "JOIN Route ON Flight.RouteID = Route.IDRoutes";
                        break;
                }
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgv.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = $"Data Source={server_name};Initial Catalog={data_base};Integrated Security=True";
            string searchText = tbSearch.Text;

            // Преобразовать и проверить значение на наличие ошибок
            int searchValue;
            bool isValid = int.TryParse(searchText, out searchValue);

            if (searchText.Length == 0)
            {
                // Вывести всю таблицу
                RefreshDataGrid();
            }
            else
            {
                if (!isValid)
                {
                    // Показать сообщение об ошибке
                    System.Windows.MessageBox.Show("Неверный формат поискового запроса. Введите целое число.");
                    return;
                }

                // Используйте значение для фильтрации данных в таблице
                string query = $"SELECT ArrivalTime as Отправление, DepartureTime as Прибытие, Date as Дата, " +
                  $"r1.NumRoutes as Маршрут, s.NameStop as Начало, e.NameStop as Конец " +
                  $"FROM Schedule " +
                  $"JOIN Flight ON Schedule.FlightID = Flight.IDFlight " +
                  $"JOIN Route r1 ON Flight.RouteID = r1.IDRoutes " +
                  $"JOIN Stop s ON r1.StartStopID = s.IDStop " +
                  $"JOIN Stop e ON r1.EndStopID = e.IDStop " +
                  $"WHERE r1.NumRoutes = {searchValue}";

                // Заполнить таблицу результатами поиска
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgv.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            // Получение данных из TextBox
            string id = tbid.Text;
            string flight = tbflightid.Text;
            string arrival = tbarrival.Text;
            string departure = tbdeparture.Text;
            string date = tbdate.Text;

            SqlConnection connection = new SqlConnection($"Data Source={server_name};Initial Catalog={data_base};Integrated Security=True");

            // Открываем соединение
            connection.Open();

            // Проверяем, существует ли запись в таблице route с указанным id
            string checkIdExistenceQuery = $"SELECT COUNT(*) FROM Schedule WHERE IDSchedule = {id}";
            SqlCommand checkIdExistenceCommand = new SqlCommand(checkIdExistenceQuery, connection);

            int idExistence = Convert.ToInt32(checkIdExistenceCommand.ExecuteScalar());

            if (idExistence > 0)
            {
                // Запись существует, выполняем обновление
                string updateQuery = $"UPDATE Schedule " +
                    $"SET FlightID = '{flight}', " +
                    $"ArrivalTime = '{arrival}', " +
                    $"DepartureTime = '{departure}', " +
                    $"Date = '{date}' " +
                    $"WHERE IDSchedule = {id}";

                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.ExecuteNonQuery();
                System.Windows.MessageBox.Show("данные обновлены");
            }
            else
            {
                string insertQuery = $"INSERT INTO Schedule (FlightID, ArrivalTime, DepartureTime, Date)" +
                                    $"VALUES ('{flight}', '{arrival}', '{departure}', '{date}')";

                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.ExecuteNonQuery();
                System.Windows.MessageBox.Show("данные добавлены");
            }
            RefreshDataGrid();
            connection.Close();
        }
    }
}
