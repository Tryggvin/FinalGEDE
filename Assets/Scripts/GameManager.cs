using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    public int Id = 0;
    public int score = 0;
    public bool isGameOver = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddToScore(int points)
    {
        score += points;
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}

