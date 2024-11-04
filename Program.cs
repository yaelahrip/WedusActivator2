using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Win32;

namespace KMSActivator
{
    class Program
    {
        static void Main(string[] args)
        {
            string Licensenyabwang = ""; // dont bother

            if (IsRunAsAdmin())
            {

                Console.WriteLine("Yo, just a heads up!\nThis program is strictly for educational vibes. Not gonna lie,\nI can't guarantee it's 100% awesome, but it's a cool example of how programs work!");
                Console.WriteLine("Coded with love by @yaelahrip!\nJust vibin' with code and showing how the magic happens! Keep coding and stay awesome, fam!\n\n");


                // Display the differences in a table format
                Console.WriteLine("KMS Server Types - Online vs. Offline\n");
                Console.WriteLine("| Feature               | Online KMS Server                       | Offline KMS Server                    |");
                Console.WriteLine("|-----------------------|-----------------------------------------|---------------------------------------|");
                Console.WriteLine("| Internet Access       | Required                                | Not Required                          |");
                Console.WriteLine("| Network Type          | Public or Private Network               | Local/Intranet Only                   |");
                Console.WriteLine("| Activation Updates    | Periodically syncs with Microsoft       | No sync; updates applied manually     |");
                Console.WriteLine("| Use Case              | Devices in varied locations             | Secure or isolated environments       |");
                Console.WriteLine("| Activation Renewal    | Can be automated                        | Clients must reconnect every 180 days |");

                // Define online and offline KMS servers
                List<string> kmsServersOnline = new List<string>
                {
                    "kms.digiboy.ir",
                    "54.223.212.31",
                    "kms.cnlic.com",
                    "kms.chinancce.com",
                    "kms.ddns.net",
                    "franklv.ddns.net",
                    "k.zpale.com",
                    "m.zpale.com",
                    "mvg.zpale.com",
                    "kms.shuax.com",
                    "kensol263.imwork.net:1688",
                    "kms.loli.best",
                    "kms.vudy.net",
                };

                List<string> kmsServersOffline = new List<string>
                {
                    "kms.lotro.cc",
                    "mhd.kmdns.net110",
                    "noip.me",
                    "45.78.3.223",
                    "kms.didichuxing.coms",
                    "zh.us.to",
                    "toxykz.f3322.org",
                    "192.168.2.81.2.7.0",
                    "kms.guowaifuli.com",
                    "106.186.25.2393",
                    "rss.vicp.net:20439",
                    "122.226.152.230",
                    "222.76.251.188",
                    "annychen.pw",
                    "heu168.6655.la",
                    "kms.aglc.cc",
                    "kms.landiannews.com",
                    "kms.xspace.in",
                    "winkms.tk"
                };

                // Ask the user if they want an online or offline server
                Console.WriteLine("\nPlease choose the type of KMS server you would like to use:");
                Console.WriteLine("1. Online KMS Server");
                Console.WriteLine("2. Offline KMS Server");
                Console.Write("Enter your choice (1 or 2): ");
                string choice = Console.ReadLine();

                // Display the list of servers based on the user's choice
                if (choice == "1")
                {
                    Console.WriteLine("\nYou selected Online KMS Servers. Here is the list:");
                    for (int i = 0; i < kmsServersOnline.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {kmsServersOnline[i]}");
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("\nYou selected Offline KMS Servers. Here is the list:");
                    for (int i = 0; i < kmsServersOffline.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {kmsServersOffline[i]}");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid choice. Please run the program again and select either 1 or 2.");
                    return; // Exit the program for invalid input
                }

                // Ask the user to select a server by its number
                Console.Write("Please enter the number of the server you want to select: ");
                string serverChoice = Console.ReadLine();
                int selectedIndex;
                string serverChoosen = "";

                // Validate the server choice and process it
                if (choice == "1" && int.TryParse(serverChoice, out selectedIndex) && selectedIndex > 0 && selectedIndex <= kmsServersOnline.Count)
                {
                    serverChoosen = kmsServersOnline[selectedIndex - 1];
                    Console.WriteLine($"You have selected the server: {serverChoosen}");
                }
                else if (choice == "2" && int.TryParse(serverChoice, out selectedIndex) && selectedIndex > 0 && selectedIndex <= kmsServersOffline.Count)
                {
                    serverChoosen = kmsServersOffline[selectedIndex - 1];
                    Console.WriteLine($"You have selected the server: {serverChoosen}");
                }
                else
                {
                    Console.WriteLine("\nInvalid server number. Please run the program again and select a valid server number.");
                }

                if (TryGetProductKey(out string productKey))
                {
                    Licensenyabwang = productKey;
                    Console.WriteLine($"Matching Product Key from our DB: {productKey}");
                }
                else
                {
                    Console.WriteLine("No matching key found for this version.");
                    Console.WriteLine("Generated Random Product Key:");
                    Licensenyabwang = GenerateKey();
                    Console.WriteLine(Licensenyabwang);
                }

                

                try
                {
                    RunCommand($"slmgr /ipk {Licensenyabwang}");
                    RunCommand($"slmgr /skms {serverChoosen}");
                    RunCommand("slmgr /ato"); // Activate
                    RunCommand("slmgr /xpr"); // Check activation status
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during activation: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Please run the application as an administrator.");
            }

        }

        static void RunCommand(string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/c {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine($"Output: {output}");
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"Error: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        static bool IsRunAsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static string GenerateKey()
        {
            string part1 = GenerateKeyPart(5);
            string part2 = GenerateKeyPart(5);
            string part3 = GenerateKeyPart(5);
            string part4 = GenerateKeyPart(5);
            string part5 = GenerateKeyPart(5);

            return $"{part1}-{part2}-{part3}-{part4}-{part5}";
        }

        static bool TryGetProductKey(out string productKey)
        {
            string productName = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "Unknown");
            string releaseId = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "Unknown");

            Console.WriteLine($"Windows Version: {productName}");
            Console.WriteLine($"Release ID: {releaseId}");

            // Dictionary for Windows version to key mapping
            var windowsKeys = new Dictionary<string, string>
            {
                { "Windows 11 Home", "TX9XD-98N7V-6WMQ6-BX7FG-H8Q99" },
                { "Windows 11 Home N", "3KHY7-WNT83-DGQKR-F7HPR-844BM" },
                { "Windows 11 Home Home Single Language", "7HNRX-D7KGG-3K4RQ-4WPJ4-YTDFH" },
                { "Windows 11 Home Country Specific", "PVMJN-6DFY6-9CCP6-7BKTT-D3WVR" },
                { "Windows 11 Pro", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
                { "Windows 11 Pro N", "MH37W-N47XK-V7XM9-C7227-GCQG9" },
                { "Windows 11 Pro for Workstations", "NRG8B-VKK3Q-CXVCJ-9G2XF-6Q84J" },
                { "Windows 11 Pro for Workstations N", "9FNHH-K3HBT-3W4TD-6383H-6XYWF" },
                { "Windows 11 Pro Education", "6TP4R-GNPTD-KYYHQ-7B7DP-J447Y" },
                { "Windows 11 Pro Education N", "YVWGF-BXNMC-HTQYQ-CPQ99-66QFC" },
                { "Windows 11 Education", "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2" },
                { "Windows 11 Education N", "2WH4N-8QGBV-H22JP-CT43Q-MDWWJ" },
                { "Windows 11 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
                { "Windows 11 Enterprise N", "DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4" },
                { "Windows 11 Enterprise G", "YYVX9-NTFWV-6MDM3-9PT4T-4M68B" },
                { "Windows 11 Enterprise G N", "44RPN-FTY23-9VTTB-MP9BX-T84FV" },
                { "Windows 11 Enterprise LTSC 2019", "M7XTQ-FN8P6-TTKYV-9D4CC-J462D" },
                { "Windows 11 Enterprise N LTSC 2019", "92NFX-8DJQP-P6BBQ-THF9C-7CG2H" },



                { "Windows 10 Home", "TX9XD-98N7V-6WMQ6-BX7FG-H8Q99" },
                { "Windows 10 Home N", "3KHY7-WNT83-DGQKR-F7HPR-844BM" },
                { "Windows 10 Home Single Language", "7HNRX-D7KGG-3K4RQ-4WPJ4-YTDFH" },
                { "Windows 10 Pro", "W269N-WFGWX-YVC9B-4J6C9-T83GX" },
                { "Windows 10 Pro N", "MH37W-N47XK-V7XM9-C7227-GCQG9" },
                { "Windows 10 Pro for Workstations", "NRG8B-VKK3Q-CXVCJ-9G2XF-6Q84J" },
                { "Windows 10 Pro N for Workstations", "9FNHH-K3HBT-3W4TD-6383H-6XYWF" },
                { "Windows 10 Education", "NW6C2-QMPVW-D7KKK-3GKT6-VCFB2" },
                { "Windows 10 Education N", "2WH4N-8QGBV-H22JP-CT43Q-MDWWJ" },
                { "Windows 10 Pro Education", "6TP4R-GNPTD-KYYHQ-7B7DP-J447Y" },
                { "Windows 10 Pro Education N", "YVWGF-BXNMC-HTQYQ-CPQ99-66QFC" },
                { "Windows 10 Enterprise", "NPPR9-FWDCX-D2C8J-H872K-2YT43" },
                { "Windows 10 Enterprise G", "YYVX9-NTFWV-6MDM3-9PT4T-4M68B" },
                { "Windows 10 Enterprise G N", "44RPN-FTY23-9VTTB-MP9BX-T84FV" },
                { "Windows 10 Enterprise N", "DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4" },
                { "Windows 10 Enterprise S", "FWN7H-PF93Q-4GGP8-M8RF3-MDWWW" },
                { "Windows 10 Enterprise 2015 LTSB", "WNMTR-4C88C-JK8YV-HQ7T2-76DF9" },
                { "Windows 10 Enterprise 2015 LTSB N", "2F77B-TNFGY-69QQF-B8YKP-D69TJ" },
                { "Windows 10 Enterprise LTSB 2016", "DCPHK-NFMTC-H88MJ-PFHPY-QJ4BJ" },
                { "Windows 10 Enterprise N LTSB 2016", "QFFDN-GRT3P-VKWWX-X7T3R-8B639" },
                { "Windows 10 Enterprise LTSC 2019", "M7XTQ-FN8P6-TTKYV-9D4CC-J462D" },
                { "Windows 10 Enterprise N LTSC 2019", "92NFX-8DJQP-P6BBQ-THF9C-7CG2H" },
                { "Windows Server 2016 Datacenter", "CB7KF-BWN84-R7R2Y-793K2-8XDDG" },
                { "Windows Server 2016 Standard", "WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY" },
                { "Windows Server 2016 Essentials", "JCKRF-N37P4-C2D82-9YXRT-4M63B" },
                { "Windows Server 2019 Datacenter", "WMDGN-G9PQG-XVVXX-R3X43-63DFG" },
                { "Windows Server 2019 Standard", "N69G4-B89J2-4G8F4-WWYCC-J464C" },
                { "Windows Server 2019 Essentials", "WVDHN-86M7X-466P6-VHXV7-YY726" },
            };

            // Try to get the product key from the dictionary
            if (windowsKeys.TryGetValue(productName, out productKey))
            {
                return true; // Key found
            }

            productKey = null; // Set to null if key not found
            return false; // Indicate failure
        }

        static string GenerateKeyPart(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] part = new char[length];

            for (int i = 0; i < length; i++)
            {
                part[i] = chars[random.Next(chars.Length)];
            }

            return new string(part);
        }
    }
}
