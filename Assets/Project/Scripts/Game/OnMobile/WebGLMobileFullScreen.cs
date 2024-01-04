using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLMobileFullScreen : MonoBehaviour
{

    #region WebGL request full screen
    [DllImport("__Internal")]
    private static extern bool RequestFullScreen();
    public bool GetFullScreen()
    {
        Debug.Log("unity get full screen");
#if !UNITY_EDITOR && UNITY_WEBGL
	        return RequestFullScreen();
#endif

        return false;
    }
    #endregion

    #region WebGL exit full screen
    [DllImport("__Internal")]
    private static extern bool ExitFullScreen();
    public bool FullScreenOff()
    {
        Debug.Log("unity get landscape mode");
#if !UNITY_EDITOR && UNITY_WEBGL
	        return ExitFullScreen();
#endif

        return false;
    }
    #endregion

    void Start()
    {
        if (Application.isMobilePlatform)
        {
            Debug.Log("this should only appear in mobile. GetFullScreen");
            GetFullScreen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            Debug.Log("menu script. FullScreenOff? =>" + FullScreenOff());
        }
    }
}
