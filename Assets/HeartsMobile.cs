using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeartsMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Helper.isMobile())
        {
            var desktop = transform.Find("Heart");
            if (desktop != null)
            {
                desktop.gameObject.SetActive(false);
            }
        }
        else
        {
            var mobile = transform.Find("HeartMobile");
            if (mobile != null)
            {
                mobile.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
