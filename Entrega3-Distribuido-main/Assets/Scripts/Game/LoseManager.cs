using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LoseManager : MonoBehaviour
{
    int score;
    [HideInInspector] public int highScore;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] Transform ballInitialPosition;
    [SerializeField] GameObject menuUI;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text scoretext;

    [SerializeField] UIController scoreText;
    [SerializeField] AuthManager authManager;
    [SerializeField] GameObject objectsOfGame;
    public UnityEvent updateAuth;

    private void OnTriggerExit2D(Collider2D collision) //gameover
    {
        if (collision.gameObject.CompareTag("Balls"))
        {
            score = scoreManager.score;
            if (score > highScore)
            {
                highScore = score;
                highScoreText.text = highScore.ToString();
                updateAuth.Invoke();
            }
            scoretext.text = score.ToString();
            score = 0;
            ReloadScene();
            gameover();
        }
    }

    private void ReloadScene()
    {
        scoreManager.DeleteScore();
      //  scoreText.scoreText.text = "0";

        menuUI.SetActive(true);
    }
    private void gameover()
    {

        objectsOfGame.SetActive(false);
    }
}
