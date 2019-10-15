using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureManager : Singleton<TreasureManager> {

    [SerializeField] private List<Vector2> treasurePoints; //NOTE: latitude = x-coordinate, longitude = y-coordinate
    [SerializeField] private List<GameObject> treasures;

    IEnumerator Start()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
	IEnumerator PlaceTreasure()
  {
    //This should only be called in the AR camera scene when an image has been recognized and treasure has been placed on it
    //Then, when we return to the map scene, we need to be sure to populate the map with the updated treasure info

    Vector2 LatLong = new Vector2(-1,-1);

    if (!Input.location.isEnabledByUser)
        yield break;

    // Start service before querying location
    Input.location.Start();

    // Wait until service initializes
    int maxWait = 20;
    while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
    {
        yield return new WaitForSeconds(1);
        maxWait--;
    }

    // Service didn't initialize in 20 seconds
    if (maxWait < 1)
    {
        print("Timed out");
        yield break;
    }

    // Connection has failed
    if (Input.location.status == LocationServiceStatus.Failed)
    {
        print("Unable to determine device location");
        yield break;
    }
    else
    {
        // Access granted and location value could be retrieved
        LatLong.x = Input.location.lastData.latitude;
        LatLong.y = Input.location.lastData.longitude;

        print("Vector2 Location: " + LatLong);

        if(LatLong.x != -1 && LatLong.y != -1){
          treasurePoints.Add(LatLong);
        }
    }
  }
}
