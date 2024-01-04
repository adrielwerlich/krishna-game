using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityPlayer : MonoBehaviour
{

    //[Header("Visuals")]
    public GameObject model;
    private Rigidbody playerRigidbody;

    [Header("Equipment")]
    private Sword sword;
    [SerializeField] private Bow bow;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float throwingSpeed;
    public int arrows = 100;
    public int bombs = 100;
    //public int health = 30;

    


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        InputManager.FireArrow += FireArrow;
        InputManager.FireBomb  += ThrowBomb;
    }

    private void OnDisable()
    {
        InputManager.FireArrow -= FireArrow;
        InputManager.FireBomb -= ThrowBomb;
    }


    // Update is called once per frame
    void Update()
    {


    }


   
    private void FireArrow()
    {
        if (arrows > 0)
        {
            //sword.gameObject.SetActive(false);
            //bow.gameObject.SetActive(true);
            bow.Attack();
            arrows--;
        }
    }

    private void ThrowBomb()
    {
        bombs--;
        if (bombs >= 0)
        {
            GameObject bombObject = Instantiate(bombPrefab);
            bombObject.transform.position = transform.position + model.transform.forward * 4 * throwingSpeed;
            Vector3 position = bombObject.transform.position;
            position.y += 1f;
            bombObject.transform.position = position;
            Vector3 throwingDirection = (model.transform.forward + Vector3.up * 4).normalized;
            bombObject.GetComponent<Rigidbody>().AddForce(throwingDirection * throwingSpeed);
        }
    }


    //public void Teleport(Vector3 target)
    //{
    //    transform.position = target;
    //    justTeleported = true;
    //}

}
