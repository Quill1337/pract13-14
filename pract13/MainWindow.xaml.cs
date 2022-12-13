using Microsoft.Win32;
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
using LibMas;
using System.IO;

namespace pract13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LibMas.Array _myArray;
        public MainWindow()
        {
            LoginWindow password = new LoginWindow();
            password.ShowDialog();
            try
            {
                using (StreamReader reader = new StreamReader("config.ini"))
                {
                    
                    int i = Convert.ToInt32(reader.ReadLine());
                    int j = Convert.ToInt32(reader.ReadLine());
                    _myArray = new LibMas.Array(i,j);
                    _myArray.Fill();
                    InitializeComponent();
                    dataGrid.ItemsSource = _myArray.ToDataTable().DefaultView;
                    sizeColumn.Text= $"{i} столбцов";
                    sizeRow.Text = $"{j} строк";
                }
            }
            catch
            {

            }
    
            InitializeComponent();
        }

        

        private void GetMinimalElement_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count == 0)
            {
                MessageBox.Show("Заполните матрицу!");
            }
            else
            {
                int min = 0;
                int i = 0;
                int product;
                for (int p = 0; p < _myArray.ColumnLength; p++)
                {
                    product = 1;
                    for (int k = 0; k < _myArray.RowLength; k++)
                    {
                        product *= _myArray[k, p];
                    }
                    if (p == 0)
                    {
                        min = product;
                    }
                    if (product <= min)
                    {
                        min = product;
                        i = p;
                    }
                }
                proizv.Text = min.ToString();
                stolbec.Text = i.ToString();
            }
        }

        private void FillUpArray_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(row.Text) || string.IsNullOrEmpty(column.Text))
            {
                MessageBox.Show("Введите размер матрицы!");
            }
            else
            {
                _myArray = new LibMas.Array(int.Parse(row.Text), int.Parse(column.Text));
                sizeRow.Text = $"Строк {row.Text}";
                sizeColumn.Text = $"Столбцов {column.Text}";
                _myArray.Fill();
                dataGrid.ItemsSource = _myArray.ToDataTable().DefaultView;
            }
        }

        private void SaveArray_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.ItemsSource == null)
            {
                MessageBox.Show("Заполните матрицу перед сохранением!", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*",
                    DefaultExt = ".txt",
                    FilterIndex = 1,
                    Title = "Экспорт массива"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    _myArray.Export(saveFileDialog.FileName);
                }
                dataGrid.ItemsSource = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void OpenArray_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*",
                    DefaultExt = ".txt",
                    FilterIndex = 1,
                    Title = "Импорт массива"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    _myArray.Import(openFileDialog.FileName);
                }
                dataGrid.ItemsSource = _myArray.ToDataTable().DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void AboutProgramm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Митрофанов Роман ИСП-31\nДана матрица размера M * N. Найти номер ее столбца с наименьшим произведением элементов и вывести данный номер, а также значение наименьшегопроизведения.");
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            row.Clear();
            column.Clear();
            proizv.Clear();
        }

        private void TBoxChangeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            proizv.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow password = new LoginWindow();
            password.Owner = this;
            password.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Options options = new Options();
            options.ShowDialog();
        }
    }
}
