using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class ScrollInteractionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userMessage;
    [SerializeField] private GameObject interactionPanel;
    
    public static event Action<Helper.MantraIndex> PlayMantra;
    public static event Action HideDirectionArrow;
    private bool _chantingMantra = false;

    private void Start()
    {
        InputManager.FireArrow += GetWeaponsMantra;
        InputManager.FireBomb += GetHealingMantra;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Player.inAudioInteraction = true;
            interactionPanel.gameObject.SetActive(true);
            printUserMessage();
        }
    }

    private void printUserMessage()
    {
        string weaponsRechargeLine = Application.isMobilePlatform
            ? "Press the arrow to get the weapons mantra"
            : "Press 1 to learn the weapons recharge mantra";

        string healingRechargeLine = Application.isMobilePlatform
            ? "Press the bomb to get the healing mantra"
            : "Press 2 to learn the healing recharge mantra";

        string weaponsMantra = !Player.HasWeaponsMantra 
               ? weaponsRechargeLine
               : "You already have the weapons mantra";

        string healingMantra = !Player.HasHealingMantra
            ? healingRechargeLine
            : "You already have the healing mantra";

        userMessage.text = $"This scroll has the sacred mantra" +
            $"\r\n{weaponsMantra}" +
            $"\r\n{healingMantra}";
    }

    void OnTriggerStay()
    {
        if (!_chantingMantra)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                GetWeaponsMantra();
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                GetHealingMantra();
            }

        }
    }

    private void GetHealingMantra()
    {
        if (!Player.HasHealingMantra)
        {
            PlayMantra.Invoke(Helper.MantraIndex.HealingMantra);
            Player.HasHealingMantra = true;
            UpdateScroolInteraction();
        }

    }

    private void GetWeaponsMantra()
    {
        if (!Player.HasWeaponsMantra)
        {
            PlayMantra.Invoke(Helper.MantraIndex.WeaponMantra);
            Player.HasWeaponsMantra = true;
            UpdateScroolInteraction();
        }

    }

    private void UpdateScroolInteraction()
    {
        _chantingMantra = true;
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
        _chantingMantra = false;
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
