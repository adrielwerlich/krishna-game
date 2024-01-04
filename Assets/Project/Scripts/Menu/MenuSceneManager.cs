using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour {
    
    
    void Start()
    {
       
    }
    public void OnStart () {
		SceneManager.LoadScene ("Level1");
	}

    public void StartKrishnaGame()
    {
        SceneManager.LoadScene(1);
    }
}
