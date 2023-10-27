using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(FriendsController))]
public class FriendsOnUnityController : MonoBehaviour
{
    [SerializeField] AuthManager authManager;
    FriendsController friendsController;
    [SerializeField] TMP_InputField friendID;
    private void Start()
    {
        friendsController = GetComponent<FriendsController>();
    }

    private void Update()
    {
        
    }

    public void AddFriend()
    {
        friendsController.SendFriendRequest(friendID.text);
    }

    public void AcceptFriend()
    {
        friendsController.AcceptFriend();
    }

    public void RemoveNotification()
    {
        friendsController.RemoveFriendRequest();
    }
}
