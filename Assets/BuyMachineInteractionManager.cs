using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BuyMachineInteractionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userMessage;
    [SerializeField] private GameObject interactionPanel;

    [SerializeField] private GameObject _treasureWeaponsPrefab;
    [SerializeField] private GameObject _treasureHealingPrefab;

    public static event Action<Helper.BuyLineIndex> PlayBuyLine;
    public static event Action HideDirectionArrow;
    private bool _chantingBuyLine = false;

    //private bool _inBuyingArea = false;

    private void Start()
    {
        InputManager.FireArrow += BuyWeapons;
        InputManager.FireBomb += BuyHealings;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Player.inAudioInteraction = true;
            //_inBuyingArea = true;
            interactionPanel.gameObject.SetActive(true);
            printUserMessage();
        }
    }

    private void printUserMessage()
    {
        string weaponsRechargeLine = Application.isMobilePlatform
            ? "Press the arrow to get the weapons"
            : "Press 1 to get weapons";

        string healingRechargeLine = Application.isMobilePlatform
            ? "Press the bomb to get the healings"
            : "Press 2 to get healings recharge";

        bool playerHasMoney = Player.money.Amount > 0;

        string weaponsMantra = playerHasMoney
               ? weaponsRechargeLine
               : "You have no money to get weapons";

        string healingMantra = playerHasMoney
            ? healingRechargeLine
            : "You have no money to get healings";

        userMessage.text = $"This is the buying machine" +
            $"\r\n{weaponsMantra}" +
            $"\r\n{healingMantra}";
    }

    void OnTriggerStay()
    {
        if (!_chantingBuyLine)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                BuyWeapons();
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                BuyHealings();
            }

        }
    }

    private Vector3 getNewPosition()
    {
        Vector3 reference = this.transform.position + (UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(4f, 14f));
        Vector3 newPosition = new Vector3(reference.x, 0, reference.z);
        return newPosition;
    }


    private void BuyHealings()
    {
        if (hasConditionsToBuy())
        {
            PlayBuyLine.Invoke(Helper.BuyLineIndex.HealingLine);
            Instantiate(_treasureHealingPrefab, getNewPosition(), Quaternion.identity);
            Player.money.Amount -= 10;
            UpdateScroolInteraction();
        }

    }

    private bool hasConditionsToBuy()
    {
        return Player.money.Amount >= 10 && Player.inAudioInteraction;
    }

    private void BuyWeapons()
    {
        if (hasConditionsToBuy())
        {
            PlayBuyLine.Invoke(Helper.BuyLineIndex.WeaponLine);
            Player.money.Amount -= 10;
            Instantiate(_treasureWeaponsPrefab, getNewPosition(), Quaternion.identity);
            UpdateScroolInteraction();
        }

    }

    private void UpdateScroolInteraction()
    {
        _chantingBuyLine = true;
        StartCoroutine(checkScroolInterationState());
    }

    private IEnumerator checkScroolInterationState()
    {
        yield return new WaitForSeconds(3.0f);

        if (Player.HasWeaponsMantra && Player.HasHealingMantra)
        {
            HideDirectionArrow.Invoke();
            this.gameObject.SetActive(false);
            closePanel();
        }

        printUserMessage();
        _chantingBuyLine = false;
    }

    void OnTriggerExit()
    {
        closePanel();
    }

    private static void SetPlayerInteractionFalse()
    {
        Player.inAudioInteraction = false;
    }

    private void closePanel()
    {
        interactionPanel.gameObject.SetActive(false);
        userMessage.text = "";
        SetPlayerInteractionFalse();
    }
}
