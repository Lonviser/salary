using System;
using System.Windows.Forms;
using System.Reflection;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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

            SalaryCalculator calculator = new SalaryCalculator(calculatedSalary);
            textBox4.Text = Convert.ToString(calculator.CalculateSalary(textBox11, textBox12)); 
        }

        public class SalaryCalculator
        {
            private double salary; //��
            private double accrued; //���������
            private double numberOfChildren; //���������� ����� 
            private double standardTaxDeduction; // ����������� ��������� ����� (�� ����� �����)
            private double standardTaxDeductionCount;// ����������� ��������� ����� (��� ��������)
            private double incomeTaxPercent; // ���������� �����
            private double numberOfOldChildren; //���������� ����� c����� 18 ���������� �����������
            private double taxDeduction; // ��������� ����� 
            private double lnor1; // ������ �� ������ ������
            private double lnor2; // ������ �� ������ ������ �� 2 �����
            private double creditPayment; // ��������� �����
            private double pensionFund;// ���������� ����
            private double unionDues; //����������� ������

            public SalaryCalculator(double[] initialValues)
            {
                if (initialValues.Length != 12) 
                {
                    throw new ArgumentException("Array length should be 11.", nameof(initialValues));
                }

                salary = initialValues[0];
                numberOfChildren = initialValues[1];
                standardTaxDeduction = initialValues[2];
                standardTaxDeductionCount = initialValues[3];
                incomeTaxPercent = initialValues[4];
                numberOfOldChildren = initialValues[5];
                taxDeduction = initialValues[6];
                lnor1 = initialValues[7];
                lnor2 = initialValues[8];
                creditPayment = initialValues[9];
                pensionFund = initialValues[10];
                unionDues = initialValues[11];
            }

            public double CalculateSalary(TextBox expelledTextBox, TextBox incomeTaxTextBox)
            {
                double incomeTax; // �������� ���������� ����� (��������)
                double expelled; //���������
                double tuitionAmount = taxDeduction * numberOfOldChildren;//����� ��������

                // ���������� �������� � �������
                if (salary < standardTaxDeduction)
                {
                    double lnor = (numberOfChildren > 1) ? lnor2 : lnor1;
                    incomeTax = (salary - standardTaxDeductionCount - tuitionAmount - lnor * numberOfChildren - creditPayment ) * incomeTaxPercent / 100;
                }
                else
                {
                    double lnor = (numberOfChildren > 1) ? lnor2 : lnor1;
                    incomeTax = (salary - tuitionAmount - lnor * numberOfChildren - creditPayment ) * incomeTaxPercent / 100;
                }

                expelled = incomeTax + salary * (pensionFund /100) + salary * (unionDues/100);
                accrued = salary - expelled;

                expelledTextBox.Text = Convert.ToString(expelled);
                incomeTaxTextBox.Text = Convert.ToString(incomeTax);

                return accrued;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
