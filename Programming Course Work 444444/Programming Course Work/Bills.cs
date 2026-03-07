using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Bills Class Created
    class Bills
    {
        // Fields
        double total_charges;
        string totalchargesdisplay;

        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default Parameterless Constructor
        public Bills()
        {
            total_charges = 0;
        }

        // Parameter Constructor
        public Bills(int totalcharge)
        {
            total_charges = totalcharge;
        }

        // Property of Bills
        public double TOTAL_CHARGE
        {
            get { return total_charges; }
            set { total_charges = value; }
        }

        // Composition creating method for Information
        public CustomersInfo Info;

        public Bills(int customernum, string nam, string add)
        {
            Info = new CustomersInfo(customernum, nam, add);
        }

        public string Bill01()
        {
            if (Info == null)
                return "Customer information not initialised.";
            return Info.CustomerInfo01();
        }

        // Composition creating method for Account
        public Customer_Acc Accnt;

        // Fix: was passing (previous, current) but Customer_Acc expects (cur, prev) — swapped
        public Bills(int previous, int current, int consumption)
        {
            Accnt = new Customer_Acc(current, previous, consumption);
        }

        public string Bills02()
        {
            if (Accnt == null)
                return "Customer account not initialised.";
            return Accnt.Account_info();
        }

        // Composition creating method for Company Charge
        public CompanyCharge Charge;

        public Bills(int water, int sewage, int service, int customer_charg)
        {
            Charge = new CompanyCharge();
            Charge.WATER_USAGE = water;
            Charge.SEWAGE_USAGE = sewage;
            Charge.SERVICE_CHARGE = service;
            Charge.CUSTOMER_CHARGE = customer_charg;
        }

        public void Bills03()
        {
            if (Charge == null)
            {
                Console.WriteLine("Company charge not initialised.");
                return;
            }

            // Write the bills text
            try
            {
                Directory.CreateDirectory(DataDir);
                StreamWriter yc = File.AppendText(Path.Combine(DataDir, "Bills.txt"));
                yc.WriteLine("Total charges: {0}", total_charges);
                yc.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not written: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            // Read the bills text
            try
            {
                StreamReader OO = File.OpenText(Path.Combine(DataDir, "Bills.txt"));
                while ((totalchargesdisplay = OO.ReadLine()) != null)
                {
                    Console.WriteLine(totalchargesdisplay);
                }
                OO.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not read: " + ex.Message);
            }
        }
    }
}
