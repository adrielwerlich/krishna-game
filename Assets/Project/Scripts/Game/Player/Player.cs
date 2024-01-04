using DigitalRuby.LightningBolt;
using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject model;
    // the string argument indicates what area of the map the player hit
    public static event Action<string> PlayerInArea;

    [Header("Movement")]
	[SerializeField] private float movingVelocity;
	[SerializeField] private float jumpingVelocity;
	[SerializeField] private float knockbackForce;


	[Header("Equipment")]
	//public int health = 5;
	//public Sword sword;
	public Bow bow;
	public GameObject quiver;
	public int arrows = 15;
	public GameObject bombPrefab;
	public int bombAmount = 5;
	public float throwingSpeed;
	public int orbAmount = 0;
	public static bool inAudioInteraction = false;

    [SerializeField] private GameObject simpleLightningPrefab;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private int energyLevel = 500;

    public int experience = 0;
    public static PlayerMoney money;


    private Rigidbody playerRigidbody;
	private bool canJump;
	private Quaternion targetModelRotation;
	private float knockbackTimer;
	private bool justTeleported;
	private Dungeon currentDungeon;
    private AudioSource _audioSource;

    private Health playerHealth;


	public bool JustTeleported {
		get {
			bool returnValue = justTeleported;
			justTeleported = false;
			return returnValue;
		}
	}

	public Dungeon CurrentDungeon {
		get {
			return currentDungeon;
		}
	}
    private static bool hasWeaponsMantra = false;
    private static bool hasHealingMantra = false;

    public static bool HasWeaponsMantra { 
		get => hasWeaponsMantra; 
		set => hasWeaponsMantra = value; 
	}
    public static bool HasHealingMantra { 
		get => hasHealingMantra; 
		set => hasHealingMantra = value; 
	}

    // Use this for initialization
    void Start () {
        money = this.gameObject.GetComponent<PlayerMoney>();

		if (money != null )
		{
	        money.Amount = 0;
		}
		bow.gameObject.SetActive (true);
		quiver.gameObject.SetActive (true);

		//sword.gameObject.SetActive (false);
        playerRigidbody = GetComponent<Rigidbody>();


        Enemy.InformPlayer += OnEnemyKilledIncreaseExperience;

        InputManager.FireArrow += FireArrow;
        InputManager.FireBomb += ThrowBomb;
        InputManager.FireLightning += FireLightning;

		playerHealth = GetComponent<Health>();

        _audioSource = GetComponent<AudioSource>();
		if ( _audioSource != null )
		{
	        _audioSource.Stop();
		}

        ScrollInteractionManager.PlayMantra += PlayTheMantra;
		KrishnaInteractionManager.PlayMantra += PlayTheMantra;
		BuyMachineInteractionManager.PlayBuyLine += PlayBuyLine;

    }

    private void PlayBuyLine(Helper.BuyLineIndex lineIndex)
    {
        PlayClip(Helper.getBuyLines()[((int)lineIndex)]);
    }

    private void PlayTheMantra(Helper.MantraIndex mantraIndex)
    {
        PlayClip(Helper.getMantras()[((int)mantraIndex)]);
    }

    private void PlayClip(AudioClip clip)
    {
        if (inAudioInteraction)
        {
            _audioSource.Stop();
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }

    private void OnDisable()
    {
        InputManager.FireArrow -= FireArrow;
        InputManager.FireBomb -= ThrowBomb;
        InputManager.FireLightning -= FireLightning;
        ScrollInteractionManager.PlayMantra -= PlayTheMantra;
        KrishnaInteractionManager.PlayMantra -= PlayTheMantra;
        BuyMachineInteractionManager.PlayBuyLine -= PlayBuyLine;

    }


    // Update is called once per frame
    void Update () {

	}

    private void FireArrow()
    {

        if (arrows > 0 && !inAudioInteraction)
        {
            //sword.gameObject.SetActive(false);
            //bow.gameObject.SetActive(true);
            bow.Attack();
            arrows--;
        }
    }

	private void FireLightning()
	{
		GameObject lightning = null;

		if (energyLevel > 100)
		{
			lightning = Instantiate(lightningPrefab);
		}
		else if (energyLevel > 0)
		{
			lightning = Instantiate(simpleLightningPrefab);
		}

		if (lightning != null)
        {
            Vector3 intialPosition = getInitialPosition();

			lightning.transform.position = intialPosition;

            var bolt = lightning.gameObject.GetComponent<LightningBoltScript>();
            bolt.StartObject.transform.position = intialPosition + new Vector3(
                -0.150f,
                -1.89f,
                -0.27f
            );
			bolt.EndObject.transform.position = intialPosition + (model.transform.forward * 5);

            StartCoroutine(Wait(lightning));
        }


    }

    IEnumerator Wait(GameObject lightning)
    {
        yield return new WaitForSeconds(.2f);
        Destroy(lightning);

    }

    private Vector3 getInitialPosition()
    {
        return transform.position
            + model.transform.forward
            + (model.transform.up * 2);
    }

    private void ThrowBomb () {
		if (bombAmount <= 0 || inAudioInteraction) {
			return;
		}

		GameObject bombObject = Instantiate (bombPrefab);
		bombObject.transform.position = transform.position + model.transform.forward;

		Vector3 throwingDirection = (model.transform.forward + (Vector3.up * .2f)).normalized;

		bombObject.GetComponent<Rigidbody> ().AddForce (throwingDirection * throwingSpeed);

		bombAmount--;
	}

	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.GetComponent<EnemyBullet> () != null) {
			Hit ((transform.position - otherCollider.transform.position).normalized);
			Destroy (otherCollider.gameObject);
		} else if (otherCollider.tag == "BombTreasure") {
			bombAmount += 10;
			Destroy (otherCollider.gameObject);
        }
        else if (otherCollider.tag == "ArrowTreasure")
        {
            arrows += 30;
            Destroy(otherCollider.gameObject);
        }
        else if (otherCollider.tag == "HealingHeart")
        {
            playerHealth.HealthValue += 20;
            Destroy(otherCollider.gameObject);
        }
		else if (otherCollider.gameObject.name == "OutsideKrishnaTemple")
        {
            PlayerInArea.Invoke("OutsideKrishnaTemple");
        }
        else if (otherCollider.tag == "GoldCoins")
        {
            money.Amount += 20;
			CreateCoins.playCoinSound();
            Destroy(otherCollider.gameObject);
        }
    }

	void OnTriggerStay (Collider otherCollider) {
		if (otherCollider.GetComponent<Dungeon> () != null) {
			currentDungeon = otherCollider.GetComponent<Dungeon> ();
		}
	}

	void OnTriggerExit (Collider otherCollider) {
		if (otherCollider.GetComponent<Dungeon> () != null) {
			Dungeon exitDungeon = otherCollider.GetComponent<Dungeon> ();
			if (exitDungeon == currentDungeon) {
				currentDungeon = null;
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
        //Debug.Log("player collision with " + collision.transform.parent.tag);

		if (collision.gameObject.GetComponent<Enemy> ()) {
			Hit ((transform.position - collision.transform.position).normalized);
		}



		//if (collision.gameObject.name == "KrishnaTempleFLOOR")
		//{
		//	PlayerInArea.Invoke("KrishnaTemple");
		//}

	}

	private void Hit (Vector3 direction) {
		Vector3 knockbackDirection = (direction + Vector3.up).normalized;
		playerRigidbody.AddForce (knockbackDirection * knockbackForce);
		knockbackTimer = 1f;

		playerHealth.HealthValue--;
		if (playerHealth.HealthValue <= 0) {
			Destroy (gameObject);
		}
	}

	public void Teleport (Vector3 target) {
		transform.position = target;
		justTeleported = true;
	}

  //  public void EnemyKilled()
  //  {
  //      Debug.Log("another monster killed");

		//experience++;
  //  }

    private void OnEnemyKilledIncreaseExperience(string type, int experienceGain)
    {
        //Debug.Log("player event enemy killed. Type is => " + type);
		if (type == "simple")
		{
			experience += 1 + experienceGain;
		} else if (type == "strong")
		{
			experience += 2 + experienceGain;
		} else if (type == "shoting")
		{
			experience += 3 + experienceGain;
		}
    }

}