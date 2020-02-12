using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class LevelGrid : MonoBehaviour
{
    private Vector2Int FoodPostion;
    private GameObject foodGameObject;
    private int width;
    private int height;
    private Snake snake;


    public LevelGrid(int width, int height){
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake){
        this.snake = snake;

         SpawnFood();
    }
    private void SpawnFood(){

        do{
             FoodPostion = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetGridPosition() == FoodPostion);
       
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(FoodPostion.x, FoodPostion.y);

    }

    public void SnakeMoved(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == FoodPostion) {
            Destroy(foodGameObject, 0.3F);
            SpawnFood();
            CMDebug.TextPopupMouse("Snake eating food");
        }
    }
}