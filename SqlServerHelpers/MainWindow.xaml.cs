using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Reflection;

namespace SqlServerHelpers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            b_test.Click += B_test_Click;

            var pwd_save = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "pwd.user");
            if (File.Exists(pwd_save))
                t_password.Password = File.ReadAllText(pwd_save);
        }

        string connectionStringBase = null;
        private void B_test_Click(object sender, RoutedEventArgs e)
        {
            connectionStringBase = null;



            var conn = $@"Data Source={ (t_server.Text + (string.IsNullOrWhiteSpace(t_instance.Text) ? "" : ("\\" + t_instance.Text)))};Initial Catalog=@@@master@@@;max pool size=500;Persist Security Info=True;
User ID={t_user.Text};Password={t_password.Password};MultipleActiveResultSets=true;Connection Timeout=15";

            if (ExecuteScalar("select count(*) from sys.tables", conn.Replace("@@@", "")) == null)
            {
                MessageBox.Show("Connection error");
                return;
            }

            MessageBox.Show("Success");
            connectionStringBase = conn;
            t_databases.ItemsSource = FillList("select * from sys.databases", connectionStringBase.Replace("@@@", ""));

        }


        public static List<string> FillList(string query, string connstr = null)
        {
            List<string> tagsList = new List<string>();

            var sqlconn = new SqlConnection(connstr);

            try
            {


                sqlconn.Open();

                using (IDbCommand command = sqlconn.CreateCommand())
                {
                    command.CommandText = query;

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                tagsList.Add(reader.GetString(0));
                        }

                        reader.Close();
                    }
                }


            }
            catch
            {
            }
            try
            {
                sqlconn.Close();
            }
            catch
            {


            }

            return tagsList;
        }

        public static object ExecuteScalar(string command, string connstr = null)
        {
            try
            {

                var sqlconn = new SqlConnection(connstr);

                var sqlcmd = new SqlCommand
                {
                    Connection = sqlconn,
                    CommandType = CommandType.Text,
                    CommandText = command
                };


                sqlconn.Open();

                try
                {
                    var x = sqlcmd.ExecuteScalar();
                    sqlconn.Close();
                    return x;
                }
                catch
                {

                }
                sqlconn.Close();

            }
            catch
            {

            }
            return null;
        }
    }
}
