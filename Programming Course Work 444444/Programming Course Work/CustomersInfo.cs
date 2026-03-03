using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Programming_Course_Work
{// Class name Customer Information created
    class CustomersInfo
    {
        // Customer Infomantion Field Name 
        int customernumber;
        string customername;
        string address;
        string infodisplay;

        // Defualt  Parameterless  Constructor
        public CustomersInfo()
        {
            customernumber = 0;
            customername = "";
            address = "";
        }
        //Paramater Constructor 
        public CustomersInfo(int customernum, string nam, string add)
        {
            customernumber = customernum;
            customername = nam;
            address = add;
        }
        //Properties For Information
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
        //  Making a Method to Display in Main
        public void CustomerDisplay()
        {
            StreamWriter In;

            try
            {
                In = File.AppendText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Customer_Information.txt");

                Console.WriteLine("please enter  customers number");
                customernumber = int.Parse(Console.ReadLine());
                Console.WriteLine("please enter customers name");
                customername = Console.ReadLine();
                Console.WriteLine("please enter  there address");
                address = Console.ReadLine();
                In.WriteLine(customernumber);
                In.WriteLine(customername);
                In.WriteLine(address);
                In.WriteLine("CUstomer number :{0} Customer name :{1} Customer address :{2}", customernumber, customername, address);
                In.Close();
            }
            catch
            {
                Console.WriteLine(" file not writtten ");
            }

            Console.ReadLine();

            try
            {
                StreamReader Fr;
                Fr = File.OpenText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Customer_Information.txt");

                infodisplay = Fr.ReadLine();
                while (infodisplay != null)
                {
                    Console.WriteLine(infodisplay);
                    infodisplay = Fr.ReadLine();
                }

                Fr.Close();
            }
            catch
            {
                Console.WriteLine(" file not read ");
            }
        }
        
        public virtual void InfoDisplay()
        {
            Console.WriteLine("This Water Commission  Enter Your  Personal Info");

        }

    }
}




