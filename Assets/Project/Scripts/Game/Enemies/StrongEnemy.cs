using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongEnemy : Enemy {

	public Animator enemyAnimator;

	private Vector3 originalEnemyAnimatorPosition;

    void Start()
    {
        if (hidden)
        {
            gameObject.SetActive(false);
        }
    }

    public void StrongEnemyKilled(bool shouldDisplayIfHidden)
    {
        if (hidden && shouldDisplayIfHidden)
        {
            gameObject.SetActive(true);
            Awake();
        }
    }

    void Awake () {
		if (enemyAnimator != null )
		{
			originalEnemyAnimatorPosition = enemyAnimator.transform.localPosition;

			enemyAnimator.SetFloat("Forward", 0.3f);
		}
	}
	
	void LateUpdate () {
		if (enemyAnimator != null)
		{
			enemyAnimator.transform.localPosition = originalEnemyAnimatorPosition;
		}
	}
}
