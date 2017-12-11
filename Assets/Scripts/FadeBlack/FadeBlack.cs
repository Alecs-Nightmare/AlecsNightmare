using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlack : MonoBehaviour {

    
    //https://www.youtube.com/watch?v=0HwZQt94uHQ&t=21s



    public Texture2D fadeOutTexture; //The texture that will overlay the screen
    public float fadeSpeed = 0.8f;
    private int drawDepth = -1000;//the texture's order in the draw hierarchy
    private float alpha = 1.0f;
    private int fadeDir = -1;//the direction to fade: in = -1 or out = 1

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);//force the number between 0 and 1 beacuse GUI.color usees alpha values between 1 and 0 
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);//set the alpha value
        GUI.depth = drawDepth;//make the black texture render on top(draw last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);//draw the texture to fit the entire screen area
    }

    //sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction) {
        fadeDir = direction;
        return (fadeSpeed);//return the fadeSpeed variable so it's easy to time Applcation.LoadLevel();
    }

    private void OnLevelWasLoaded()
    {
        //alpha = 1;
        BeginFade(-1);
    }
}
