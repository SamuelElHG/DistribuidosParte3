using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Game UI")]
   // [SerializeField] GameObject ball;
    public TextMeshProUGUI scoreText;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject gameplay;

    [Header("Menu")]
    [SerializeField] GameObject menuUI;

    [Header("Scoreboard")]
    [SerializeField] GameObject scoreboardUI;

    [Header("friends")]
    [SerializeField] GameObject friendsUI;

    [Header("conected")]
    [SerializeField] GameObject connectedUI;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
    }

    public void UpdateScore()
    {
        scoreText.text = scoreManager.score.ToString();
    }

    public void PlayButton()
    {
        gameUI.SetActive(true);
        menuUI.SetActive(false);       
        gameplay.SetActive(true);
    }

    public void ShowScoreboard()
    {
        scoreboardUI.SetActive(true);
        menuUI.SetActive(false);
        //ball.SetActive(false);
    }

    public void BackToMenu()
    {
        menuUI.SetActive(true);
        //ball.SetActive(true);
        scoreboardUI.SetActive(false);
        connectedUI.SetActive(false);
        friendsUI.SetActive(false);
    }
    public void ShowConnected()
    {
        connectedUI.SetActive(true);
        friendsUI.SetActive(false);
        //ball.SetActive(false);
    }
    public void ShowFriends()
    {
        connectedUI.SetActive(false);
        friendsUI.SetActive(true);
        //ball.SetActive(false);
    }


}
