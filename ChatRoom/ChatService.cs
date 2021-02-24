using ChatRoom.Cryptography;
using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatRoom
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChatService
    {
        //  Dictionary<string, List<string>> chatRoomMessages = new Dictionary<string, List<string>>();

        public void SendMessageToServer(string message)
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;

            // svim korisnicima iz te grupe salje poruku (poziva metodu SendMessageToClients)
            GroupsHelper.Instance.BroadcastMessage(message, winIdentity.Name);

            string groupRealName = "";
            string realName = winIdentity.Name.Split('\\')[1]; // uzmem samo usera bez kompa
            string completeMessage = "";
            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var groupName = sid.Translate(typeof(NTAccount));
                if (groupName.ToString().Contains('\\'))
                {
                    groupRealName = groupName.ToString().Split('\\')[1];
                    if (GroupsHelper.Instance.CheckIfGroupExsits(groupRealName))
                    {
                        completeMessage = "\nGroup: " + groupRealName + "\nUser: " + realName + "\nmessage: " + message + "\n";
                        break;
                    }
                }
            }
            //kada posaljes svim klijentima salje i na drugi server
            ServerServerChannel channel = new ServerServerChannel();

            // metoda iz IChatService koja je implementirana na serveru
            channel.ProxyForward.SendMessageToSecondaryServer(completeMessage);
        }

        public void ConnectTo()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;

            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var groupName = sid.Translate(typeof(NTAccount));
                if (groupName.ToString().Contains('\\'))
                {
                    string groupRealName = groupName.ToString().Split('\\')[1];
                    // kada izvucemo iz koje je grupe korisnik koji je poslao poruku, provjerimo da li ta grupa postoji u App.Config
                    if (GroupsHelper.Instance.CheckIfGroupExsits(groupRealName))
                    {
                        //prvi put kad posalje poruku on ce ga ovde dodati, svaki sledeci put ce ga samo preskociti
                        //Callback mi je Instanca interfejsa za callback, to je callback za trenutnog clienta
                        //Za svakog usera kada posalje poruku mozemo uzeti preko OperationContext klase callback kanal

                        //  AddUserToGroup zapamti iz koje sobe(chatRooma) nam je dosla poruka, koji korisnik ju je poslao i kojim putem, da bismo tim istim putem mogli vratiti poruku
                        GroupsHelper.Instance.AddUserToGroup(groupRealName, winIdentity.Name, Callback);
                        break;
                    }
                }
            }
        }

        public ICryptographyInterface GetCryptoAlg()
        {
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity winIdentity = identity as WindowsIdentity;
            string groupRealName = "";
            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var groupName = sid.Translate(typeof(NTAccount));
                if (groupName.ToString().Contains('\\'))
                {
                    groupRealName = groupName.ToString().Split('\\')[1];
                    // kada izvucemo iz koje je grupe korisnik koji je poslao poruku, provjerimo da li ta grupa postoji u App.Config
                    if (GroupsHelper.Instance.CheckIfGroupExsits(groupRealName))
                    {
                        //prvi put kad posalje poruku on ce ga ovde dodati, svaki sledeci put ce ga samo preskociti
                        //Callback mi je Instanca interfejsa za callback, to je callback za trenutnog clienta
                        //Za svakog usera kada posalje poruku mozemo uzeti preko OperationContext klase callback kanal

                        //  AddUserToGroup zapamti iz koje sobe(chatRooma) nam je dosla poruka, koji korisnik ju je poslao i kojim putem, da bismo tim istim putem mogli vratiti poruku
                        //  GroupsHelper.Instance.AddUserToGroup(groupRealName, winIdentity.Name, Callback);
                        var r = CryptoHelper.GetAlgForGroup(groupRealName);
                        return r;
                        break;
                    }
                }
            }
            return null;
        }

        // OperationContext.Current mi je referenca na trenutno pozvanu metodu nekog clienta (SendMessageToServer)
        // ja za tog clienta mogu da uzmem callbackChannel i da onda pozivam metodu na tom clientu iz tog callback kanala (SendMessageToClients)
        // GetCallbackChannel je ugradjena metoda OperationContext-a koji je ugradjena klasa
        private IChatServiceCallback Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            }
        }
    }
}