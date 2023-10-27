using Firebase.Database;
using Firebase.Extensions;
using ServerController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FriendsController : MonoBehaviour
{
    DatabaseReference reference;
    [SerializeField] string thisGameID = "0";
    [SerializeField] GameObject notificationPlataform;

    User thisUser = new User("", true);
    string notificationFriend = "0";
    int FriendsNotificationCount;
    int FriendsCount;
    int ConectedNotificationCount;

    string otherGameID = "0";
    int OtherFriendsNotificationCount;
    int OtherFriendsCount;
    int OtherConectedNotificationCount;


    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Update()
    {
        if(thisGameID != "0")
        {
            GetCount();
            UpdateUser(thisGameID);
            GetOtherCount(otherGameID);
            ViewRequest();
            Debug.Log(thisUser.Name);
        }
    }

    public void ViewRequest()
    {
        if (notificationFriend != "0")
        {
            otherGameID = notificationFriend;
            notificationPlataform.SetActive(true);
        }
        else
        {
            notificationPlataform.SetActive(false);
        }
    }

    public void AddNewUser(string userID, string name)
    {
        User user = new User(name, true);
        string json = JsonUtility.ToJson(user);
        string idGenerator = UnityEngine.Random.Range(1000, 9999).ToString();
        reference.Child("users").Child(idGenerator).SetRawJsonValueAsync(json);
        reference.Child("users").Child(idGenerator).Child("Friends").Child("FriendsCount").SetRawJsonValueAsync("0");
        reference.Child("users").Child(idGenerator).Child("FriendsNotification").Child("FriendsNotificationCount").SetRawJsonValueAsync("0");
        reference.Child("users").Child(idGenerator).Child("ConectedNotification").Child("ConectedNotificationCount").SetRawJsonValueAsync("0");
        reference.Child("friendsID").Child(userID).Child("UserIDGame").SetRawJsonValueAsync(idGenerator);
    }

    public void UpdateUser(string userID)
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").Child(userID).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("No se Completo la busqueda");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                thisUser.ConectedNotification = new string[ConectedNotificationCount];
                thisUser.Friends = new string[FriendsCount];
                thisUser.FriendsNotification = new string[ConectedNotificationCount];
                /*
                for (int i = 0; i < thisUser.ConectedNotification.Length; i++)
                {
                    thisUser.ConectedNotification[i] = snapshot.Child("ConectedNotification").Child("NotificationNumber" + i).Value.ToString();
                }*/

                

                for (int i = 0; i < thisUser.Friends.Length; i++)
                {
                    thisUser.Friends[i] = snapshot.Child("Friends").Child("friendNumber" + i).Value.ToString();
                }
                for (int i = 0; i < thisUser.FriendsNotification.Length; i++)
                {
                   thisUser.FriendsNotification[i] = snapshot.Child("ConectedNotification").Child("ConectedNotificationNumber" + i).Value.ToString();
                }
                notificationFriend = snapshot.Child("FriendsNotification").Child("FriendsNotificationNumber0").Value.ToString();
                thisUser.Name = snapshot.Child("Name").Value.ToString();
                thisUser.isConected = (bool)snapshot.Child("isConected").Value;
            }
        });
    }

    public void AcceptFriend()
    {
            FirebaseDatabase.DefaultInstance
        .GetReference("users").Child(notificationFriend).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("No se Completo la busqueda");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                    reference.Child("users").Child(thisGameID).Child("Friends").Child("friendNumber" + FriendsCount).SetRawJsonValueAsync(notificationFriend);
                    reference.Child("users").Child(thisGameID).Child("Friends").Child("FriendsCount").SetRawJsonValueAsync((FriendsCount + 1).ToString());
                    reference.Child("users").Child(notificationFriend).Child("Friends").Child("friendNumber" + OtherFriendsCount).SetRawJsonValueAsync(thisGameID);
                    reference.Child("users").Child(notificationFriend).Child("Friends").Child("FriendsCount").SetRawJsonValueAsync((OtherFriendsCount + 1).ToString());

            }
        });
    }

    //no tocar 
    public void GetIDGame(string myUserID)
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("friendsID").Child(myUserID).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("No se Completo la busqueda");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Child("UserIDGame"));
                thisGameID = snapshot.Child("UserIDGame").Value.ToString();
            }
        });
        
    }

    public void SendFriendRequest(string otherUserID)
    {
        otherGameID = otherUserID;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(otherUserID).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("No se Completo la busqueda");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child("Name").Value != null)
                {
                    reference.Child("users").Child(otherUserID).Child("FriendsNotification").Child("FriendsNotificationNumber" + OtherFriendsNotificationCount.ToString()).SetRawJsonValueAsync(thisGameID.ToString());
                    reference.Child("users").Child(otherUserID).Child("FriendsNotification").Child("FriendsNotificationCount").SetRawJsonValueAsync((OtherFriendsNotificationCount + 1).ToString());
                }
                else
                {
                    Debug.Log("No se encontro ningun usuario");
                }
            }
        });
    }
    //hasta acá
    public void RemoveFriendRequest()
    {
        reference.Child("users").Child(thisGameID).Child("FriendsNotification").Child("FriendsNotificationNumber0").SetRawJsonValueAsync("0");
        //reference.Child("users").Child(thisGameID).Child("FriendsNotification").Child("FriendsNotificationCount").SetRawJsonValueAsync((FriendsNotificationCount - 1).ToString());
    }

    private void GetCount()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").Child(thisGameID).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("No se Completo la busqueda");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Child("Name").Value != null)
                {
                    FriendsNotificationCount = (int)snapshot.Child("FriendsNotification").Child("FriendsNotificationCount").Value;
                    FriendsCount = (int)snapshot.Child("Friends").Child("FriendsCount").Value;
                    ConectedNotificationCount = (int)snapshot.Child("ConectedNotification").Child("ConectedNotificationCount").Value;
                }
                else
                {
                    Debug.Log("No se encontro ningun usuario");
                }
            }
        });
    }

    private void GetOtherCount(string other)
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users").Child(other).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("No se Completo la busqueda");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                    OtherFriendsNotificationCount = (int)snapshot.Child("FriendsNotification").Child("FriendsNotificationCount").Value;
                    Debug.Log(OtherFriendsNotificationCount);
                    OtherFriendsCount = (int)snapshot.Child("Friends").Child("FriendsCount").Value;
                    OtherConectedNotificationCount = (int)snapshot.Child("ConectedNotification").Child("ConectedNotificationCount").Value;
            }
        });
    }
}
