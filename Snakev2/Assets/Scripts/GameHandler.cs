using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

// main container for all our preset scene objects
// entry point fot our game
public class GameHandler : MonoBehaviour {
    private static GameHandler instance;

    [SerializeField] private Snake snake;
    // storing our level grid
    private LevelGrid levelGrid;
    private void Awake(){
        instance = this;
       Score.InitializeStatic();

       Score.SetNewHighScore(100);
    }
    private void Start() {
        Debug.Log("GameHandler.Start");

        // instantiating our level grid 
        levelGrid = new LevelGrid(60, 60);
        // calling the snake setup function
        // passing in the level grid
        snake.Setup(levelGrid);
        // calling setup and passing in the snake reference
        levelGrid.Setup(snake);
    }
    // update is called once per frame
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            GameHandler.PauseGame();
        }
    }

    public static void SnakeDeath(){
       bool isNewHighScore = Score.SetNewHighScore();
        GameOverWindow.ShowStatic(isNewHighScore);
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