using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlaySettings : MonoBehaviour
{
    private float rotationSpeed;

    [Header("Player")]
    [SerializeField] private Player p;
    [SerializeField] private UnityPlayer up;
    private ProcessMovementInput movementInput;

    [Header("UIController")]
    [SerializeField] private Slider slider;
    [SerializeField] private Slider mobileSlider;

    [SerializeField] private Text rotationTextValue;


    // Start is called before the first frame update
    void Start()
    {
        if (up != null)
        {
            movementInput = up.GetComponent<ProcessMovementInput>();
        } else if (p != null)
        {
            movementInput = p.GetComponent<ProcessMovementInput>();
        }

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnGUI()
    {
        // Get the value of the slider.
        float value;

        if (Application.isMobilePlatform)
        {
            value = mobileSlider.value;
        }
        else
        {
            value = slider.value;
        }


        if (value != rotationSpeed) {
            rotationSpeed = value;
            UpdatePlayerRotationSpeed(rotationSpeed);
        }
    }

    private void UpdatePlayerRotationSpeed(float rotationSpeed)
    {
        movementInput.playerRotatingSpeed = rotationSpeed;
        rotationTextValue.text = rotationSpeed.ToString();
    }
}
