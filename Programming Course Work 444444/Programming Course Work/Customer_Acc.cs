using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Derived class (inherits CustomersInfo): adds meter readings and consumption.
    // Provides the standalone meter-entry workflow used by menu option 2.
    class Customer_Acc : CustomersInfo
    {
        // --- Fields ---
        int current_meter;      // most recent meter reading (units)
        int previous_meter;     // meter reading from the prior billing period (units)
        int current_consumption; // units consumed = current - previous
        string Accdisplay;      // temporary buffer for reading file lines back to console

        // Portable output directory shared by all classes in this project.
        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default constructor — initialises all meter fields to zero.
        public Customer_Acc()
        {
            current_meter = 0;
            previous_meter = 0;
            current_consumption = 0;
        }

        // Constructor for meter values only (no customer identity).
        public Customer_Acc(int cur, int prev, int consume)
        {
            current_meter = cur;
            previous_meter = prev;
            current_consumption = consume;
        }

        // Full constructor — sets both inherited identity fields and meter fields.
        // Used when constructing a complete account record in one step.
        public Customer_Acc(int customernum, string nam, string add, int current, int previous, int consumption)
            : base(customernum, nam, add)
        {
            current_meter = current;
            previous_meter = previous;
            current_consumption = consumption;
        }

        // --- Properties ---

        // Most recent meter reading in units.
        public int CURRENT_METER
        {
            get { return current_meter; }
            set { current_meter = value; }
        }

        // Meter reading from the previous billing period.
        public int PREVIOUS_METER
        {
            get { return previous_meter; }
            set { previous_meter = value; }
        }

        // Units consumed this period (current − previous).
        public int CURRENT_CONSUMPTION
        {
            get { return current_consumption; }
            set { current_consumption = value; }
        }

        // Returns the three meter fields as a newline-separated string (used by Bills).
        public string Account_info()
        {
            return current_meter + "\n" + previous_meter + "\n" + current_consumption;
        }

        // Loops until the user enters a valid non-negative integer.
        // Prevents FormatException crashes that bare int.Parse would cause.
        private static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= 0)
                    return result;
                Console.WriteLine("  Invalid input — please enter a whole number.");
            }
        }

        // Collects meter readings from the console, validates them, appends to Customer_Account.txt,
        // then reads the whole file back so the user can confirm the record.
        // Validation: current reading must be >= previous reading.
        // Input is collected BEFORE the file is opened to avoid handle leaks on bad input.
        public void CustomersAccount()
        {
            previous_meter = ReadInt("Please enter customers previous reading: ");

            // Current reading must be >= previous — loop until a valid value is entered.
            do
            {
                current_meter = ReadInt("Please enter customers current reading : ");
                if (current_meter < previous_meter)
                    Console.WriteLine("  Current reading cannot be less than the previous reading.");
            }
            while (current_meter < previous_meter);

            current_consumption = current_meter - previous_meter;

            try
            {
                Directory.CreateDirectory(DataDir);
                StreamWriter Ac = File.AppendText(Path.Combine(DataDir, "Customer_Account.txt"));
                Ac.WriteLine(previous_meter);
                Ac.WriteLine(current_meter);
                Ac.WriteLine(current_consumption);
                Ac.WriteLine("Previous meter: {0}  Current meter: {1}  Current consumption: {2}",
                    previous_meter, current_meter, current_consumption);
                Ac.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not written: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            try
            {
                StreamReader Bw = File.OpenText(Path.Combine(DataDir, "Customer_Account.txt"));
                Accdisplay = Bw.ReadLine();
                while (Accdisplay != null)
                {
                    Console.WriteLine(Accdisplay);
                    Accdisplay = Bw.ReadLine();
                }
                Bw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not read: " + ex.Message);
            }
        }

        // Overrides the base class display hook for the account view.
        public override void InfoDisplay()
        {
            Console.WriteLine("Your Final Display is");
            Console.ReadLine();
        }
    }
}
