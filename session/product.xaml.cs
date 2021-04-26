using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace session
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class product : Window
    {
        public product()
        {
            InitializeComponent();
            string command = "Select ID,MainImagePath,Title,Cost,IsActive From Product";
            act(command);
            filtr.Text = "Все элементы";
        }
        private void act(string command)
        {
            string connect = @"data source=vc-stud-mssql1;user id=user88_db;password=user88;MultipleActiveResultSets=True;App=EntityFramework";
            SqlConnection myConnection = new SqlConnection(@connect);
            SqlCommand Mycommand = new SqlCommand(command, myConnection);
            myConnection.Open();
            Mycommand.ExecuteNonQuery();
            SqlDataAdapter dapter = new SqlDataAdapter(Mycommand);
            DataTable Table = new DataTable("Auto");
            dapter.Fill(Table);
            products.ItemsSource = Table.DefaultView;
            myConnection.Close();
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string command = "Select ID,MainImagePath,Title,Cost,IsActive From Product Where Title like '" + search.Text + "%' or Description like '" + search.Text + "%'";
            act(command);
        }

        private void ybyvanie_Click(object sender, RoutedEventArgs e)
        {
            string command = "Select ID,MainImagePath,Title,Cost,IsActive From Product Order by Cost Desc";
            act(command);
        }

        private void vozrastanie_Click(object sender, RoutedEventArgs e)
        {
            string command = "Select ID,MainImagePath,Title,Cost,IsActive From Product Order by Cost Asc";
            act(command);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Вы уверены, что хотите удалить данный товар?", "Удаление", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) 
            {
                DataRowView rowView = products.SelectedValue as DataRowView;
                string command = "Delete From Product where ID ='" + rowView[0] + "'";
                act(command);
            }
            else
            {
                System.Windows.MessageBox.Show("Удаление отменено!");
            }

        }

        private void products_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView rowView = products.SelectedValue as DataRowView;
            string id = rowView[0].ToString();
            adding s = new adding(id);
            s.Show();
        }

        private void filtr_SelectionChanged(object sender, EventArgs e)
        {
            if (filtr.Text == "Все элементы")
            {
                string command = "Select ID,MainImagePath,Title,Cost,IsActive From Product";
                act(command);
            }
            else
            {
                string command = "Select ID,MainImagePath,Title,Cost,IsActive,Manufacturer From Product Where Manufacturer = '" + filtr.Text + "'";
                act(command);
            }
        }
    }
}
