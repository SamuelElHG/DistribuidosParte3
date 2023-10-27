using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ServerController
{
    public class UserRank
    {
        public string Useremail;
        public int Rank;

        public UserRank(string useremail, int rank)
        {
            Useremail = useremail;
            Rank = rank;
        }
    }
    public class User
    {
        public string[] ConectedNotification;
        public string[] Friends;
        public string[] FriendsNotification;
        public string Name;
        public bool isConected;

        public User(string name, bool isConected)
        {
            Name = name;
            this.isConected = isConected;
        }
    }
}
