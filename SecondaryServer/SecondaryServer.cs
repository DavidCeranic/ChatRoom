using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    public class SecondaryServer : ISecondaryServer
    {
        public void SendMessageToSecondaryServer(string message)
        {
            Baza.podaci.Add(message);
            Console.WriteLine(message);
            writeTxt(message);
        }

        public static void writeTxt(string message)
        {
           // String[] separator = { "[", "]" };
           // String[] niz = message.Split(separator, StringSplitOptions.RemoveEmptyEntries);

           //string compNameAndGroup = niz[1];
           // String[] separator2 = { "\\" };
           // String[] niz2 = compNameAndGroup.Split(separator, StringSplitOptions.RemoveEmptyEntries);
           // string msg = niz[2];
           // // string onlyGroup = niz2[1];
           // string faileName = compNameAndGroup + ".txt";

            if (!File.Exists("Novo.txt"))
            {
                StreamWriter sw = new StreamWriter("Novo.txt");

                sw.WriteLine(message);
                sw.Close();
            }
            else
            {
                Stream fs = new FileStream("Novo.txt", FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(message);
                sw.Close();
            }
        }
    }
}