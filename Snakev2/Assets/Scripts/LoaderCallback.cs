using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool firstUpdate = true;
    private void Update()
    {
        // calling the loaderCallback function
        if (firstUpdate){
            firstUpdate = false;
            Loader.LoaderCallback();
        } 
    }
}
