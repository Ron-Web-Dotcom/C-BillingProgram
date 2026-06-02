using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Composition class: ties together CustomersInfo, Customer_Acc, and CompanyCharge
    // to produce a complete bill. Calculates the grand total (including GCT),
    // prints a formatted bill to the console, and saves it to a timestamped file.
    class Bills
    {
        // --- Fields ---
        double total_charges;        // grand total after GCT is applied
        string totalchargesdisplay;  // temporary buffer for reading Bills.txt back to console

        // Portable output directory shared by all classes in this project.
        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default constructor — zeroes the total.
        public Bills()
        {
            total_charges = 0;
        }

        // Constructor for a total-only record (no composition objects populated).
        public Bills(int totalcharge)
        {
            total_charges = totalcharge;
        }

        // --- Property ---

        // Grand total due after GCT. Set by CalculateTotal().
        public double TOTAL_CHARGE
        {
            get { return total_charges; }
            set { total_charges = value; }
        }

        // --- Composition: customer identity ---

        // Holds the customer's number, name, and address.
        public CustomersInfo Info;

        // Constructor that populates the Info composition object only.
        public Bills(int customernum, string nam, string add)
        {
            Info = new CustomersInfo(customernum, nam, add);
        }

        // Returns customer identity as a newline-separated string, or an error message
        // if the Info object has not been initialised.
        public string Bill01()
        {
            if (Info == null)
                return "Customer information not initialised.";
            return Info.CustomerInfo01();
        }

        // --- Composition: meter readings ---

        // Holds previous/current readings and computed consumption.
        public Customer_Acc Accnt;

        // Constructor that populates the Accnt composition object only.
        // Parameters are (previous, current, consumption); Customer_Acc expects (current, previous, consumption).
        public Bills(int previous, int current, int consumption)
        {
            Accnt = new Customer_Acc(current, previous, consumption);
        }

        // Returns meter readings as a newline-separated string, or an error message
        // if the Accnt object has not been initialised.
        public string Bills02()
        {
            if (Accnt == null)
                return "Customer account not initialised.";
            return Accnt.Account_info();
        }

        // --- Composition: company charges ---

        // Holds tariff fields (water, sewage, service charge, customer charge, GCT).
        public CompanyCharge Charge;

        // Constructor that populates the Charge composition object only.
        public Bills(int water, int sewage, int service, int customer_charg)
        {
            Charge = new CompanyCharge();
            Charge.WATER_USAGE = water;
            Charge.SEWAGE_USAGE = sewage;
            Charge.SERVICE_CHARGE = service;
            Charge.CUSTOMER_CHARGE = customer_charg;
        }

        // Appends total_charges to Bills.txt and reads the file back to the console.
        // Guards against an uninitialised Charge object before writing.
        public void Bills03()
        {
            if (Charge == null)
            {
                Console.WriteLine("Company charge not initialised.");
                return;
            }

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

        // Full single-call constructor: wires all three composition objects and calculates
        // the complete bill (service charge + customer charge + GCT) in one step.
        // Used by GenerateFullBill() in Program.cs (menu option 4).
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

        // Applies GCT to produce the grand total.
        // Formula: subtotal = SERVICE_CHARGE + CUSTOMER_CHARGE
        //          GCT amount = subtotal × (GCT% / 100)
        //          total_charges = subtotal + GCT amount
        public void CalculateTotal()
        {
            if (Charge == null) return;
            double subtotal  = Charge.SERVICE_CHARGE + Charge.CUSTOMER_CHARGE;
            double gctAmount = subtotal * (Charge.GCT / 100.0);
            total_charges    = subtotal + gctAmount;
        }

        // Prints a formatted bill to the console with separator lines and labelled rows.
        // Requires Info, Accnt, and Charge to all be populated for a complete display.
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

        // Writes a formatted bill to a timestamped file: Bill_Customer{N}_{yyyyMMdd_HHmmss}.txt
        // Uses a 'using' block to guarantee the StreamWriter is closed even if an exception is thrown.
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
