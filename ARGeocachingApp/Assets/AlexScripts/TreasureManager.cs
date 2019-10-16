using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using System.IO;

/*
To convert from latitude and longitude to game coordinates I think you use these:
var map = LocationProviderFactory.Instance.mapManager;
transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
*/

public class TreasureManager : Singleton<TreasureManager> {

  struct TreasureObj
  {
      public Vector3 gameCoords;
      public Vector2 LatLong;
      public bool found;
  }

  [SerializeField] private List<TreasureObj> treasurePoints = new List<TreasureObj>(); //NOTE: for LatLong, latitude = x-coordinate, longitude = y-coordinate
  //[SerializeField] private List<GameObject> treasures;
  private string path;

  public GameObject player;

  public GameObject treasure;

  Vector3 playerLocation;

  void Start()
  {
    path = string.Concat(Application.persistentDataPath, "/coordinates.txt");
      //call to pull data from database

      //instantiate treasure chests based on coordinates

      //toggle visibility of not found treasure chests

  }

	public IEnumerator PlaceTreasure()
  {
    #if UNITY_EDITOR
      TreasureObj e;
      e.found = false;
      // Access granted and location value could be retrieved
      e.LatLong.x = 60.19188f;
      e.LatLong.y = 24.9685822f;

      print("Latitude and Longitude: " + e.LatLong);

      e.gameCoords = new Vector3(playerLocation.x, playerLocation.y + 5, playerLocation.z);

      if(e.LatLong.x != -1 && e.LatLong.y != -1){
        treasurePoints.Add(e);
      }

      Instantiate(treasure, e.gameCoords, Quaternion.Euler(-90,0,0));
      yield break;

    #endif
    //This should only be called in the AR camera scene when an image has been recognized and treasure has been placed on it
    //Then, when we return to the map scene, we need to be sure to populate the map with the updated treasure info

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
        TreasureObj t;
        t.found = false;
        // Access granted and location value could be retrieved
        t.LatLong.x = Input.location.lastData.latitude;
        t.LatLong.y = Input.location.lastData.longitude;

        print("Latitude and Longitude: " + t.LatLong);

        t.gameCoords = new Vector3(playerLocation.x, playerLocation.y + 10, playerLocation.z);

        if(t.LatLong.x != -1 && t.LatLong.y != -1){
          treasurePoints.Add(t);
        }

        Instantiate(treasure, t.gameCoords, Quaternion.Euler(-90,0,0));
    }
  }

  public void AssignPlayerLocation() //call this method when transitioning to treasure placement/finding scenes to store the player's game world location
  {
    print("Trying to instantiate treasure");
    playerLocation = player.transform.position;

    StartCoroutine(PlaceTreasure()); //will have to move this elsewhere later
  }

  void WriteTreasureLocationsToFile()
  {
    Debug.Log("DATA PATH: "+ Application.persistentDataPath);
    StreamWriter writer = new StreamWriter(path);

    if (!File.Exists(path))
    {
      writer = File.CreateText(path);
    }

    for(int i = 0; i < treasurePoints.Count; i++)
    {
      writer.WriteLine(treasurePoints[i].LatLong);
    }

    writer.Close();
    ReadTreasureLocationsFromFile();
  }

  void ReadTreasureLocationsFromFile()
  {
    treasurePoints.Clear();
    StreamReader reader = new StreamReader(path);

    string text = " ";
    while(text != null)
    {
        text = reader.ReadLine();
        //Console.WriteLine(text);
        if(text != null)
        {
          print ("TEST " + text);
        }
    }
    reader.Close();
  }
}
