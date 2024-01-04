using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour {

	[Header("Game")]
	public Player player;
	public UnityPlayer UnityPlayer;
    public CustomGameCamera gameCamera;

	[Header("UI")]
	public GameObject[] hearts;
    public Text playerHealth;
    public Text bombText;
	public Text arrowText;
	public Text moneyText;
    public Text orbText;
	public GameObject dungeonPanel;
	public Text dungeonInfoText;

	[SerializeField] private bool _showMoneyText = false;

	// Use this for initialization
	void Start () {
		if (moneyText != null) { 
			moneyText.gameObject.SetActive(_showMoneyText);
		}
	}

	void SetValues(int health, int bombs, int arrows, int money = 0)
	{
        playerHealth.text = "Health: " + health;
        bombText.text = "Bombs: " + bombs;
        arrowText.text = "Arrows: " + arrows;
		if (moneyText != null )
		{
            moneyText.text = "Money: " + money;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (UnityPlayer != null)
		{
            SetValues(
                UnityPlayer.GetComponent<Health>().HealthValue,
                UnityPlayer.bombs,
                UnityPlayer.arrows
            );
        }
		else if (player != null) 
		{
			// Check for player information.
			//for (int i = 0; i < hearts.Length; i++) {
			//	hearts [i].SetActive (i < player.health);
			//}
			int money = 0;
			if (Player.money != null)
			{
				money = Player.money.Amount;
			}
			SetValues(
                player.GetComponent<Health>().HealthValue,
				player.bombAmount,
				player.arrows,
				money
            );
			//playerHealth.text = "Health: " + player.health;
   //         bombText.text = "Bombs: " + player.bombAmount;
			//arrowText.text = "Arrows: " + player.arrowAmount;
			//orbText.text = "Orbs: " + player.orbAmount;

			// Check for dungeon information.
			//Dungeon currentDungeon = player.CurrentDungeon;
			//dungeonPanel.SetActive(currentDungeon != null);
			//if (currentDungeon != null) {
				//float clearPercentage = (float)(currentDungeon.EnemyCount - currentDungeon.CurrentEnemyCount) / currentDungeon.EnemyCount;
				//dungeonInfoText.text = "Progress: " + Mathf.FloorToInt(clearPercentage * 100) + "%";

				//if (currentDungeon.JustCleared) {
				//	gameCamera.FocusOn (currentDungeon.Treasure.gameObject);
				//}
			//}
		} else {
			for (int i = 0; i < hearts.Length; i++) {
				hearts [i].SetActive (false);
			}

			//resetTimer -= Time.deltaTime;
			//if (resetTimer <= 0f) {
			//	SceneManager.LoadScene ("Menu");
			//}
		}
	}
}
