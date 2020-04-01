using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class will be used to easily
// reference assests from code 
public class GameAssets : MonoBehaviour
{
    //static variable for our instance
    public static  GameAssets i;

    // we can access all the public fields through
    // this static reference
    private void Awake(){
        i = this;
    }
    // Snake head sprite reference
   public Sprite snakeHeadSprite;
   // Snake head sprite reference
   public Sprite snakeBodySprite;
   // food sprite reference
   public Sprite foodSprite;
}
