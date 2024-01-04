using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class Enemy : MonoBehaviour {
	public int health = 1;
    public bool hidden = false;

	private bool IsGrounded = true;

    public virtual string Type { get; set; }

	public static event Action<string, int> InformPlayer;
	public static event Action<Transform> EnemyKilled;

	public UnityEvent<bool> ActivateDisabledEnemy;

	//[SerializeField] private bool isKrishnaGame = false;

    private void OnEnable()
    {
		if (this.gameObject != null && this.gameObject.activeSelf)
		{
			Helper.EnemyCount++;
			gameObject.tag = "Enemy";
		}
		//Debug.Log("enemy counter => " + Helper.EnemyCount);
    }

    public virtual void Hit(string type, string origin, int multiplier = 1)
    {
		if (multiplier > 1) 
		{ 
			health -= 1 * multiplier;
		} 
		else
		{
			health--;
		}
		if (health <= 0) {
			//EffectManager.Instance.ApplyEffect (transform.position, EffectManager.Instance.killEffectPrefab);
			//Debug.Log("destroy enemy type " + type);
			if (EnemyKilled != null)
			{
				EnemyKilled.Invoke(this.transform);
			}

			Destroy (gameObject);
			ActivateDisabledEnemy.Invoke(true);

			string enemyType = null;
			int experienceGain = 0;
            if (type == "simple")
			{
				enemyType = "simple";
				experienceGain = origin == "arrow" ? 2 : 1;
            }

			if (enemyType != null && experienceGain > 0 && InformPlayer != null)
			{
				InformPlayer?.Invoke(enemyType, experienceGain);
			}

			Helper.EnemyCount--;
			//Debug.Log("Enemy killed. enemy count decreased => " + Helper.EnemyCount);
			if (Helper.EnemyCount <= 0)
			{
				// show success message
				
			}

        } else {
			//EffectManager.Instance.ApplyEffect (transform.position, EffectManager.Instance.hitEffectPrefab);
		}
	}


    public void OnTriggerEnter (Collider otherCollider) {
		//Debug.Log("OnTriggerEnter enemy " + otherCollider.name);
		//Debug.Log("OnTriggerEnter enemy tag" + otherCollider.tag);
		//if (otherCollider.GetComponent<Arrow> () != null) {
			//Hit ();
			//Destroy (otherCollider.gameObject);
		//}
	}

    private void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(4, 6, 9);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
  //      Debug.Log("OnTriggerEnter enemy " + collision.gameObject.name);
		//bool isEnemy = collision.gameObject.GetComponent<Enemy>() != null;

  //      if (isEnemy || collision.gameObject.name == "Floor")
		//{
		//	IsGrounded = true;
		//} else
		//{
		//	IsGrounded = false;
  //      }

        if (collision.gameObject.tag == "Arrow")
		{
            Hit(this.Type, "arrow");
			Destroy(collision.gameObject);
		}

    }

    public void OnTriggerStay (Collider otherCollider) {
		if (otherCollider.GetComponent<Sword> () != null) {
			if (otherCollider.GetComponent<Sword> ().JustAttacked) {
				//Hit(this.Type);
			}
		}
	}
}
