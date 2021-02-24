using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom
{
    public class GroupsHelper
    {
        private static GroupsHelper _instance = null;
        private static object _padLock = new object();

        //sve grupe sa svojim userima, key je ime grupe, HashSet svih usera u toj grupi je value
        private Dictionary<string, HashSet<string>> _groupsWithUsers = new Dictionary<string, HashSet<string>>();

        //svi useri sa svojim callback-ovima(kanal), key je ime user-a
        private Dictionary<string, IChatServiceCallback> _userCallbacks = new Dictionary<string, IChatServiceCallback>();

        public static GroupsHelper Instance
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                        _instance = new GroupsHelper();

                    return _instance;
                }
            }
        }

        public GroupsHelper()
        {   // izvucem sve grupe iz konfiguracije
            List<string> groups = ConfigurationManager.AppSettings["groups"].Split(',').ToList();
            foreach (string group in groups)
            {
                // inicijaliujem svaku grupu, koristim kolekciju hashSet da bi zabranila postojanje 2 iste grupe
                _groupsWithUsers[group] = new HashSet<string>();
            }
        }

        // metoda koja provjejrava da li grupa postoji u dictionary-ju
        public bool CheckIfGroupExsits(string groupName)
        {
            return _groupsWithUsers.ContainsKey(groupName);
        }

        // ako postoji (prethodna metoda) onda cemo zapamtiti grupu, korisnika i kanal
        public void AddUserToGroup(string groupName, string userName, IChatServiceCallback callBack)
        {
            if (_groupsWithUsers.ContainsKey(groupName))
            {
                _groupsWithUsers[groupName].Add(userName);  //zapamtimo usera
                _userCallbacks[userName] = callBack;        //zapamtimo kanal
            }
        }

        // Metoda koja ce svim korisnicima, zapamcenim u metodi AddUserToGroup, vratiti poruku
        public void BroadcastMessage(string message, string userName)
        {
            string group = GetGroupForUser(userName); //vratim grupu tog korisnika koji je poslao poruku
            if (group != null)
            {
                //za sve korisnike koji su u istoj grupi
                foreach (var user in _groupsWithUsers[group])
                {
                    //pristupam drugoj kolekciji koja za svakog korisnika vraca kanal i na taj kanal saljem nazad poruku
                    //salje svima koje smo zapamtili gore u AddUserToGroup
                    _userCallbacks[user].SendMessageToClients($"{userName}:\t{message}");
                }
            }
        }

        // Metoda koja vraca ime grupe u kojoj je korisnik koji je poslao poruku serveru
        private string GetGroupForUser(string userName)
        {
            foreach (var groupName in _groupsWithUsers.Keys)   //za sve grupe iz kolekcije
            {
                //proveri da li se u HashSetu te grupe nalazi trazeni user
                //ako se nalazi vratimo tu grupu
                if (_groupsWithUsers[groupName].Contains(userName))
                    return groupName;
            }
            return null;
        }

        //Na poruku dodajemo username i grupu da bi je poslali na sporedni server
        public void AddUserToGroup2(string groupName, string userName)
        {
            if (_groupsWithUsers.ContainsKey(groupName))
            {
                _groupsWithUsers[groupName].Add(userName);  //zapamtimo usera
            }
        }
    }
}