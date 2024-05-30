using Diamant.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Diamant.View
{
    public partial class DetailReturnedItem : Window
    {
        private ReturnedItem currentReturnedItems;
        private Client clientFIO;
        private Employee employeeFIO;
        public DetailReturnedItem(ReturnedItem currentReturnedItems)
        {
            InitializeComponent();
            DataContext = currentReturnedItems;

            this.currentReturnedItems = currentReturnedItems;

            using (var db = new PawnshopContext())
            {
                clientFIO = db.Clients.FirstOrDefault(c => c.ClientId == currentReturnedItems.ClientId);
                employeeFIO = db.Employees.FirstOrDefault(e => e.EmployeeId == currentReturnedItems.EmployeeId);

                name.Content = currentReturnedItems.NameProduct.ToString();
                assessedValue.Content = currentReturnedItems.AssessedValue.ToString("C2");
                bailAmount.Content = currentReturnedItems.BailAmount.ToString("C2");
                dueDate.Content = currentReturnedItems.DueDate.ToString();
                shelfLife.Content = currentReturnedItems.ShelfLife.ToString();
                statusProduct.Content = currentReturnedItems.StatusProduct.ToString();
                if (clientFIO != null)
                {
                    clientID.Content = clientFIO.FullNameClient;
                }
                else
                {
                    clientID.Content = "Клиент не найден";
                }

                if (employeeFIO != null)
                {
                    employeeID.Content = employeeFIO.FullNameEmployee;
                }
                else
                {
                    employeeID.Content = "Сотрудник не найден";
                }
                deletionDate.Content = currentReturnedItems.DeletionDate.ToString();

            }
        }

        private void back_OnClick(object sender, RoutedEventArgs e)
        {
            ReturnedProduct wReturnedItem = new();
            wReturnedItem.Show();
            Close();
        }

        private void report_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentReturnedItems == null || clientFIO == null || employeeFIO == null)
            {
                MessageBox.Show("Ошибка: Не удалось загрузить данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Excel файл (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = ".xlsx";
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string fileName = saveFileDialog.FileName;
                FileInfo newFile = new FileInfo(fileName);
                ExcelPackage excelPackage;

                if (newFile.Exists)
                {
                    excelPackage = new ExcelPackage(newFile);
                }
                else
                {
                    excelPackage = new ExcelPackage();
                }

                ExcelWorksheet worksheet;
                if (excelPackage.Workbook.Worksheets.Count > 0)
                {
                    worksheet = excelPackage.Workbook.Worksheets[0];
                    worksheet.Cells.Clear();
                }
                else
                {
                    worksheet = excelPackage.Workbook.Worksheets.Add("DetailReturnedItem");
                }
                worksheet.Cells[1, 1].Value = "Наименование:";
                worksheet.Cells[1, 2].Value = currentReturnedItems.NameProduct;

                worksheet.Cells[2, 1].Value = "Оценочная стоимость:";
                worksheet.Cells[2, 2].Value = currentReturnedItems.AssessedValue.ToString("C2");

                worksheet.Cells[3, 1].Value = "Выданная сумма:";
                worksheet.Cells[3, 2].Value = currentReturnedItems.BailAmount.ToString("C2");

                worksheet.Cells[4, 1].Value = "Дата сдачи:";
                worksheet.Cells[4, 2].Value = currentReturnedItems.DueDate.ToString();

                worksheet.Cells[5, 1].Value = "Срок хранения:";
                worksheet.Cells[5, 2].Value = currentReturnedItems.ShelfLife.ToString();

                worksheet.Cells[6, 1].Value = "Статус:";
                worksheet.Cells[6, 2].Value = currentReturnedItems.StatusProduct.ToString();

                worksheet.Cells[7, 1].Value = "Клиент:";
                worksheet.Cells[7, 2].Value = clientFIO.FullNameClient.ToString();

                worksheet.Cells[8, 1].Value = "Сотрудник:";
                worksheet.Cells[8, 2].Value = employeeFIO.FullNameEmployee.ToString();

                worksheet.Cells[9, 1].Value = "Дата возврата:";
                worksheet.Cells[9, 2].Value = currentReturnedItems.DeletionDate.ToString();

                excelPackage.SaveAs(newFile);
                excelPackage.SaveAs(newFile);
                MessageBox.Show("Данные успешно экспортированы в файл " + fileName, "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}