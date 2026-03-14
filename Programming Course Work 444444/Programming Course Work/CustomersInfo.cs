using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Class name Customer Information created
    class CustomersInfo
    {
        // Customer Information Fields
        int customernumber;
        string customername;
        string address;
        string infodisplay;

        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default Parameterless Constructor
        public CustomersInfo()
        {
            customernumber = 0;
            customername = "";
            address = "";
        }

        // Parameter Constructor
        public CustomersInfo(int customernum, string nam, string add)
        {
            customernumber = customernum;
            customername = nam;
            address = add;
        }

        // Properties
        public int CUSTOMERNUMBER
        {
            get { return customernumber; }
            set { customernumber = value; }
        }

        public string CUSTOMERNAME
        {
            get { return customername; }
            set { customername = value; }
        }

        public string ADDRESS
        {
            get { return address; }
            set { address = value; }
        }

        public string CustomerInfo01()
        {
            return customernumber + "\n" + customername + "\n" + address;
        }

        // ---------------------------------------------------------------
        // Feature: validated integer input — loops until user enters a
        // valid non-negative whole number, preventing FormatException crashes
        // ---------------------------------------------------------------
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

        // Method to Display in Main
        // Fix: user input is now read before opening the file, preventing file handle leaks
        //      on bad input and giving an accurate error message if the write fails
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
                // Fix: was "CUstomer number" — corrected capitalisation
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

        public virtual void InfoDisplay()
        {
            Console.WriteLine("This Water Commission  Enter Your  Personal Info");
        }
    }
}
