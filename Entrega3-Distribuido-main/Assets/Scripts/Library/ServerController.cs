using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Google.MiniJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ServerController{

    public class FirebaseUser : MonoBehaviour
    {
        //private variables
        Firebase.Auth.FirebaseAuth auth;
        Firebase.Auth.FirebaseUser user;
        private bool isSignIn = false;

        #region unityFuntions
        public void StartServerProfile()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    InitializeFirebase();

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                    // Firebase Unity SDK is not safe to use here.
                }
            });
            //WriteNewScore("copito", 4);
        }

        private bool isSinged = false;

        public bool IsSignIn { get => isSignIn; }

        public void UpdateServerProfile(TMP_Text profileUserName)
        {
            if (isSinged)
            {
                if (!isSignIn)
                {
                    isSignIn = true;
                    profileUserName.text = "" + user.DisplayName;
                }
            }
        }

        public void OnDestroyServerProfile()
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
        #endregion

        #region publicFuntions
        public void LoginUser(TMP_InputField loginEmail, TMP_InputField loginPassword)
        {
            if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
            {
                return;
            }

            //Do login

            auth.SignInWithEmailAndPasswordAsync(loginEmail.text, loginPassword.text).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                    return;
                }

                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
            });
        }
        public void LogOutUser()
        {
            auth.SignOut();
            isSignIn = false;
        }

        public void RegisterUser(TMP_InputField registerName, TMP_InputField registerEmail, TMP_InputField registerPassword, TMP_InputField registerConfirmPassword)
        {
            if (string.IsNullOrEmpty(registerName.text) && string.IsNullOrEmpty(registerEmail.text) && string.IsNullOrEmpty(registerPassword.text) && string.IsNullOrEmpty(registerConfirmPassword.text))
            {
                return;
            }

            //Do register
            CreateUser(registerEmail.text, registerPassword.text, registerName.text);
        }

        public void ForgotPassword_SubmitData(TMP_InputField ForgotPasswordEmail)
        {
            if (string.IsNullOrEmpty(ForgotPasswordEmail.text))
            {
                return;
            }

            auth.SendPasswordResetEmailAsync(ForgotPasswordEmail.text).ContinueWithOnMainThread(Task =>
            {
                if (Task.IsCanceled)
                {
                    Debug.Log("SendPasswordResetEmailAsync is canceled");
                    return;
                }
                if (Task.IsFaulted)
                {
                    return;
                }
                Debug.Log("creo que lo explote xd");
            });
        }
        #endregion

        #region privateFuntions
        void CreateUser(string email, string password, string userName)
        {
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);

                UpdateUserProfile(userName);
            });
        }

        void InitializeFirebase()
        {
            print($"Configurando autorización de Firebase");
            auth = FirebaseAuth.DefaultInstance;
        }

        void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            if (auth.CurrentUser != user)
            {
                bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                    && auth.CurrentUser.IsValid();
                if (!signedIn && user != null)
                {
                    Debug.Log("Signed out " + user.UserId);
                }
                user = auth.CurrentUser;
                if (signedIn)
                {
                    Debug.Log("Signed in " + user.UserId);
                    isSignIn = true;
                }
            }
        }

        void UpdateUserProfile(string userName)
        {
            Firebase.Auth.FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    DisplayName = userName,
                };
                user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Debug.Log("User profile updated successfully.");
                    isSignIn = true;
                });
            }
        }
        #endregion
    }

    //SetValueAsync and list for create a data base

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

    public class RankController : MonoBehaviour
    {
        DatabaseReference reference;
        public void StartRankController()
        {
            // Get the root reference location of the database.
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

    public class FriendsController : MonoBehaviour
    {
        DatabaseReference reference;
        public void StartFriendsController()
        {
            // Get the root reference location of the database.
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void AddNewUser(string userID, string name)
        {
            User user = new User(name, true);
            string json = JsonUtility.ToJson(user);

            reference.Child("users").Child(userID).SetRawJsonValueAsync(json);
            reference.Child("users").Child(userID).Child("Friends").Child("FriendsCount").SetRawJsonValueAsync("0");
            reference.Child("users").Child(userID).Child("FriendsNotification").Child("FriendsNotificationCount").SetRawJsonValueAsync("0");
            reference.Child("users").Child(userID).Child("ConectedNotification").Child("ConectedNotificationCount").SetRawJsonValueAsync("0");
        }

        public User UpdateUser(string userID)
        {
            User thisUser = new User("", true);

            FirebaseDatabase.DefaultInstance
            .GetReference("users").Child(userID).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.Log("No se Completo la busqueda");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Child("Name").Value != null)
                    {
                        thisUser.ConectedNotification = new string[(int)snapshot.Child("ConectedNotification").Child("ConectedNotificationCount").Value];
                        thisUser.Friends = new string[(int)snapshot.Child("Friends").Child("FriendsCount").Value];
                        thisUser.FriendsNotification = new string[(int)snapshot.Child("FriendsNotification").Child("FriendsNotificationCount").Value];

                        for (int i = 0; i < thisUser.ConectedNotification.Length; i++)
                        {
                            thisUser.ConectedNotification[i] = snapshot.Child("ConectedNotification").Child("NotificationNumber" + i).Value.ToString();
                        }
                        for (int i = 0; i < thisUser.Friends.Length; i++)
                        {
                            thisUser.Friends[i] = snapshot.Child("Friends").Child("friendNumber" + i).Value.ToString();
                        }
                        for (int i = 0; i < thisUser.FriendsNotification.Length; i++)
                        {
                            thisUser.FriendsNotification[i] = snapshot.Child("ConectedNotification").Child("ConectedNotificationNumber" + i).Value.ToString();
                        }

                        thisUser.Name = snapshot.Child("Name").Value.ToString();
                        thisUser.isConected = (bool)snapshot.Child("isConected").Value;
                    }
                    else
                    {
                        Debug.Log("No se encontro ningun usuario");
                    }
                }
            });

            return thisUser;
        }

        public void AcceptFriend(string myUserID, string userIDToSearch, int numberNotification)
        {
            if (myUserID != userIDToSearch)
            {
                FirebaseDatabase.DefaultInstance
            .GetReference("users").Child(userIDToSearch).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.Log("No se Completo la busqueda");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Child("Name").Value != null)
                    {
                        int myCountFriends = GetCount(myUserID, "Friends");
                        int otherCountFriends = GetCount(userIDToSearch, "Friends");
                        reference.Child("users").Child(myUserID).Child("Friends").Child("friendNumber" + myCountFriends).SetRawJsonValueAsync(userIDToSearch);
                        reference.Child("users").Child(myUserID).Child("Friends").Child("FriendsCount").SetRawJsonValueAsync((myCountFriends + 1).ToString());
                        reference.Child("users").Child(userIDToSearch).Child("Friends").Child("friendNumber" + otherCountFriends).SetRawJsonValueAsync(myUserID);
                        reference.Child("users").Child(userIDToSearch).Child("Friends").Child("FriendsCount").SetRawJsonValueAsync((otherCountFriends + 1).ToString());
                        RemoveFriendRequest(myUserID, numberNotification);
                    }
                    else
                    {
                        Debug.Log("No se encontro ningun usuario");
                    }
                }
            });
            }
            else
            {
                Debug.Log("No te puedes agregar a ti mismo");
            }
        }

        public void SendFriendRequest(string myUserID, string otherUserID)
        {
            if (myUserID != otherUserID)
            {
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
                            int myCountFriendsNotification = GetCount(otherUserID, "FriendsNotification");
                            reference.Child("users").Child(otherUserID).Child("FriendsNotification").Child("FriendsNotificationNumber" + myCountFriendsNotification + 1).SetRawJsonValueAsync(myUserID);
                            reference.Child("users").Child(otherUserID).Child("FriendsNotification").Child("FriendsNotificationCount").SetRawJsonValueAsync((myCountFriendsNotification + 1).ToString());
                        }
                        else
                        {
                            Debug.Log("No se encontro ningun usuario");
                        }
                    }
                });
            }
            else
            {
                //do this if the user no 
            }
        }

        public void RemoveFriendRequest(string userID, int numberNotification)
        {
            FirebaseDatabase.DefaultInstance.GetReference("users").Child(userID).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.Log("No se Completo la busqueda");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Child("Name").Value != null)
                    {
                        int myCountFriendsNotification = GetCount(userID, "FriendsNotification");
                        reference.Child("users").Child(userID).Child("FriendsNotification").Child("FriendsNotificationNumber" + numberNotification).RemoveValueAsync();
                        reference.Child("users").Child(userID).Child("FriendsNotification").Child("FriendsNotificationCount").SetRawJsonValueAsync((myCountFriendsNotification - 1).ToString());
                    }
                    else
                    {
                        Debug.Log("No se encontro ningun usuario");
                    }
                }
            });
        }

        private int GetCount(string userID, string _nameCount)
        {
            int count = 0;
            FirebaseDatabase.DefaultInstance
            .GetReference("users").Child(userID).GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.Log("No se Completo la busqueda");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if (snapshot.Child("Name").Value != null)
                    {
                        count = (int)snapshot.Child(_nameCount).Child(_nameCount + "Count").Value;
                    }
                    else
                    {
                        Debug.Log("No se encontro ningun usuario");
                    }
                }
            });
            return count;
        }
    }
}
