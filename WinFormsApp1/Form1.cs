using System;
using System.Windows.Forms;
using System.Reflection;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private SalaryCalculator salaryCalculator;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double salary = double.Parse(textBox1.Text);
            double numberOfChildren = double.Parse(textBox2.Text);
            double standardTaxDeduction = double.Parse(textBox3.Text);

            double[] calculatedSalary = { salary, numberOfChildren, 10000, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
            SalaryCalculator calculator = new SalaryCalculator(calculatedSalary);
            MessageBox.Show($"������ ����������� ��������: {calculator.CalculateSalary()}");
        }

        public class SalaryCalculator
        {
            private double salary; //��
            private double accrued; //���������
            private double numberOfChildren; //���������� ����� 
            private double standardTaxDeduction; // ����������� ��������� ����� (�� ����� �����)
            private double standardTaxDeductionCount;// ����������� ��������� ����� (��� ��������)
            private double incomeTax; // ���������� �����
            private double incomeTaxPercent; // ���������� �����
            private double expelled; //���������
            private double numberOfOldChildren; //���������� ����� c����� 18 ���������� �����������
            private double taxDeduction; // ��������� ����� 
            private double lnor1; // ������ �� ������ ������
            private double lnor2; // ������ �� ������ ������ �� 2 �����
            private double creditPayment; // ��������� �����
            private double tuitionAmount; //����� ��������
            private double pensionFund;// ���������� ����
            private double unionDues; //����������� ������

            public SalaryCalculator(double[] initialValues)
            {
                if (initialValues.Length != 15) 
                {
                    throw new ArgumentException("Array length should be 15.", nameof(initialValues));
                }

                salary = initialValues[0];
                numberOfChildren = initialValues[1];
                standardTaxDeduction = initialValues[2];
                standardTaxDeductionCount = initialValues[3];
                incomeTax = initialValues[4];
                incomeTaxPercent = initialValues[5];
                expelled = initialValues[6];
                numberOfOldChildren = initialValues[7];
                taxDeduction = initialValues[8];
                lnor1 = initialValues[9];
                lnor2 = initialValues[10];
                creditPayment = initialValues[11];
                tuitionAmount = initialValues[12];
                pensionFund = initialValues[13];
                unionDues = initialValues[14];
            }

            public double CalculateSalary()
            {
                // ���������� �������� � �������
                if (salary < standardTaxDeduction)
                {
                    double lnor = (numberOfChildren > 1) ? lnor2 : lnor1;
                    incomeTax = (salary - standardTaxDeduction - taxDeduction * numberOfOldChildren - lnor * numberOfChildren - creditPayment - tuitionAmount) * incomeTaxPercent / 100;
                }
                else
                {
                    double lnor = (numberOfChildren > 1) ? lnor2 : lnor1;
                    incomeTax = (salary - taxDeduction * numberOfOldChildren - lnor * numberOfChildren - creditPayment - tuitionAmount) * incomeTaxPercent / 100;
                }

                expelled = incomeTax + salary * pensionFund + salary * unionDues;
                accrued = salary - expelled;

                return accrued;
            }
        }
    }
}
