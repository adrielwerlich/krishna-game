using System;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InputManager.ToggleShowGameMenu += DisplayGameMenu;
        InputManager.ReloadLevel += ReloadLevel;

        //Debug.Log("game menu started");
        gameObject.SetActive(false);
    }

    private void ReloadLevel()
    {
        InputManager.ToggleShowGameMenu -= DisplayGameMenu;
        //Debug.Log("game menu reload level");
    }


    private void OnDestroy()
    {
        InputManager.ToggleShowGameMenu -= DisplayGameMenu;

        //Debug.Log("game menu destroyed");

    }

    private void DisplayGameMenu(bool isGameMenuActive)
    {
        if (gameObject != null) { 
            gameObject.SetActive(isGameMenuActive);
        }
        //Debug.Log("game menu display");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
