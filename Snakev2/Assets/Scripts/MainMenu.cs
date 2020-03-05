using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MainMenu : MonoBehaviour
{
    private enum Sub {
         Main,
         HowToPlay,
    }
   private void Awake(){
    transform.Find("howToPlaySubMenu").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    transform.Find("mainSub").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

    transform.Find("mainSub").Find("PlayBtn").GetComponent<Button_UI>().ClickFunc = () => Loader.Load(Loader.Scene.GameScene);

    transform.Find("mainSub").Find("QuitBtn").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();

    transform.Find("mainSub").Find("HowToPlayBtn").GetComponent<Button_UI>().ClickFunc = () => ShowSub(Sub.HowToPlay);

    ShowSub(Sub.Main);

   }

   private void ShowSub(Sub sub){
       transform.Find("mainSub").gameObject.SetActive(false);
       transform.Find("howToPlaySubMenu").gameObject.SetActive(false); 

       switch (sub) {
           case Sub.Main:
                transform.Find("mainSub").gameObject.SetActive(true);
                break;
            case Sub.HowToPlay:
            transform.Find("howToPlaySubMenu").gameObject.SetActive(true);
                break;


       }

   }
}
