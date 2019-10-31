using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticController : MonoBehaviour {

// #if UNITY_ANDROID && !UNITY_EDITOR
//     // private static readonly AndroidJavaObject Vibrator =
//     //     new AndroidJavaClass("com.unity3d.player.UnityPlayer")// Get the Unity Player.
//     //     .GetStatic<AndroidJavaObject>("currentActivity")// Get the Current Activity from the Unity Player.
//     //     .Call<AndroidJavaObject>("getSystemService", "vibrator");// Then get the Vibration Service from the Current Activity.
// #endif

    List<GameObject> treasures;
    public TreasureManager treasureManager;
    bool isVibrating;
    // Start is called before the first frame update
    void Start()
    {
        treasures = treasureManager.GetComponent<TreasureManager>().treasureObjs;
    }

    // static void KyVibrator()
    // {
    //     // Trick Unity into giving the App vibration permission when it builds.
    //     // This check will always be false, but the compiler doesn't know that.
    //     if (Application.isEditor) Handheld.Vibrate();
    // }


    // Update is called once per frame
    void Update()
    {
        if(!isVibrating && getClosestTreasure() <= 300.0f){
            isVibrating = true;
            print("vibrating");
            
            // Vibrate(200);
            Handheld.Vibrate();
        }
    }

    float getClosestTreasure(){
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach(GameObject t in treasures){
            float dist = Vector3.Distance(t.GetComponent<Transform>().position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return minDist;
    }

// #if UNITY_ANDROID && !UNITY_EDITOR
//     // public static void Vibrate(long milliseconds)
//     // {
//     //     print("vibrating");
//     //     Vibrator.Call("vibrate", milliseconds);
//     // }

//     // public static void Vibrate(long[] pattern, int repeat)
//     // {
//     //     Vibrator.Call("vibrate", pattern, repeat);
//     // }
// #endif
}
