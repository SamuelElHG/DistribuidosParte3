using Firebase.Database;
using ServerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankController : MonoBehaviour
{
    DatabaseReference reference;
    private void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateRank(string rank, string name, string userID)
    {
        UserRank user = new UserRank(name, 0);
        string json = JsonUtility.ToJson(user);

        reference.Child("ranks").Child(rank).Child(userID).SetRawJsonValueAsync(json); //remember no work with email
    }
    public void RemoveRank(string rank, string userID)
    {
        reference.Child("ranks").Child(rank).Child(userID).RemoveValueAsync();
    }

    public void AddRankNumber(int numberRank, string rank, string name, string userID)
    {
        UserRank user = new UserRank(name, numberRank);
        string json = JsonUtility.ToJson(user);

        reference.Child("ranks").Child(rank).Child(userID).SetRawJsonValueAsync(json); //remember no work with email
    }
}