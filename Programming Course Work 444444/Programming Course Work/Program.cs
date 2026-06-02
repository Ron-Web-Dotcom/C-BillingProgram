using System;
using System.IO;
using System.Linq;

namespace Programming_Course_Work
{
    // Application entry point. Presents an 8-option menu and dispatches to the
    // appropriate workflow. All data files are written to DataDir on the Desktop.
    class Program
    {
        // Portable path to the shared output folder — same directory used by all classes.
        private static readonly string DataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            "Computer Programing");

        static void Main(string[] args)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("============================================");
                Console.WriteLine("      WATER COMMISSION BILLING SYSTEM       ");
                Console.WriteLine("============================================");
                Console.WriteLine("  --- Data Entry ---");
                Console.WriteLine("  1.  Enter Customer Information");
                Console.WriteLine("  2.  Enter Meter Readings");
                Console.WriteLine("  3.  Calculate Company Charges");
                Console.WriteLine("  --- Billing ---");
                Console.WriteLine("  4.  Generate Full Bill");
                Console.WriteLine("  --- Records ---");
                Console.WriteLine("  5.  View Saved Bills");
                Console.WriteLine("  6.  Search Bills by Customer Number");
                Console.WriteLine("  7.  Delete a Bill");
                Console.WriteLine("  8.  Clear All Records");
                Console.WriteLine("  ---");
                Console.WriteLine("  0.  Exit");
                Console.WriteLine("============================================");
                Console.Write("Select an option: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": Console.Clear(); new CustomersInfo().CustomerDisplay(); break;
                    case "2": Console.Clear(); new Customer_Acc().CustomersAccount(); break;
                    case "3":
                        Console.Clear();
                        CompanyCharge cc = new CompanyCharge();
                        cc.TOTALUSAGE();
                        cc.SERVICECHARGE();
                        break;
                    case "4": Console.Clear(); GenerateFullBill();        break;
                    case "5": Console.Clear(); ViewSavedBills();          break;
                    case "6": Console.Clear(); SearchBillsByCustomer();   break;
                    case "7": Console.Clear(); DeleteABill();             break;
                    case "8": Console.Clear(); ClearAllRecords();         break;
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

        // Guided single-session workflow that collects all customer, meter, and charge data,
        // calculates the full bill (including GCT), prints it to the console, and saves it
        // to a timestamped file via Bills.SaveBill().
        static void GenerateFullBill()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("            GENERATE FULL BILL              ");
            Console.WriteLine("============================================\n");

            int    customerNum = ReadInt("Enter customer number   : ");
            Console.Write(              "Enter customer name     : ");
            string name        = Console.ReadLine();
            Console.Write(              "Enter customer address  : ");
            string address     = Console.ReadLine();

            int previous = ReadInt("Enter previous meter reading: ");
            int current;
            do
            {
                current = ReadInt("Enter current meter reading : ");
                if (current < previous)
                    Console.WriteLine("  Current reading cannot be less than the previous reading.");
            }
            while (current < previous);

            // Water and sewage usage default to $289 — press Enter to accept the default.
            int water          = ReadIntWithDefault("Enter water usage charge    (Enter = 289): ", 289);
            int sewage         = ReadIntWithDefault("Enter sewage usage charge   (Enter = 289): ", 289);
            int customerCharge = ReadInt(           "Enter customer charge                   : ");

            Bills bill = new Bills(customerNum, name, address,
                                   previous, current,
                                   water, sewage, customerCharge);
            Console.WriteLine();
            bill.PrintFormattedBill();
            bill.SaveBill();
            Pause();
        }

        // Lists all Bill_Customer*.txt files in DataDir and lets the user pick one to view.
        static void ViewSavedBills()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("             VIEW SAVED BILLS               ");
            Console.WriteLine("============================================\n");

            string[] files = GetBillFiles();
            if (files == null) return;

            PrintFileList(files);

            int choice = ReadIntWithDefault(
                $"Enter bill number to view (1-{files.Length}, Enter to cancel): ", 0);
            if (choice < 1 || choice > files.Length) return;

            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(files[choice - 1]));
            Pause();
        }

        // Filters saved bills to a specific customer number using the
        // Bill_Customer{N}_*.txt filename convention, then lets the user view one.
        static void SearchBillsByCustomer()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("        SEARCH BILLS BY CUSTOMER            ");
            Console.WriteLine("============================================\n");

            int customerNum = ReadInt("Enter customer number to search: ");

            if (!Directory.Exists(DataDir))
            {
                Console.WriteLine("No records found — the data directory has not been created yet.");
                Pause();
                return;
            }

            string[] files = Directory
                .GetFiles(DataDir, "Bill_Customer" + customerNum + "_*.txt")
                .OrderBy(f => f)
                .ToArray();

            if (files.Length == 0)
            {
                Console.WriteLine("\nNo bills found for customer #" + customerNum + ".");
                Pause();
                return;
            }

            Console.WriteLine("\nFound " + files.Length + " bill(s) for customer #" + customerNum + ":");
            PrintFileList(files);

            int choice = ReadIntWithDefault(
                $"Enter bill number to view (1-{files.Length}, Enter to cancel): ", 0);
            if (choice < 1 || choice > files.Length) return;

            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(files[choice - 1]));
            Pause();
        }

        // Lists all saved bills and permanently deletes the one chosen by the user.
        // Requires the user to type "YES" to confirm before deletion proceeds.
        static void DeleteABill()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("               DELETE A BILL                ");
            Console.WriteLine("============================================\n");

            string[] files = GetBillFiles();
            if (files == null) return;

            PrintFileList(files);

            int choice = ReadIntWithDefault(
                $"Enter bill number to delete (1-{files.Length}, Enter to cancel): ", 0);
            if (choice < 1 || choice > files.Length) return;

            string selected = files[choice - 1];
            Console.WriteLine("\nYou are about to permanently delete:");
            Console.WriteLine("  " + Path.GetFileName(selected));
            Console.Write("Type YES to confirm: ");

            if (Console.ReadLine()?.Trim().ToUpper() == "YES")
            {
                try
                {
                    File.Delete(selected);
                    Console.WriteLine("Bill deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to delete: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Deletion cancelled — no files were removed.");
            }

            Pause();
        }

        // Lists every .txt file in DataDir and deletes all of them after typed "YES" confirmation.
        // This wipes Customer_Information.txt, Customer_Account.txt, Company_Charge.txt,
        // Bills.txt, and all timestamped bill files.
        static void ClearAllRecords()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("             CLEAR ALL RECORDS              ");
            Console.WriteLine("============================================\n");

            if (!Directory.Exists(DataDir))
            {
                Console.WriteLine("No records found — the data directory has not been created yet.");
                Pause();
                return;
            }

            string[] allFiles = Directory.GetFiles(DataDir, "*.txt");
            if (allFiles.Length == 0)
            {
                Console.WriteLine("No data files to clear.");
                Pause();
                return;
            }

            Console.WriteLine("The following files will be permanently deleted:");
            Console.WriteLine(new string('-', 50));
            foreach (string f in allFiles)
                Console.WriteLine("  " + Path.GetFileName(f));
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("\nWARNING: This action cannot be undone.");
            Console.Write("Type YES to confirm: ");

            if (Console.ReadLine()?.Trim().ToUpper() != "YES")
            {
                Console.WriteLine("Cancelled — no files were deleted.");
                Pause();
                return;
            }

            int deleted = 0, failed = 0;
            foreach (string f in allFiles)
            {
                try   { File.Delete(f); deleted++; }
                catch { failed++; }
            }

            Console.WriteLine("\n" + deleted + " file(s) deleted" +
                              (failed > 0 ? ", " + failed + " could not be removed." : "."));
            Pause();
        }

        // --- Shared helpers ---

        // Returns all Bill_Customer*.txt files in DataDir sorted by name,
        // or null (after printing a message) if none exist.
        static string[] GetBillFiles()
        {
            if (!Directory.Exists(DataDir))
            {
                Console.WriteLine("No records found — the data directory has not been created yet.");
                Pause();
                return null;
            }

            string[] files = Directory.GetFiles(DataDir, "Bill_Customer*.txt")
                                      .OrderBy(f => f)
                                      .ToArray();

            if (files.Length == 0)
            {
                Console.WriteLine("No saved bills found.");
                Pause();
                return null;
            }

            return files;
        }

        // Prints a 1-based numbered list of file names (no directory path).
        static void PrintFileList(string[] files)
        {
            Console.WriteLine(new string('-', 55));
            for (int i = 0; i < files.Length; i++)
                Console.WriteLine("  " + (i + 1).ToString().PadLeft(2) + ".  " + Path.GetFileName(files[i]));
            Console.WriteLine(new string('-', 55));
        }

        // Pauses the console until the user presses Enter, then returns to the menu loop.
        static void Pause()
        {
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }

        // Loops until the user enters a valid non-negative integer.
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

        // Loops until the user enters a valid non-negative integer or presses Enter.
        // Pressing Enter returns defaultValue (used for optional fields like water/sewage tariff).
        static int ReadIntWithDefault(string prompt, int defaultValue)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input)) return defaultValue;
                if (int.TryParse(input, out int result) && result >= 0) return result;
                Console.WriteLine($"  Invalid input — enter a whole number or press Enter for {defaultValue}.");
            }
        }
    }
}
