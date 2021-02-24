using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Common;

namespace Client
{
    public class ChatServiceCallback : IChatServiceCallback
    {
        public void SendMessageToClients(string message)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            string b = Regex.Replace(message, @"\t", "");

            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(b);

            byte[] bbb = bytes.Skip(46).Take(32).ToArray(); 
            string decryptedMessage = mw.CryptoAlg.DecryptData(bbb);
            int lenght =decryptedMessage.Length;
            string deo = "";
            for (int i = 0; i < lenght; i++)
            {
               deo = string.Concat(deo, decryptedMessage[i]);
            }
            //String[] separator = { "\\" };
            //String[] niz = decryptedMessage.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            //string readyMessage = niz[0];
            // String[] separator2 = { "\\" }
            // String[] niz2 = compNameAndGroup.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            // string msg = niz[2];
            // // string onlyGroup = niz2[1];

            
            mw.TextBlock.Text += deo + "\n";

            //osvezava main window
            //mi inace nemamo pristup MainWindowu direktno, mi ovde napravimo objekat te klase
            //i sa klasom Application uzimamo referencu na taj window i osvezimo textBlock
        }
    }
}