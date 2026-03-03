using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Programming_Course_Work
{ //Bills Class Created
    class Bills
    {
        //Field 
        double total_charges;
        string totalchargesdisplay;

        // Defualt  Parameterless  Constructor
        public Bills()
        {
            total_charges = 0;
        }
        //Parameter Constructor
        public Bills(int totalcharge)
        {
            total_charges = totalcharge;
        }
        //Property of Bills
        public double TOTAL_CHARGE
        {
            get { return total_charges; }
            set { total_charges = value; }
        }


        // Composition creatig method for Information
        public CustomersInfo Info;

        public Bills(int customernum, string nam, string add)
        {
            Info = new CustomersInfo(customernum, nam, add);
        }

        public string Bill01()
        {
            return Info.CustomerInfo01();
        }
        // Composition creating method for Account

        public Customer_Acc Accnt;


        public Bills(int previous, int current, int consumption)
        {
            Accnt = new Customer_Acc(previous, current, consumption);

        }
        public string Bills02()
        {
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


            // When im writing in the bills text 
            StreamWriter yc;

            try
            {
                yc = File.AppendText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Bills.txt");
                

                yc.WriteLine("Total charges: {0}", total_charges);
                yc.Close();
            }
            catch
            {
                Console.WriteLine(" file not writtten ");
            }
            Console.ReadLine();

            // When im reading in the bills text
            try
            {
                StreamReader OO;
                OO = File.OpenText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Bills.txt");

                while ((totalchargesdisplay = OO.ReadLine()) != null)
                {
                    Console.WriteLine(totalchargesdisplay);
                }

                OO.Close();
            }
            catch
            {
                Console.WriteLine(" file not read ");
            }
        }

        }

    }




    


           


    

