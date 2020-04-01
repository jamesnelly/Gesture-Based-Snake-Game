using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CodeMonkey;
using CodeMonkey.Utils;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType =  Thalmic.Myo.VibrationType;


public class Snake : MonoBehaviour
{

    public GameObject myo = null;

    // diiferent directions the snake can move in
    private enum Direction{
        Left, 
        Right, 
        Up, 
        Down
    }

    // the state of our snake dead or alive
    private enum State{
        Alive, 
        Dead
    }

    private State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition; 
    // this will conatin the time remaining until the next movement
    private float gridMoveTimer;
    // this will contain the amount of time between moves
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    // this will control the size of the snake body size
    private int snakeBodySize;
    // storing where the snake as been is postions
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    // recieving our level grid
    public void Setup(LevelGrid levelGrid){
        this.levelGrid = levelGrid;
    }
    
    
    private void Awake(){
        // initilization of the grid position
        gridPosition = new Vector2Int(30, 30);
        // move the snake on the grid every 0.1 of a second
        gridMoveTimerMax = .1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 60;
        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
    }


    private Pose _lastPose = Pose.Unknown;

    private void Update(){
        switch (state){
            case State.Alive:
            // handling our inputs
            HandleInput();
             // handling our movements
            HandleGridMovement();
            break;
            case State.Dead:
            break;
        }
        
    }
    private void HandleInput(){

         ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

         if(thalmicMyo.pose != _lastPose){
            _lastPose = thalmicMyo.pose;
         

        if (thalmicMyo.pose == Pose.FingersSpread){
            thalmicMyo.Vibrate (VibrationType.Medium);

            ExtendedUnlockAndNotifyUserAction (thalmicMyo);
      
            // Myo Gesture Fist will move the direction
            // of the snake up 
       } else if (thalmicMyo.pose == Pose.Fist){
            if (gridMoveDirection != Direction.Down){
                 // i cannot move down if  i am already moving up
                gridMoveDirection = Direction.Up;
            }
             ExtendedUnlockAndNotifyUserAction (thalmicMyo);
        }
            // Myo Gesture Double tap of fingers will move the direction
            // of the snake down 
         else if (thalmicMyo.pose == Pose.DoubleTap){
            if (gridMoveDirection != Direction.Up){
                 // i cannot move up if  i am already moving down
                gridMoveDirection = Direction.Down;
             }
              ExtendedUnlockAndNotifyUserAction (thalmicMyo);
        }
            // Myo Gesture wave-in with your hand will move the direction
            // of the snake to the left 
         else if (thalmicMyo.pose == Pose.WaveIn){
            if (gridMoveDirection != Direction.Right){
                 // i cannot move right if  i am already moving left
                gridMoveDirection = Direction.Left;
                }
                 ExtendedUnlockAndNotifyUserAction (thalmicMyo);
            }
            // Myo Gesture wave-out with your hand will move the direction
            // of the snake to the right 
         else if (thalmicMyo.pose == Pose.WaveOut){
            if (gridMoveDirection != Direction.Left){
                // i cannot move left if  i am already moving right
                gridMoveDirection = Direction.Right;
                }
            ExtendedUnlockAndNotifyUserAction (thalmicMyo);
         }
     
    }
  }
    private void HandleGridMovement(){
        // .deltatime contains the amount of time that has 
        // elapsed since the last update
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax){
            gridMoveTimer -= gridMoveTimerMax;


            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0) {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection){
                default:
                // grid movement direction going right
                case Direction.Right: gridMoveDirectionVector = new Vector2Int (+1, 0); break;
                // grid movement direction going left
                case Direction.Left:  gridMoveDirectionVector = new Vector2Int (-1, 0); break;
                // grid movement direction going up
                case Direction.Up:    gridMoveDirectionVector = new Vector2Int (0, +1); break;
                // grid movement direction going down
                case Direction.Down:  gridMoveDirectionVector = new Vector2Int (0, -1); break;
            }
            // increase grid position by gridmovedirection vector
            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            // checking to see if the snake ate some food
            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood){
                // Snake Ate Food, Grow Body
                snakeBodySize++;
                CreateSnakeBodyPart();

            }

            if (snakeMovePositionList.Count >= snakeBodySize +1){
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count -1);
            }
            // update the body
             UpdateSnakeBodyParts();
            // goes through every single snake body part
            // and checks for the postion of the body of the snake head
            // and if its the same as the SnakebodyPartGridPostion then game is over
            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList){
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if(gridPosition == snakeBodyPartGridPosition){
                    state = State.Dead;
                    GameHandler.SnakeDeath(); 
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            // this handles the postion of the snake head when moving 
            // the head is now always facing in the direction that you move him
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) +90);

           
        }
    }
    private void CreateSnakeBodyPart() {
        // adding to the list a new snake body part
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts() {
        // gives it its new postion
        for (int i = 0; i < snakeBodyPartList.Count; i++) {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int dir){
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;

    }
    // Getting our grid postion
    public Vector2Int GetGridPostion(){
        return gridPosition;
    }

    // this will return full list of postions being taken up by the snake head and body
    public List<Vector2Int> GetFullSnakeGridPostion(){
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList){
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    private class SnakeBodyPart{
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
    public SnakeBodyPart(int bodyIndex){
        GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            // creates the first body party and then gets added to the list and then the second 
            //body part and so on
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = snakeBodyGameObject.transform;
    }
    // this will relocate the body
    // this function will get the body parts 
    // to correctly rotate to where the snake is moving
    public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition){
        this.snakeMovePosition = snakeMovePosition;
        transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);
        float angle;
            switch (snakeMovePosition.GetDirection()) {
            default:
            // by default the snake is facing up 
            case Direction.Up: 
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 0; 
                    break;
                case Direction.Left: 
                    angle = 0 + 45; 
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Right: 
                    angle = 0 - 45; 
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                }
                break;
            case Direction.Down: 
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = 180; 
                    break;
                case Direction.Left: 
                    angle = 180 - 45;
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                case Direction.Right:
                    angle = 180 + 45; 
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Left:
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = +90; 
                    break;
                case Direction.Down:
                    angle = 180 - 45; 
                    transform.position += new Vector3(-.2f, .2f);
                    break;
                case Direction.Up: 
                    angle = 45; 
                    transform.position += new Vector3(-.2f, -.2f);
                    break;
                }
                break;
            case Direction.Right: 
                switch (snakeMovePosition.GetPreviousDirection()) {
                default: 
                    angle = -90; 
                    break;
                case Direction.Down:
                    angle = 180 + 45; 
                    transform.position += new Vector3(.2f, .2f);
                    break;
                case Direction.Up:
                    angle = -45; 
                    transform.position += new Vector3(.2f, -.2f);
                    break;
                }
                break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition(){
            // move postion of snake
            return snakeMovePosition.GetGridPosition();
        }

    }

    // handles one move postion from the snake
    private class SnakeMovePosition{

        private SnakeMovePosition previousSnakeMovePosition;
        // this for our grid postion
        private Vector2Int gridPosition; 
        // reference's direction enum 
        private Direction direction;
        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction){
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;

        }

        public Vector2Int GetGridPosition(){
            return gridPosition;
        }

        public Direction GetDirection(){
            return direction;
        }
        public Direction GetPreviousDirection() {
            if (previousSnakeMovePosition == null) {
                return Direction.Right;
            } else {
                return previousSnakeMovePosition.direction;
            }
        }

    }

    void ExtendedUnlockAndNotifyUserAction (ThalmicMyo myo){
        ThalmicHub hub = ThalmicHub.instance;

        if(hub.lockingPolicy == LockingPolicy.Standard){
            myo.Unlock (UnlockType.Timed);
        }
        myo.NotifyUserAction ();
    }

}