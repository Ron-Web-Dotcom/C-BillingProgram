using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{

    // Class name Customer Account inhert from Customer Information
    class Customer_Acc : CustomersInfo
    {
        // 
        int current_meter;
        int previous_meter;
        int current_consumption;
        string Accdisplay;

        // Defualt  Parameterless  Constructor
        public Customer_Acc()
        {
            current_meter = 0;
            previous_meter = 0;
            current_consumption = 0;
        }
        //Parameter Constructor 
        public Customer_Acc(int cur, int prev, int consume)
        {
            current_meter = cur;
            previous_meter = prev;
            current_consumption = consume;
        }
        //Parameter Constructor that include a base class and a dervied class the  base class is CUSTOMER IFORMATION and dervied class is CUSTOMER ACCOUNT
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
        //  Making a Method to Display in Main
        public void CustomersAccount()
        {
            StreamWriter Ac;

            try
            {
                Ac = File.AppendText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Customer_Account.txt");

                Console.WriteLine("please enter customers previous reading");
                previous_meter = int.Parse(Console.ReadLine());
                Console.WriteLine("please enter customers current reading");
                current_meter = int.Parse(Console.ReadLine());
                current_consumption = current_meter - previous_meter;
                Ac.WriteLine(previous_meter);
                Ac.WriteLine(current_meter);
                Ac.WriteLine(current_consumption);
                Ac.WriteLine("Previous meter :{0} Current meter: {1} Current consumption :{2}", previous_meter, current_meter, current_consumption);
                Ac.Close();
            }
            catch
            {
                Console.WriteLine(" file not writtten ");

            }
            Console.ReadLine();

            try
            {
                StreamReader Bw;
                Bw = File.OpenText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Customer_Account.txt");

                Accdisplay = Bw.ReadLine();
                while (Accdisplay != null)
                {
                    Console.WriteLine(Accdisplay);
                    Accdisplay = Bw.ReadLine();
                }

                Bw.Close();
            }
            catch
            {
                Console.WriteLine(" file not read ");
            }
        }
             
        

        public override void InfoDisplay()
        {
            Console.WriteLine("Your Final Display is");
            Console.ReadLine();
        }

    

    }
}



