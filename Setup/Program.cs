using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Out.WriteLine("Hello World");
            //Console.Out.Write("Press enter to continue...");
            //Console.In.ReadLine();
            //return;
            try
            {
                #region Install Certificate

                //X509Certificate2 cert = new X509Certificate2("Word/SFSOspc.pfx", "ehWjjuJuVZSbgBAJUR2X", X509KeyStorageFlags.PersistKeySet);
                //X509Store store = new X509Store(StoreName.My);
                //store.Open(OpenFlags.ReadWrite);
                //store.Add(cert);
                //Console.Out.WriteLine("X509Certificate2 cert = new X509Certificate2(\"C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\SFSOspc.pfx\", \"\", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);");
                //X509Certificate2 cert = new X509Certificate2("C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\SFSOspc.pfx", "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                //Console.Out.WriteLine("X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);");
                //X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                //Console.Out.WriteLine("store.Open(OpenFlags.ReadWrite);");
                //store.Open(OpenFlags.ReadWrite);
                //Console.Out.WriteLine("store.Add(cert);");
                //store.Add(cert);

                #endregion // Install Certificate

                #region Enable Prompt for Untrusted Certificates

                Microsoft.Win32.RegistryKey key;
                //key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\.NETFramework\Security\TrustManager");
                key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\.NETFramework\Security\TrustManager\PromptingLevel");
                if (key == null)
                {
                    Console.Out.WriteLine("key was null");
                }
                else
                {
                    key.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\Security\TrustManager\PromptingLevel", true);
                    key.SetValue("MyComputer", "Enabled");
                    key.SetValue("LocalIntranet", "Enabled");
                    key.SetValue("Internet", "Enabled");
                    key.SetValue("TrustedSites", "Enabled");
                    key.SetValue("UntrustedSites", "Enabled");
                    key.Close();
                }

                #endregion // Enable Prompt for Untrusted Certificates

                #region Install ClickOnce Applications
                //Console.Out.WriteLine("Process.Start(\"C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\Word\\setup.exe\");");
                //Process.Start("C:\\Users\\CTDragon\\Desktop\\ALPHA_7\\Word\\setup.exe");

                #endregion // Install ClickOnce Applications

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }

            Console.Out.Write("Press enter to continue...");
            Console.In.ReadLine();
        }
    }
}
