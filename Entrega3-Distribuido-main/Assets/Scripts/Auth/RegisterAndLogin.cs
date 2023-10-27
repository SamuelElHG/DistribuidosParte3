using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerController;
using TMPro;

public class RegisterAndLogin : MonoBehaviour
{
    ServerController.FirebaseUser firebaseUser;
    [SerializeField] TMP_Text profileUserName;

    [Header("Login")]
    [SerializeField] TMP_InputField emailLogin;
    [SerializeField] TMP_InputField passwordLogin;

    [Header("Register")]
    [SerializeField] TMP_InputField nameRegister;
    [SerializeField] TMP_InputField emailRegister;
    [SerializeField] TMP_InputField passwordRegister;
    [SerializeField] TMP_InputField confirmPasswordRegister;

    [Header("Forgot Password")]
    [SerializeField] TMP_InputField emailForgotPassword;

    #region unity Funtions
    private void Start()
    {
        firebaseUser = (ServerController.FirebaseUser)FindObjectOfType(typeof(ServerController.FirebaseUser));
    }

    private void Update()
    {
        firebaseUser.UpdateServerProfile(profileUserName);
    }

    private void OnDestroy()
    {
        firebaseUser.OnDestroyServerProfile();
    }
    #endregion

    public void Login()
    {
        firebaseUser.LoginUser(emailLogin, passwordLogin);
    }

    public void Register()
    {
        firebaseUser.RegisterUser(nameRegister, emailRegister, passwordRegister, confirmPasswordRegister);
    }

    public void ForgotPassword()
    {
        firebaseUser.ForgotPassword_SubmitData(emailForgotPassword);
    }
}
