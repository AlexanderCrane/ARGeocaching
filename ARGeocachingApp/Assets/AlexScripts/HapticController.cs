using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticController : Singleton<HapticController> {

    public static AndroidJavaClass unityPlayer = null;
    public static AndroidJavaObject currentActivity = null;
    public static AndroidJavaObject vibrator = null;

    List<TreasureManager.TreasureObj> treasures;
    public TreasureManager treasureManager;
    bool isVibrating;

    bool isScanning;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize(){
    #if UNITY_ANDROID && !UNITY_EDITOR
        if (Application.platform == RuntimePlatform.Android) {
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            }
    #endif
    }

    // Start is called before the first frame update
    void Start()
    {
        Handheld.Vibrate(); // This tricks Unity into giving vibration priviledges.
        treasures = treasureManager.GetComponent<TreasureManager>().treasurePoints;
    }

    public void setScanning(){
        isScanning = !isScanning;

        print("isScanning: " + isScanning);

        if(!isScanning){
            isVibrating = false;
            Cancel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isScanning && !isVibrating && getClosestTreasure() <= 30.0f){
            isVibrating = true;
            print("vibrating");
            long[] la = {0, 400, 200, 400};
            Vibrate(la, 1);
            
        } else if(isScanning && !isVibrating && getClosestTreasure() <= 50.0f){
            isVibrating = true;
            print("vibrating");
            long[] la = {0, 600, 400, 600};
            Vibrate(la, 1);

        } else if(isScanning && !isVibrating && getClosestTreasure() <= 70.0f){
            isVibrating = true;
            print("vibrating");
            long[] la = {0, 1200, 600, 1200};
            Vibrate(la, 1);
        } else if(isScanning && isVibrating && getClosestTreasure() > 70.0f) {
            isVibrating = false;
        }
    }

    float getClosestTreasure(){
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach(TreasureManager.TreasureObj t in treasures){
            float dist = Vector3.Distance(t.treasure.GetComponent<Transform>().position, currentPos);
            if (dist < minDist)
            {
                tMin = t.treasure;
                minDist = dist;
            }
        }
        return minDist;
    }

    public static void Vibrate(long milliseconds)
    {
        if(isAndroid() && vibrator != null){
            print("vibrating");
            vibrator.Call("vibrate", milliseconds);
        } else {
            Handheld.Vibrate();
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if(isAndroid() && vibrator != null){
            print("vibrating");
            vibrator.Call("vibrate", pattern, repeat);
        } else {
            Handheld.Vibrate();
        }
    }

    public static bool HasVibrator()
    {
        return vibrator.Call<bool>("hasVibrator");
    }

    public static void Cancel()
    {
        if (isAndroid()){
            vibrator.Call("cancel");
        }
    }
    
    private static bool isAndroid()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
	    return true;
    #else
        return false;
    #endif
    }
}
