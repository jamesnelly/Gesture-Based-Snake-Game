using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    [SerializeField] private Snake snake;
    private LevelGrid levelGrid;
    private void Start() {
        Debug.Log("GameHandler.Start");


        levelGrid = new LevelGrid(18,18);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }

}