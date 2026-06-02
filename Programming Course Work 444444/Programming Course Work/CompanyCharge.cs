using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Derived class (inherits Customer_Acc → CustomersInfo): adds tariff fields and GCT rate.
    // Calculates total usage, service charge, and holds all the charge components
    // that Bills uses to produce the final invoice.
    class CompanyCharge : Customer_Acc
    {
        // --- Fields ---
        int water_usage;         // fixed water usage tariff amount ($)
        int sewage_usage;        // fixed sewage usage tariff amount ($)
        double service_charge;   // computed: total_usage + (consumption × $200)
        int customer_charge;     // flat per-customer levy
        int total_usage;         // water_usage + sewage_usage (computed in TOTALUSAGE)
        double gct;              // General Consumption Tax rate (default 16.5%)
        string TotalUsagedisplay; // temporary buffer for file read-back
        string servicedisplay;    // temporary buffer for file read-back

        // Portable output directory shared by all classes in this project.
        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default constructor — sets standard tariff defaults.
        // Water and sewage usage default to $289 each; GCT defaults to 16.5%.
        public CompanyCharge()
        {
            water_usage = 289;
            sewage_usage = 289;
            service_charge = 0;
            customer_charge = 0;
            total_usage = 0;
            gct = 16.5;
        }

        // Full constructor — wires all charge fields plus the inherited meter fields.
        public CompanyCharge(int current, int previous, int consumption,
                             int water, int sewage, int service, int customer_charg, int total, double g)
            : base(current, previous, consumption)
        {
            water_usage = water;
            sewage_usage = sewage;
            service_charge = service;
            customer_charge = customer_charg;
            total_usage = total;
            gct = g;
        }

        // --- Properties ---

        // Fixed water tariff amount. Defaults to $289 per period.
        public int WATER_USAGE
        {
            get { return water_usage; }
            set { water_usage = value; }
        }

        // Fixed sewage tariff amount. Defaults to $289 per period.
        public int SEWAGE_USAGE
        {
            get { return sewage_usage; }
            set { sewage_usage = value; }
        }

        // Computed service charge: total_usage + (consumption × $200).
        public double SERVICE_CHARGE
        {
            get { return service_charge; }
            set { service_charge = value; }
        }

        // Flat per-customer levy added to the subtotal.
        public int CUSTOMER_CHARGE
        {
            get { return customer_charge; }
            set { customer_charge = value; }
        }

        // Sum of water_usage + sewage_usage (computed by TOTALUSAGE).
        public int TOTAL_USAGE
        {
            get { return total_usage; }
            set { total_usage = value; }
        }

        // General Consumption Tax percentage. Default 16.5%. Applied in Bills.CalculateTotal().
        public double GCT
        {
            get { return gct; }
            set { gct = value; }
        }

        // Returns the four charge fields as a newline-separated string.
        public string Company()
        {
            return water_usage + "\n" + sewage_usage + "\n" + service_charge + "\n" + customer_charge;
        }

        // Computes total_usage = water + sewage, appends it to Company_Charge.txt,
        // then reads the file back to confirm the record. Called by menu option 3.
        public void TOTALUSAGE()
        {
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

        // Computes SERVICE_CHARGE = total_usage + (consumption × $200),
        // appends the result to Company_Charge.txt, then reads the file back.
        // Must be called AFTER TOTALUSAGE() so total_usage is populated.
        public void SERVICECHARGE()
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                StreamWriter Sc = File.AppendText(Path.Combine(DataDir, "Company_Charge.txt"));
                SERVICE_CHARGE = TOTAL_USAGE + (CURRENT_CONSUMPTION * 200);
                Sc.WriteLine("Service charge: {0}", service_charge);
                Sc.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not written: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

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
