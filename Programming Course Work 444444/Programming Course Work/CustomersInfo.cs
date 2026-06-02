using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Base class: stores and persists customer identity details (number, name, address).
    // Inherited by Customer_Acc and, transitively, by CompanyCharge.
    class CustomersInfo
    {
        // --- Fields ---
        int customernumber;   // unique identifier assigned by the water commission
        string customername;  // full name of the account holder
        string address;       // service address
        string infodisplay;   // temporary buffer used when reading back file lines

        // Portable path to the shared output folder on the current user's Desktop.
        // All classes in this project write to the same directory.
        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default constructor — initialises fields to safe empty values.
        public CustomersInfo()
        {
            customernumber = 0;
            customername = "";
            address = "";
        }

        // Convenience constructor used by Bills (composition) and test scenarios.
        public CustomersInfo(int customernum, string nam, string add)
        {
            customernumber = customernum;
            customername = nam;
            address = add;
        }

        // --- Properties ---

        // Unique customer account number (non-negative integer).
        public int CUSTOMERNUMBER
        {
            get { return customernumber; }
            set { customernumber = value; }
        }

        // Full name of the account holder.
        public string CUSTOMERNAME
        {
            get { return customername; }
            set { customername = value; }
        }

        // Service / billing address.
        public string ADDRESS
        {
            get { return address; }
            set { address = value; }
        }

        // Returns the three identity fields as a newline-separated string (used by Bills).
        public string CustomerInfo01()
        {
            return customernumber + "\n" + customername + "\n" + address;
        }

        // Loops until the user enters a valid non-negative integer.
        // Prevents FormatException crashes that the original int.Parse calls caused.
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

        // Collects customer details from the console, appends them to Customer_Information.txt,
        // then reads the whole file back to the console so the user can confirm the record.
        // Input is collected BEFORE the file is opened to avoid leaking a file handle on bad input.
        public void CustomerDisplay()
        {
            customernumber = ReadInt("Please enter customer number  : ");
            Console.Write(          "Please enter customer name    : ");
            customername = Console.ReadLine();
            Console.Write(          "Please enter customer address : ");
            address = Console.ReadLine();

            try
            {
                Directory.CreateDirectory(DataDir);
                StreamWriter In = File.AppendText(Path.Combine(DataDir, "Customer_Information.txt"));
                In.WriteLine(customernumber);
                In.WriteLine(customername);
                In.WriteLine(address);
                In.WriteLine("Customer number: {0}  Customer name: {1}  Customer address: {2}",
                    customernumber, customername, address);
                In.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not written: " + ex.Message);
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

            try
            {
                StreamReader Fr = File.OpenText(Path.Combine(DataDir, "Customer_Information.txt"));
                infodisplay = Fr.ReadLine();
                while (infodisplay != null)
                {
                    Console.WriteLine(infodisplay);
                    infodisplay = Fr.ReadLine();
                }
                Fr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("File not read: " + ex.Message);
            }
        }

        // Virtual hook so derived classes can override the welcome / display message.
        public virtual void InfoDisplay()
        {
            Console.WriteLine("This Water Commission  Enter Your  Personal Info");
        }
    }
}
