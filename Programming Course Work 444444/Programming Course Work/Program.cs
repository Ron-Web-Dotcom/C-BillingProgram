using System;

namespace Programming_Course_Work
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("============================================");
                Console.WriteLine("      WATER COMMISSION BILLING SYSTEM       ");
                Console.WriteLine("============================================");
                Console.WriteLine("  1.  Enter Customer Information");
                Console.WriteLine("  2.  Enter Meter Readings");
                Console.WriteLine("  3.  Calculate Company Charges");
                Console.WriteLine("  4.  Generate Full Bill");
                Console.WriteLine("  0.  Exit");
                Console.WriteLine("============================================");
                Console.Write("Select an option: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        Console.Clear();
                        new CustomersInfo().CustomerDisplay();
                        break;

                    case "2":
                        Console.Clear();
                        new Customer_Acc().CustomersAccount();
                        break;

                    case "3":
                        Console.Clear();
                        CompanyCharge cc = new CompanyCharge();
                        cc.TOTALUSAGE();
                        cc.SERVICECHARGE();
                        break;

                    case "4":
                        Console.Clear();
                        GenerateFullBill();
                        break;

                    case "0":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("\nInvalid option. Press Enter to try again...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        // ---------------------------------------------------------------
        // Feature: Generate a complete bill in one guided workflow
        // ---------------------------------------------------------------
        static void GenerateFullBill()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("            GENERATE FULL BILL              ");
            Console.WriteLine("============================================\n");

            // --- Customer info ---
            int    customerNum = ReadInt("Enter customer number   : ");
            Console.Write(           "Enter customer name     : ");
            string name        = Console.ReadLine();
            Console.Write(           "Enter customer address  : ");
            string address     = Console.ReadLine();

            // --- Meter readings ---
            int previous = ReadInt("Enter previous meter reading: ");
            int current;
            do
            {
                current = ReadInt("Enter current meter reading : ");
                if (current < previous)
                    Console.WriteLine("  Current reading cannot be less than the previous reading.");
            }
            while (current < previous);

            // --- Charges (show defaults; Enter key keeps them) ---
            int water         = ReadIntWithDefault("Enter water usage charge    (Enter = 289): ", 289);
            int sewage        = ReadIntWithDefault("Enter sewage usage charge   (Enter = 289): ", 289);
            int customerCharge = ReadInt(          "Enter customer charge                   : ");

            // --- Build and display bill ---
            Bills bill = new Bills(customerNum, name, address,
                                   previous, current,
                                   water, sewage, customerCharge);
            Console.WriteLine();
            bill.PrintFormattedBill();
            bill.SaveBill();

            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        // ---------------------------------------------------------------
        // Input helpers
        // ---------------------------------------------------------------

        // Read a non-negative integer; loop until the user enters a valid one.
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && result >= 0)
                    return result;
                Console.WriteLine("  Invalid input — please enter a whole number.");
            }
        }

        // Read a non-negative integer; pressing Enter returns defaultValue.
        static int ReadIntWithDefault(string prompt, int defaultValue)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input))
                    return defaultValue;
                if (int.TryParse(input, out int result) && result >= 0)
                    return result;
                Console.WriteLine($"  Invalid input — enter a whole number or press Enter for {defaultValue}.");
            }
        }
    }
}
