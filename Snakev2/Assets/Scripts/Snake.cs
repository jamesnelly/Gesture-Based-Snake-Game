using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition; 
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List <Vector2Int> snakeMovePositionList;
    public List <Transform> snakeBodyTransformList;


    public void Setup(LevelGrid levelGrid){
        this.levelGrid = levelGrid;
    }
    
    
    private void Awake(){
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = .2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize =0;
        snakeBodyTransformList = new List<Transform>();
    }

    private void Update(){
        HandleInput();
        HandleGridMovement();
    }
    private void HandleInput(){
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            if (gridMoveDirection.y != -1){
            gridMoveDirection.x = 0;
            gridMoveDirection.y = +1;
            }
        }
         if (Input.GetKeyDown(KeyCode.DownArrow)){
            if (gridMoveDirection.y != +1){
            gridMoveDirection.x = 0;
            gridMoveDirection.y = -1;
             }
        }
         if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (gridMoveDirection.x != +1){
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
                }
            }
         if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (gridMoveDirection.x != -1){
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
                }
         }
    }

    private void HandleGridMovement(){
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax){

            gridMoveTimer -= gridMoveTimerMax;
            snakeMovePositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood){
                // Snake Ate Food, Grow Body
                snakeBodySize++;
                CreateSnakeBody();

            }

            if (snakeMovePositionList.Count >= snakeBodySize +1){
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count -1);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) -90);

            for (int i = 0; i < snakeBodyTransformList.Count; i++){
                Vector3 snakeBodyPos = new Vector3 (snakeMovePositionList[i].x, snakeMovePositionList[i].y);
                snakeBodyTransformList[i].position = snakeBodyPos;
            }
        }
    }

    private void CreateSnakeBody(){
        GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
        snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
        snakeBodyTransformList.Add(snakeBodyGameObject.transform);
    }

    private float GetAngleFromVector(Vector2Int dir){
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    

    public Vector2Int GetGridPostion(){
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPostion(){
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }
}