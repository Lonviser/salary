using System;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private SalaryCalculator calculator;

        public Form1()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double salary = double.Parse(textBox1.Text);
                double numberOfChildren = double.Parse(textBox2.Text);
                double standardTaxDeduction = double.Parse(textBox14.Text);
                double standardTaxDeductionCount = double.Parse(textBox13.Text);
                double incomeTaxPercent = double.Parse(textBox5.Text);
                double numberOfOldChildren = double.Parse(textBox3.Text);
                double taxDeduction = double.Parse(textBox10.Text);
                double lnor1 = double.Parse(textBox8.Text);
                double lnor2 = double.Parse(textBox9.Text);
                double creditPayment = double.Parse(textBox15.Text);
                double pensionFund = double.Parse(textBox6.Text);
                double unionDues = double.Parse(textBox7.Text);

                double[] calculatedSalary = { salary, numberOfChildren, standardTaxDeduction, standardTaxDeductionCount, incomeTaxPercent, numberOfOldChildren, taxDeduction, lnor1, lnor2, creditPayment, pensionFund, unionDues };

                calculator = new SalaryCalculator(calculatedSalary);
                textBox4.Text = Convert.ToString(calculator.CalculateSalary(textBox11, textBox12));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Файлы Excel (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*",
                    Title = "Выберите файл Excel"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(openFileDialog.FileName)))
                    {
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];

                        // Индексы строк для соответствующих параметров
                        int[] rowIndices = { 1, 2, 4, 5, 6, 7, 8, 9 };  // Обновлен порядок

                        // Индексы колонок с данными
                        int columnIndex = 2;  // Обновлен индекс

                        // Загрузка данных в соответствующие текстовые поля
                        textBox1.Text = worksheet.Cells[rowIndices[0], columnIndex].Value?.ToString();
                        textBox2.Text = worksheet.Cells[rowIndices[1], columnIndex].Value?.ToString();
                        textBox14.Text = worksheet.Cells[rowIndices[2], columnIndex].Value?.ToString();
                        textBox5.Text = worksheet.Cells[rowIndices[3], columnIndex].Value?.ToString();
                        textBox6.Text = worksheet.Cells[rowIndices[4], columnIndex].Value?.ToString();
                        textBox7.Text = worksheet.Cells[rowIndices[5], columnIndex].Value?.ToString();  // Обновлено
                        textBox3.Text = worksheet.Cells[rowIndices[6], columnIndex].Value?.ToString();  // Добавлено
                        textBox8.Text = worksheet.Cells[rowIndices[7], columnIndex].Value?.ToString();
                        textBox9.Text = worksheet.Cells[rowIndices[8], columnIndex].Value?.ToString();

           
                            textBox10.Text = worksheet.Cells[rowIndices[9], columnIndex].Value?.ToString();
                        
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }








        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("ДеталиЗарплаты");
                    worksheet.Cells[1, 1].Value = "Зарплата";
                    worksheet.Cells[1, 2].Value = Convert.ToDouble(textBox1.Text);
                    worksheet.Cells[3, 1].Value = "Отчислено";
                    worksheet.Cells[3, 2].Value = "ПодоходныйНалог";
                    worksheet.Cells[3, 3].Value = "К выдаче";

                    double accrued = calculator.CalculateSalary(textBox11, textBox12);
                    worksheet.Cells[4, 1].Value = Convert.ToDouble(calculator.Expelled);
                    worksheet.Cells[4, 2].Value = Convert.ToDouble(calculator.IncomeTax);
                    worksheet.Cells[4, 3].Value = Convert.ToDouble(accrued);

                    var saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Файлы Excel (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*",
                        FileName = "РасчётЗарплаты.xlsx"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo file = new FileInfo(saveFileDialog.FileName);
                        excelPackage.SaveAs(file);
                    }
                }

                MessageBox.Show("Итоговые данные успешно экспортированы в файл.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
    }

    public class SalaryCalculator
    {
        private double[] initialValues;
        public double Expelled { get; private set; }
        public double IncomeTax { get; private set; }

        public SalaryCalculator(double[] initialValues)
        {
            if (initialValues.Length != 12)
            {
                throw new ArgumentException("Массив должен содержать 12 элементов.", nameof(initialValues));
            }

            this.initialValues = initialValues;
        }

        public double CalculateSalary(TextBox expelledTextBox, TextBox incomeTaxTextBox)
        {
            double salary = initialValues[0];
            double numberOfChildren = initialValues[1];
            double standardTaxDeduction = initialValues[2];
            double standardTaxDeductionCount = initialValues[3];
            double incomeTaxPercent = initialValues[4];
            double numberOfOldChildren = initialValues[5];
            double taxDeduction = initialValues[6];
            double lnor1 = initialValues[7];
            double lnor2 = initialValues[8];
            double creditPayment = initialValues[9];
            double pensionFund = initialValues[10];
            double unionDues = initialValues[11];

            double tuitionAmount = taxDeduction * numberOfOldChildren;

            double lnor = (numberOfChildren > 1) ? lnor2 : lnor1;

            double incomeTax;
            if (salary < standardTaxDeduction)
            {
                incomeTax = (salary - standardTaxDeductionCount - tuitionAmount - lnor * numberOfChildren - creditPayment) * incomeTaxPercent / 100;
            }
            else
            {
                incomeTax = (salary - tuitionAmount - lnor * numberOfChildren - creditPayment) * incomeTaxPercent / 100;
            }

            double expelled = incomeTax + salary * (pensionFund / 100) + salary * (unionDues / 100);
            double accrued = salary - expelled;

            Expelled = expelled;
            IncomeTax = incomeTax;

            expelledTextBox.Text = Convert.ToString(expelled);
            incomeTaxTextBox.Text = Convert.ToString(incomeTax);

            return accrued;
        }
    }
}
