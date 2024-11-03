using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.Principal;

namespace KMSActivator
{
    class Program
    {
        static void Main(string[] args)
        {


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

                Console.WriteLine("Generated Product Key:");
                string LicenseKey = GenerateKey();
                Console.WriteLine(LicenseKey);

                try
                {
                    RunCommand($"slmgr /ipk {LicenseKey}");
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
