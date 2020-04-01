using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class LevelGrid
{
    // postion where the food spwans
    private Vector2Int foodGridPosition;
    // reference of the game object food
    private GameObject foodGameObject;
    // width of grid
    private int width;
    //Height of grid
    private int height;
    // snake object reference
    private Snake snake;

    // constructor for the level grid
    public LevelGrid(int width, int height){
        this.width = width;
        this.height = height;
    }
    // passing sname into the level grid
    public void Setup(Snake snake){
        this.snake = snake;

         SpawnFood();
    }

    //function to spawn food
    private void SpawnFood(){

        do{
            // random postion for food to spawn
             foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
             // this code will run only if the food spawns on the snake then 
             // randomize the food once again 
        } while (snake.GetFullSnakeGridPostion().IndexOf(foodGridPosition) != -1);
       // new game object that is food with the sprite render componenet
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        // referencing this game object in the game assests class 
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        // locating the game on the grid and giving it the postions
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);

    }
    // checking if the snake has moved 
    // also this will return true if the snake has eaten food and false if not
   public bool TrySnakeEatFood (Vector2Int snakeGridPosition)
    {       // checking if the snake is in the same postion as the food 
        if(snakeGridPosition == foodGridPosition) {
            //destroy the current game object
            Object.Destroy(foodGameObject);
            // spawn food once again
            SpawnFood();
            Score.AddScore();
            return true;
        } else {
            return false;
        }
    }
    
    // setting up our grid so 
    // the snake can wrap from the left to right and vice versa
    // and the snake can wrap from the top to the bottom and vice versa
    public Vector2Int ValidateGridPosition(Vector2Int gridPosition){
        if (gridPosition.x < 0){
            gridPosition.x = width -1;
        }
        if (gridPosition.x > width -1){
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0){
            gridPosition.y = height -1;
        }
        if (gridPosition.y > height -1){
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}