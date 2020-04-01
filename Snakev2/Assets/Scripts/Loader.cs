using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene{
        GameScene,
        Loading,
        MainMenu,
    }

    private static Action loaderCallbackAction;
    // Recieving a scene
    public static void Load(Scene scene){
        // this action will only be trigger after the loading Scene is loaded
        loaderCallbackAction = () => {
            //Load this scene when the loading scene is complete

        SceneManager.LoadScene(scene.ToString());
        };

        SceneManager.LoadScene(Scene.Loading.ToString()); 
    }
    public static void LoaderCallback(){
        if (loaderCallbackAction != null){
            loaderCallbackAction();
            loaderCallbackAction = null;
        } 

    }

}