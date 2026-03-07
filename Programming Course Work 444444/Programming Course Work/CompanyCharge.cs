using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Class name Company Charge inherits from Customer Account
    class CompanyCharge : Customer_Acc
    {
        // Company Charge Fields
        int water_usage;
        int sewage_usage;
        double service_charge;
        int customer_charge;
        int total_usage;
        double gct;
        string TotalUsagedisplay;
        string servicedisplay;

        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default Parameterless Constructor
        public CompanyCharge()
        {
            water_usage = 289;
            sewage_usage = 289;
            service_charge = 0;
            customer_charge = 0;
            total_usage = 0;
            gct = 16.5;
        }

        // Parameter Constructor including base class (Customer Account) and derived class (Company Charge)
        // Fix: was 'pervious' — corrected to 'previous'
        public CompanyCharge(int current, int previous, int consumption, int water, int sewage, int service, int customer_charg, int total, double g)
            : base(current, previous, consumption)
        {
            water_usage = water;
            sewage_usage = sewage;
            service_charge = service;
            customer_charge = customer_charg;
            total_usage = total;
            gct = g;
        }

        public int WATER_USAGE
        {
            get { return water_usage; }
            set { water_usage = value; }
        }

        public int SEWAGE_USAGE
        {
            get { return sewage_usage; }
            set { sewage_usage = value; }
        }

        public double SERVICE_CHARGE
        {
            get { return service_charge; }
            set { service_charge = value; }
        }

        public int CUSTOMER_CHARGE
        {
            get { return customer_charge; }
            set { customer_charge = value; }
        }

        public int TOTAL_USAGE
        {
            get { return total_usage; }
            set { total_usage = value; }
        }

        public double GCT
        {
            get { return gct; }
            set { gct = value; }
        }

        public string Company()
        {
            return water_usage + "\n" + sewage_usage + "\n" + service_charge + "\n" + customer_charge;
        }

        public void TOTALUSAGE()
        {
            // Write the total usage
            try
            {
                Directory.CreateDirectory(DataDir);
                StreamWriter Bk = File.AppendText(Path.Combine(DataDir, "Company_Charge.txt"));
                TOTAL_USAGE = WATER_USAGE + SEWAGE_USAGE;
                Bk.WriteLine("Total usage: {0}", total_usage);
                Bk.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not written: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            // Read the total usage
            try
            {
                StreamReader Bo = File.OpenText(Path.Combine(DataDir, "Company_Charge.txt"));
                TotalUsagedisplay = Bo.ReadLine();
                while (TotalUsagedisplay != null)
                {
                    Console.WriteLine(TotalUsagedisplay);
                    TotalUsagedisplay = Bo.ReadLine();
                }
                Bo.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not read: " + ex.Message);
            }
        }

        public void SERVICECHARGE()
        {
            // Write the service charge
            try
            {
                Directory.CreateDirectory(DataDir);
                StreamWriter Sc = File.AppendText(Path.Combine(DataDir, "Company_Charge.txt"));
                SERVICE_CHARGE = TOTAL_USAGE + (CURRENT_CONSUMPTION * 200);
                // Fix: removed duplicate raw write; fixed format string spacing
                Sc.WriteLine("Service charge: {0}", service_charge);
                Sc.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not written: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            // Read the service charge
            try
            {
                StreamReader Ta = File.OpenText(Path.Combine(DataDir, "Company_Charge.txt"));
                servicedisplay = Ta.ReadLine();
                while (servicedisplay != null)
                {
                    Console.WriteLine(servicedisplay);
                    servicedisplay = Ta.ReadLine();
                }
                Ta.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not read: " + ex.Message);
            }
        }
    }
}
