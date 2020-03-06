using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour{

    private  static GameOverWindow instance;

    private void Awake(){
        instance = this;

        transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.GameScene);
        };
        
        Hide();
    }

    private void Show(bool isNewHighScore) {
        gameObject.SetActive(true);

        transform.Find("NewHighScoreText").gameObject.SetActive(isNewHighScore);
        //transform.Find("ScoreText").GetComponent<Text>().text = Score.GetScore().ToString();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public static void ShowStatic(bool isNewHighScore) {
        instance.Show(isNewHighScore);
    }
}
