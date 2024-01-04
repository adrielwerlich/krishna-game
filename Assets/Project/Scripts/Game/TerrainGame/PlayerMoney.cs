using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] private int amount;

    public int Amount { get => amount; set => amount = value; }

}
