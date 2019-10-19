using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadMapScene(){
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void LoadBuryScene(){
        SceneManager.LoadScene("BuryTreasure", LoadSceneMode.Single);
    }

    public void LoadSearchScene(){
        SceneManager.LoadScene("SearchForTreasure", LoadSceneMode.Single);
    }
}
