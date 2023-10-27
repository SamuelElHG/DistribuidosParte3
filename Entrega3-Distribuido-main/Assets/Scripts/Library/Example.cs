using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerController;
using Unity.VisualScripting;
using Firebase.Auth;

public class Example : MonoBehaviour
{
    ServerController.FirebaseUser firebaseUser;
    ServerController.RankController rankController;
    ServerController.FriendsController friendsController;

    private void Start()
    {
        firebaseUser = gameObject.AddComponent<ServerController.FirebaseUser>();
        rankController = gameObject.AddComponent<ServerController.RankController>();
        friendsController = gameObject.AddComponent<ServerController.FriendsController>();
        //firebaseUser.StartServerProfile();
        //rankController.StartRankController();
        friendsController.StartFriendsController();

        //friendsController.AddNewUser("12345", "pato");
        //friendsController.AddNewUser("1234567", "andres");

        //friendsController.AddFriend("12345", "1234567");
    }
}
