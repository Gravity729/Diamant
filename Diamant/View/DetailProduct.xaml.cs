using Diamant.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LicenseContext = OfficeOpenXml.LicenseContext;


namespace Diamant.View
{
    public partial class DetailProduct : Window
    {
        private Product currentProducts;
        private Client clientFIO;
        private Employee employeeFIO;

        public DetailProduct(Product currentProducts)
        {
            InitializeComponent();
            DataContext = currentProducts;

            this.currentProducts = currentProducts;

            using (var db = new PawnshopContext())
            {
                clientFIO = db.Products.Include(b => b.Client).FirstOrDefault(b => b.ClientId == currentProducts.ClientId).Client;
                employeeFIO = db.Products.Include(b => b.Employee).FirstOrDefault(b => b.EmployeeId == currentProducts.EmployeeId).Employee;

                name.Content = currentProducts.NameProduct.ToString();
                assessedValue.Content = currentProducts.AssessedValue.ToString("C2");
                bailAmount.Content = currentProducts.BailAmount.ToString("C2");
                dueDate.Content = currentProducts.DueDate.ToString();
                shelfLife.Content = currentProducts.ShelfLife.ToString();
                statusProduct.Content = currentProducts.StatusProduct.ToString();
                clientID.Content = clientFIO.FullNameClient.ToString();
                employeeID.Content = employeeFIO.FullNameEmployee.ToString();
            }
        }


        private void back_OnClick(object sender, RoutedEventArgs e)
        {
            Products ProductsWindow = new();
            ProductsWindow.Show();
            Close();
        }

        private void report_OnClick(object sender, RoutedEventArgs e)
        {
            if (currentProducts == null || clientFIO == null || employeeFIO == null)
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
                    worksheet = excelPackage.Workbook.Worksheets.Add("DetailProduct");
                }
                worksheet.Cells[1, 1].Value = "Наименование:";
                worksheet.Cells[1, 2].Value = currentProducts.NameProduct;

                worksheet.Cells[2, 1].Value = "Оценочная стоимость:";
                worksheet.Cells[2, 2].Value = currentProducts.AssessedValue.ToString("C2");

                worksheet.Cells[3, 1].Value = "Выданная сумма:";
                worksheet.Cells[3, 2].Value = currentProducts.BailAmount.ToString("C2");

                worksheet.Cells[4, 1].Value = "Дата сдачи:";
                worksheet.Cells[4, 2].Value = currentProducts.DueDate.ToString();

                worksheet.Cells[5, 1].Value = "Срок хранения:";
                worksheet.Cells[5, 2].Value = currentProducts.ShelfLife.ToString();

                worksheet.Cells[6, 1].Value = "Статус:";
                worksheet.Cells[6, 2].Value = currentProducts.StatusProduct.ToString();

                worksheet.Cells[7, 1].Value = "Клиент:";
                worksheet.Cells[7, 2].Value = clientFIO.FullNameClient.ToString();

                worksheet.Cells[8, 1].Value = "Сотрудник:";
                worksheet.Cells[8, 2].Value = employeeFIO.FullNameEmployee.ToString();

                excelPackage.SaveAs(newFile);
                excelPackage.SaveAs(newFile);
                MessageBox.Show("Данные успешно экспортированы в файл " + fileName, "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
