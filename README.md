# Water Commission Billing System

A C# .NET 4.5 console application for managing water utility customer accounts, meter readings, and bill generation.

---

## Project Structure

```
C-BillingProgram/
└── Programming Course Work 444444/
    └── Programming Course Work/
        ├── Program.cs           ← Entry point & main menu
        ├── CustomersInfo.cs     ← Base class: customer details
        ├── Customer_Acc.cs      ← Derived class: meter readings
        ├── CompanyCharge.cs     ← Derived class: charges & GCT
        ├── Bills.cs             ← Composition class: full bill
        └── App.config
```

---

## Class Architecture

```
CustomersInfo          (base)
    └── Customer_Acc   (inherits CustomersInfo)
            └── CompanyCharge (inherits Customer_Acc)

Bills                  (composition: holds CustomersInfo + Customer_Acc + CompanyCharge)
```

| Class | Responsibility |
|---|---|
| `CustomersInfo` | Stores and persists customer number, name, address |
| `Customer_Acc` | Stores previous/current meter readings; calculates consumption |
| `CompanyCharge` | Stores water/sewage/service/customer charges; holds GCT rate (16.5%) |
| `Bills` | Ties all three together; calculates totals; prints and saves formatted bill |
| `Program` | 8-option main menu; drives all user workflows |

---

## How to Build and Run

**Requirements:** Visual Studio 2013+ or .NET Framework 4.5 SDK

1. Open `Programming Course Work 444444/Programming Course Work.sln` in Visual Studio.
2. Press **F5** (Debug) or **Ctrl+F5** (Run without debugging).
3. The program creates a `Computer Programing` folder on the Desktop and writes all output files there.

**Command line:**
```
msbuild "Programming Course Work 444444\Programming Course Work.sln"
"Programming Course Work 444444\Programming Course Work\bin\Debug\Programming Course Work.exe"
```

---

## Menu Overview

```
============================================
      WATER COMMISSION BILLING SYSTEM
============================================
  --- Data Entry ---
  1.  Enter Customer Information
  2.  Enter Meter Readings
  3.  Calculate Company Charges
  --- Billing ---
  4.  Generate Full Bill
  --- Records ---
  5.  View Saved Bills
  6.  Search Bills by Customer Number
  7.  Delete a Bill
  8.  Clear All Records
  ---
  0.  Exit
============================================
```

---

## Features

### Option 1 — Enter Customer Information
Prompts for customer number (validated integer), name, and address. Appends the record to `Customer_Information.txt` and prints the file contents.

### Option 2 — Enter Meter Readings
Prompts for previous and current meter readings with validation:
- Both values must be non-negative integers.
- Current reading must be ≥ previous reading (loops until satisfied).
- Calculates consumption automatically. Appends to `Customer_Account.txt`.

### Option 3 — Calculate Company Charges
Uses `CompanyCharge` to calculate total usage (water + sewage) and service charge (total usage + consumption × $200). Appends results to `Company_Charge.txt`.

### Option 4 — Generate Full Bill
Single guided workflow collecting all data in one session:
1. Customer information
2. Meter readings (with validation)
3. Water and sewage usage charges (default 289 each — press Enter to accept)
4. Customer charge

Calculates service charge, applies 16.5% GCT, prints a formatted bill to the console, and saves it to a timestamped file.

### Option 5 — View Saved Bills
Lists all `Bill_Customer*.txt` files in the data directory. User selects a number to view the full contents of that bill.

### Option 6 — Search Bills by Customer Number
Filters saved bills to a specific customer number. Displays matching files and allows the user to view any one of them.

### Option 7 — Delete a Bill
Lists all saved bills. After the user selects one, requires typing `YES` to confirm permanent deletion.

### Option 8 — Clear All Records
Lists every `.txt` file in the data directory. Requires typing `YES` to delete all of them. Cannot be undone.

---

## Bill Calculation Formula

```
Consumption        = Current Reading − Previous Reading
Service Charge     = (Water Usage + Sewage Usage) + (Consumption × $200)
Sub-Total          = Service Charge + Customer Charge
GCT Amount         = Sub-Total × 16.5%
TOTAL DUE          = Sub-Total + GCT Amount
```

### Example

| Field | Value |
|---|---|
| Previous Reading | 100 units |
| Current Reading | 150 units |
| Consumption | 50 units |
| Water Usage | $289.00 |
| Sewage Charge | $289.00 |
| Consumption Charge | $10,000.00 (50 × $200) |
| Service Charge | $10,578.00 |
| Customer Charge | $500.00 |
| Sub-Total | $11,078.00 |
| GCT (16.5%) | $1,827.87 |
| **TOTAL DUE** | **$12,905.87** |

---

## Output Files

All files are written to `%USERPROFILE%\Desktop\Computer Programing\`

| File | Content |
|---|---|
| `Customer_Information.txt` | Appended customer records (Option 1) |
| `Customer_Account.txt` | Appended meter reading records (Option 2) |
| `Company_Charge.txt` | Appended charge calculation records (Option 3) |
| `Bill_Customer{N}_{timestamp}.txt` | Individual formatted bill (Option 4) |

---

## Bug Fixes Applied

| # | File | Bug | Fix |
|---|---|---|---|
| 1 | CustomersInfo.cs | Hardcoded `C:\Users\RON TAYLOR\Desktop\` path | Replaced with `Environment.GetFolderPath(SpecialFolder.Desktop)` |
| 2 | CustomersInfo.cs | File opened before input — handle leaked on bad input | Moved all input collection before `File.AppendText` |
| 3 | CustomersInfo.cs | `int.Parse` crashes on non-numeric input | Replaced with `int.TryParse` loop |
| 4 | CustomersInfo.cs | Typo `"CUstomer number"` in format string | Fixed capitalisation |
| 5 | Customer_Acc.cs | Hardcoded path | Replaced with portable path |
| 6 | Customer_Acc.cs | File opened before input | Moved input collection before file open |
| 7 | Customer_Acc.cs | No validation: current < previous meter accepted silently | Added `do…while` loop rejecting invalid readings |
| 8 | Customer_Acc.cs | `int.Parse` crashes on non-numeric input | Replaced with `ReadInt()` helper |
| 9 | CompanyCharge.cs | Hardcoded path | Replaced with portable path |
| 10 | CompanyCharge.cs | Constructor parameter name `pervious` (typo) | Corrected to `previous` |
| 11 | CompanyCharge.cs | `WATER_USAGE` setter assigned hardcoded `289` instead of `value` | Fixed to `water_usage = value` |
| 12 | CompanyCharge.cs | `SEWAGE_USAGE` setter assigned hardcoded `289` instead of `value` | Fixed to `sewage_usage = value` |
| 13 | CompanyCharge.cs | `GCT` setter assigned hardcoded `16.5` instead of `value` | Fixed to `gct = value` |
| 14 | CompanyCharge.cs | `SERVICECHARGE()`: `SERVICE_CHARGE = ...` line was commented out | Uncommented the assignment |
| 15 | CompanyCharge.cs | `SERVICECHARGE()`: duplicate raw value written to file | Removed duplicate line |
| 16 | CompanyCharge.cs | `SERVICECHARGE()`: format string had extra space `"service charge  :"` | Fixed spacing |
| 17 | CompanyCharge.cs | No `Directory.CreateDirectory` before writes | Added `Directory.CreateDirectory(DataDir)` |
| 18 | Bills.cs | Hardcoded path | Replaced with portable path |
| 19 | Bills.cs | `Bills(int previous, int current, int consumption)` passed args in wrong order to `Customer_Acc` | Swapped to `new Customer_Acc(current, previous, consumption)` |
| 20 | Bills.cs | `gct` field existed but was never used in any calculation | Added `CalculateTotal()` applying GCT |
| 21 | Bills.cs | No null guards on composition objects | Added null checks in `Bill01()`, `Bills02()`, `Bills03()` |
| 22 | Bills.cs | No formatted bill output | Added `PrintFormattedBill()` method |
| 23 | Bills.cs | No persistent bill file per customer | Added `SaveBill()` writing timestamped `.txt` files |

---

## Future Enhancements

- Payment tracking (record payments against outstanding bills)
- Monthly billing reports with totals across all customers
- Multiple billing periods per customer with history view
- Customer record editing (update name, address)
- CSV / Excel export of billing data
- SQLite or SQL Server database back-end to replace flat text files
