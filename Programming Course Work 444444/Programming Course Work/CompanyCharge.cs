using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Programming_Course_Work
{ // Class name Company Charge inhert from Customer Account
    class CompanyCharge : Customer_Acc
    {
        //Company Charge Field 

        int water_usage;
        int sewage_usage;
        double service_charge;
        int customer_charge;
        int total_usage;
        double gct;
        string TotalUsagedisplay;
        string servicedisplay;
        // Defualt  Parameterless  Constructor
        public CompanyCharge()
        {
            water_usage = 289;
            sewage_usage = 289;
            service_charge = 0;
            customer_charge = 0;
            total_usage = 0;
            gct = 16.5;
        }
        //Parameter Constructor that include a base class and a dervied class 

        public CompanyCharge(int current, int pervious, int consumption, int water, int sewage, int service, int customer_charg, int total, double g)
            : base(current, pervious, consumption)
        {
            water_usage = water;
            sewage_usage = sewage;
            service_charge = service;
            customer_charge = customer_charg;
            total_usage = total;
            gct = g;
        }
        public int WATER_USAGE
        {
            get { return water_usage; }
            set { water_usage = value; }
        }
        public int SEWAGE_USAGE
        {
            get { return sewage_usage; }
            set { sewage_usage = value; }
        }
        public double SERVICE_CHARGE
        {
            get { return service_charge; }
            set { service_charge = value; }
        }
        public int CUSTOMER_CHARGE
        {
            get { return customer_charge; }
            set { customer_charge = value; }
        }

        public int TOTAL_USAGE
        {
            get { return total_usage; }
            set { total_usage = value; }
        }
        public double GCT
        {
            get { return gct; }
            set { gct = value; }
        }

        public string Company()
        {

            return water_usage + "\n" + sewage_usage + "\n" + service_charge + "\n" + customer_charge;
        }

        public void TOTALUSAGE()
        {
            StreamWriter Bk;
            try
            {

                Bk = File.AppendText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Company_Charge.txt");

                TOTAL_USAGE = WATER_USAGE + SEWAGE_USAGE;
                Bk.WriteLine("Total usage: {0}", total_usage);
                Bk.Close();
            }
            catch
            {
                Console.WriteLine(" file not writtten ");

            }
            Console.ReadLine();

            try
            {
                StreamReader Bo;
                Bo = File.OpenText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Company_Charge.txt");

                TotalUsagedisplay = Bo.ReadLine();
                while (TotalUsagedisplay != null)
                {
                    Console.WriteLine(TotalUsagedisplay);
                    TotalUsagedisplay = Bo.ReadLine();
                }

                Bo.Close();
            }
            catch
            {
                Console.WriteLine(" file not read ");
            }
        }
                      
  
        public void SERVICECHARGE ()
        {
            StreamWriter Sc;

            try
            {
                Sc = File.AppendText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Company_Charge.txt");

                SERVICE_CHARGE = TOTAL_USAGE + (CURRENT_CONSUMPTION * 200);
                Sc.WriteLine(service_charge);
                Sc.WriteLine("Service charge:{0}", service_charge);
                Sc.Close();
            }
            catch
            {
                Console.WriteLine(" file not writtten ");

            }
                Console.ReadLine();

            try
            {
                StreamReader Ta;
                Ta = File.OpenText("C:\\Users\\RON TAYLOR\\Desktop\\Computer Programing\\Company_Charge.txt");

                servicedisplay = Ta.ReadLine();
                while (servicedisplay != null)
                {
                    Console.WriteLine(servicedisplay);
                    servicedisplay = Ta.ReadLine();
                }

                Ta.Close();
            }
            catch
            {
                Console.WriteLine(" file not read ");
            }
        }

         
        
    }
 }


     
        
    



