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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace session
{
    /// <summary>
    /// Interaction logic for adding.xaml
    /// </summary>
    public partial class adding : Window
    {
        int num;
        string imag;
        public adding(string id)
        {
            InitializeComponent();
            num = Int32.Parse(id);
            LoadOut();
        }
        public void LoadOut()
        {
            string connect = @"data source=vc-stud-mssql1;user id=user88_db;password=user88;MultipleActiveResultSets=True;App=EntityFramework";
            string command = "Select * from Product where ID='" + num + "' ";
            SqlConnection myConnection = new SqlConnection(@connect);
            SqlCommand myCommand = new SqlCommand(command, myConnection);
            myConnection.Open();
            SqlDataReader rd = myCommand.ExecuteReader();
            string nam = "null";
            string coost = "null";
            string manufacturer = "null";
            string description = "null";
            string pic = "null";
            while (rd.Read())
            {
                nam = rd[1].ToString();
                coost = rd[2].ToString();
                manufacturer = rd[6].ToString();
                description = rd[3].ToString();
                pic = rd[4].ToString();
            }
            index.Content = num;
            name.Text = nam;
            cost.Text = coost;
            manuf.Text = manufacturer;
            descr.Text = description;
            Uri imgUri = new Uri(pic);
            ImageSource i = new BitmapImage(imgUri);
            img.Source = i;
            myConnection.Close();
        }

        private void clean_Click(object sender, RoutedEventArgs e)
        {
            index.Content = null;
            name.Text = null;
            cost.Text = null;
            manuf.Text = null;
            descr.Text = null;
            img.Source = null;
        }
        private void act(string command)
        {
            string connect = @"data source=vc-stud-mssql1;user id=user88_db;password=user88;MultipleActiveResultSets=True;App=EntityFramework";
            SqlConnection myConnection = new SqlConnection(@connect);
            SqlCommand Mycommand = new SqlCommand(command, myConnection);
            myConnection.Open();
            Mycommand.ExecuteNonQuery();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Вы уверены, что хотите удалить данный товар?", "Удаление", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) 
            {
                string command = "Delete From Product where ID ='" + index.Content + "'";
                act(command);
                index.Content = null;
                name.Text = null;
                cost.Text = null;
                manuf.Text = null;
                descr.Text = null;
                img.Source = null;
            }
            else
            {
                System.Windows.MessageBox.Show("Удаление отменено!");
            }
            
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string nam = name.Text;
            string coost = cost.Text;
            string manufacturer = manuf.Text;
            string description = descr.Text;
            string pic = imag;
            string command = "Update Product set Title = '" + nam + "' and Cost = '" + coost + "' and Description = '" + description + "' and Manufacturer = '" + manufacturer + " and MainImagePath = '" + pic + "''";
            act(command);
            System.Windows.MessageBox.Show(string.Format("Данные успешно изменены!"), "Сообщение");
        }

        private void addprod_Click(object sender, RoutedEventArgs e)
        {
            string connect = @"data source=vc-stud-mssql1;user id=user88_db;password=user88;MultipleActiveResultSets=True;App=EntityFramework";
            SqlConnection myConnection = new SqlConnection(@connect);
            string command = "Insert into Product (Title,Cost,Description,MainImagePath,Manufacturer) values ('{0}','{1}','{2}','{3}','{4}')";
            myConnection.Open();
            string nam = name.Text;
            string coost = cost.Text;
            string description = descr.Text;
            string pic = imag;
            string manufacturer = manuf.Text;
            string sInsSotr = string.Format(command,nam, coost, description, pic, manufacturer);
            SqlCommand cmdIns = new SqlCommand(sInsSotr, myConnection);
            cmdIns.ExecuteNonQuery();
            index.Content = null;
            name.Text = null;
            cost.Text = null;
            manuf.Text = null;
            descr.Text = null;
            img.Source = null;
        }

        private void addph_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png|all files (*.*)|*.*";
            try
            {
                openFileDialog1.ShowDialog();
                Uri imgUri = new Uri(openFileDialog1.FileName);
                ImageSource i = new BitmapImage(imgUri);
                img.Source = i;
                imag = openFileDialog1.FileName;
            }
            catch
            {
                System.Windows.MessageBox.Show(string.Format("Невозможно открыть выбранный файл!"), "Сообщение");
            }
        }
    }
    

    
}
