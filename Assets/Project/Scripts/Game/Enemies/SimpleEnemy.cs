using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Enemy {

    public override string Type
    {
        get { return "simple"; }
        set { }
    }

    // Use this for initialization
    void Start () {
        if (hidden)
        {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SimpleEnemyKilled(bool shouldDisplayIfHidden)
    {
        if (hidden && shouldDisplayIfHidden)
        { 
            gameObject.SetActive(true);
        }
    }

    public override void Hit(string type, string origin, int multiplier = 1)
    {
		base.Hit(type, origin, multiplier);
	}
}
