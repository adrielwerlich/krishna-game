using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideParticlesOnMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Helper.isMobile())
        {
            var ps = transform.Find("Particle System");
            if (ps != null)
            {
                ps.gameObject.SetActive(false);
            }
        }
        //Debug.Log("bomb particle system => " + transform.Find("Particle System"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
