using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerController;

public class FirebaseSingleton : MonoBehaviour
{
    ServerController.FirebaseUser firebaseUser;
    ServerController.RankController rankController;
    ServerController.FriendsController friendsController;

    private void Start()
    {
        firebaseUser = gameObject.AddComponent<ServerController.FirebaseUser>();
        rankController = gameObject.AddComponent<ServerController.RankController>();
        friendsController = gameObject.AddComponent<ServerController.FriendsController>();

        firebaseUser.StartServerProfile();
        rankController.StartRankController();
        friendsController.StartFriendsController();
    }
}
