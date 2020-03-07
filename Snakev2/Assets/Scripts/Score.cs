using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score {

    public static event EventHandler OnHighScoreChanged;

    private static int score;
     public static void InitializeStatic(){
        OnHighScoreChanged = null;
        score = 0;
    }

    public static int GetScore(){
        return score;
    }
    
    public static void AddScore(){
        score += 100;
    }

    public static int GetHighScore(){
        return PlayerPrefs.GetInt("highscore", 0);
    }

    public static bool SetNewHighScore(){
        return SetNewHighScore(score);
    }

    public static bool SetNewHighScore(int score){
        int highscore = GetHighScore();
        if(score > highscore){
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            if (OnHighScoreChanged !=null) OnHighScoreChanged(null, EventArgs.Empty);
            return true;
        } else{
            return false;
        }
    }
}