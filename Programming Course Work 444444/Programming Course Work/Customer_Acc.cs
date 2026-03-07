using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{
    // Class name Customer Account inherits from Customer Information
    class Customer_Acc : CustomersInfo
    {
        int current_meter;
        int previous_meter;
        int current_consumption;
        string Accdisplay;

        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        // Default Parameterless Constructor
        public Customer_Acc()
        {
            current_meter = 0;
            previous_meter = 0;
            current_consumption = 0;
        }

        // Parameter Constructor
        public Customer_Acc(int cur, int prev, int consume)
        {
            current_meter = cur;
            previous_meter = prev;
            current_consumption = consume;
        }

        // Parameter Constructor including base class (Customer Information) and derived class (Customer Account)
        public Customer_Acc(int customernum, string nam, string add, int current, int previous, int consumption)
            : base(customernum, nam, add)
        {
            current_meter = current;
            previous_meter = previous;
            current_consumption = consumption;
        }

        // Properties of Customer Account
        public int CURRENT_METER
        {
            get { return current_meter; }
            set { current_meter = value; }
        }

        public int PREVIOUS_METER
        {
            get { return previous_meter; }
            set { previous_meter = value; }
        }

        public int CURRENT_CONSUMPTION
        {
            get { return current_consumption; }
            set { current_consumption = value; }
        }

        public string Account_info()
        {
            return current_meter + "\n" + previous_meter + "\n" + current_consumption;
        }

        // Method to Display in Main
        // Fix: user input is now read before opening the file, preventing file handle leaks
        //      on bad input and giving an accurate error message if the write fails
        public void CustomersAccount()
        {
            Console.WriteLine("Please enter customers previous reading");
            previous_meter = int.Parse(Console.ReadLine());
            Console.WriteLine("Please enter customers current reading");
            current_meter = int.Parse(Console.ReadLine());
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

        public override void InfoDisplay()
        {
            Console.WriteLine("Your Final Display is");
            Console.ReadLine();
        }
    }
}
