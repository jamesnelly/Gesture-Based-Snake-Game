using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {
    private static GameHandler instance;

    [SerializeField] private Snake snake;
    private LevelGrid levelGrid;
    private void Awake(){
        instance = this;
       // Score.InitializeStatic();

       //Score.SetNewHighScore(100);
    }
    private void Start() {
        Debug.Log("GameHandler.Start");


        levelGrid = new LevelGrid(60, 60);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);


       //CMDebug.ButtonUI(Vector2.zero, "Reload Scene", () => {
            //Loader.Load(Loader.Scene.GameScene);
      // });
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            GameHandler.PauseGame();
        }
    }

    public static void SnakeDeath(){
       //bool isNewHighScore = Score.SetNewHighScore();
        //GameOverWindow.ShowStatic(isNewHighScore);
        ScoreWindow.HideStatic();
    }

    public static void ResumeGame(){
        PauseWindow.HideStatic();
         Time.timeScale = 1f;
    }

    public static void PauseGame(){
        PauseWindow.ShowStatic();
        Time.timeScale = 0f;
    }

}