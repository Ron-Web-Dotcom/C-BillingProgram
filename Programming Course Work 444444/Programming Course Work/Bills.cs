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

        // ---------------------------------------------------------------
        // Feature: Full single-call constructor
        // Wires all three composition objects together and calculates the
        // complete bill (including GCT) in one step.
        // ---------------------------------------------------------------
        public Bills(int customernum, string name, string address,
                     int previous, int current,
                     int water, int sewage, int customerCharge)
        {
            Info  = new CustomersInfo(customernum, name, address);

            int consumption = current - previous;
            Accnt = new Customer_Acc(current, previous, consumption);

            Charge = new CompanyCharge();
            Charge.CURRENT_METER       = current;
            Charge.PREVIOUS_METER      = previous;
            Charge.CURRENT_CONSUMPTION = consumption;
            Charge.WATER_USAGE         = water;
            Charge.SEWAGE_USAGE        = sewage;
            Charge.CUSTOMER_CHARGE     = customerCharge;
            Charge.TOTAL_USAGE         = water + sewage;
            Charge.SERVICE_CHARGE      = Charge.TOTAL_USAGE + (consumption * 200);

            CalculateTotal();
        }

        // ---------------------------------------------------------------
        // Feature: CalculateTotal — applies GCT to produce grand total
        // GCT (16.5%) was stored on CompanyCharge but never used anywhere.
        // ---------------------------------------------------------------
        public void CalculateTotal()
        {
            if (Charge == null) return;
            double subtotal  = Charge.SERVICE_CHARGE + Charge.CUSTOMER_CHARGE;
            double gctAmount = subtotal * (Charge.GCT / 100.0);
            total_charges    = subtotal + gctAmount;
        }

        // ---------------------------------------------------------------
        // Feature: PrintFormattedBill — formatted bill to the console
        // ---------------------------------------------------------------
        public void PrintFormattedBill()
        {
            string sep = new string('-', 46);
            Console.WriteLine(sep);
            Console.WriteLine("          WATER COMMISSION BILL");
            Console.WriteLine(sep);

            if (Info != null)
            {
                Console.WriteLine("  Customer # : " + Info.CUSTOMERNUMBER);
                Console.WriteLine("  Name       : " + Info.CUSTOMERNAME);
                Console.WriteLine("  Address    : " + Info.ADDRESS);
                Console.WriteLine(sep);
            }

            if (Accnt != null)
            {
                Console.WriteLine("  Previous Reading  : " + Accnt.PREVIOUS_METER);
                Console.WriteLine("  Current Reading   : " + Accnt.CURRENT_METER);
                Console.WriteLine("  Consumption       : " + Accnt.CURRENT_CONSUMPTION + " units");
                Console.WriteLine(sep);
            }

            if (Charge != null)
            {
                double consumptionCharge = Charge.CURRENT_CONSUMPTION * 200.0;
                double subtotal          = Charge.SERVICE_CHARGE + Charge.CUSTOMER_CHARGE;
                double gctAmount         = subtotal * (Charge.GCT / 100.0);

                Console.WriteLine("  Water Usage        : $" + Charge.WATER_USAGE.ToString("0.00"));
                Console.WriteLine("  Sewage Charge      : $" + Charge.SEWAGE_USAGE.ToString("0.00"));
                Console.WriteLine("  Consumption Charge : $" + consumptionCharge.ToString("0.00")
                                  + "  (" + Accnt.CURRENT_CONSUMPTION + " units x $200)");
                Console.WriteLine("  Service Charge     : $" + Charge.SERVICE_CHARGE.ToString("0.00"));
                Console.WriteLine("  Customer Charge    : $" + Charge.CUSTOMER_CHARGE.ToString("0.00"));
                Console.WriteLine("  Sub-Total          : $" + subtotal.ToString("0.00"));
                Console.WriteLine(string.Format("  GCT ({0}%)         : ${1}",
                                  Charge.GCT, gctAmount.ToString("0.00")));
            }

            Console.WriteLine(sep);
            Console.WriteLine("  TOTAL DUE          : $" + total_charges.ToString("0.00"));
            Console.WriteLine(sep);
        }

        // ---------------------------------------------------------------
        // Feature: SaveBill — writes a formatted bill to a timestamped file
        // ---------------------------------------------------------------
        public void SaveBill()
        {
            try
            {
                Directory.CreateDirectory(DataDir);
                string customerPart = Info != null ? Info.CUSTOMERNUMBER.ToString() : "unknown";
                string timestamp    = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string path         = Path.Combine(DataDir,
                                          "Bill_Customer" + customerPart + "_" + timestamp + ".txt");

                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    string sep = new string('-', 40);
                    sw.WriteLine("WATER COMMISSION BILL");
                    sw.WriteLine(sep);

                    if (Info != null)
                    {
                        sw.WriteLine("Customer # : " + Info.CUSTOMERNUMBER);
                        sw.WriteLine("Name       : " + Info.CUSTOMERNAME);
                        sw.WriteLine("Address    : " + Info.ADDRESS);
                        sw.WriteLine(sep);
                    }

                    if (Accnt != null)
                    {
                        sw.WriteLine("Previous Reading  : " + Accnt.PREVIOUS_METER);
                        sw.WriteLine("Current Reading   : " + Accnt.CURRENT_METER);
                        sw.WriteLine("Consumption       : " + Accnt.CURRENT_CONSUMPTION + " units");
                        sw.WriteLine(sep);
                    }

                    if (Charge != null)
                    {
                        double consumptionCharge = Charge.CURRENT_CONSUMPTION * 200.0;
                        double subtotal          = Charge.SERVICE_CHARGE + Charge.CUSTOMER_CHARGE;
                        double gctAmount         = subtotal * (Charge.GCT / 100.0);

                        sw.WriteLine("Water Usage        : $" + Charge.WATER_USAGE.ToString("0.00"));
                        sw.WriteLine("Sewage Charge      : $" + Charge.SEWAGE_USAGE.ToString("0.00"));
                        sw.WriteLine("Consumption Charge : $" + consumptionCharge.ToString("0.00")
                                     + "  (" + Accnt.CURRENT_CONSUMPTION + " units x $200)");
                        sw.WriteLine("Service Charge     : $" + Charge.SERVICE_CHARGE.ToString("0.00"));
                        sw.WriteLine("Customer Charge    : $" + Charge.CUSTOMER_CHARGE.ToString("0.00"));
                        sw.WriteLine("Sub-Total          : $" + subtotal.ToString("0.00"));
                        sw.WriteLine(string.Format("GCT ({0}%)         : ${1}",
                                     Charge.GCT, gctAmount.ToString("0.00")));
                    }

                    sw.WriteLine(sep);
                    sw.WriteLine("TOTAL DUE          : $" + total_charges.ToString("0.00"));
                    sw.WriteLine(sep);
                    sw.WriteLine("Generated: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                }

                Console.WriteLine("Bill saved to: " + path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save bill: " + ex.Message);
            }
        }
    }
}
