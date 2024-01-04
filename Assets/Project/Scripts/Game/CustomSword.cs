using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CustomSword : MonoBehaviour
{

    public GameObject rotationCenter;
    public Blade blade;
    public float swingingSpeed = 26f;
    public float cooldownSpeed = 2f;
    public float cooldownDuration = .2f;
    public float attackDuration = .25f;

    private Quaternion targetRotation;
    private float cooldownTimer;
    private bool isAttacking;

    public bool IsAttacking { get => isAttacking; }


    // Start is called before the first frame update
    void Start()
    {
        targetRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rotationCenter.transform.localRotation = Quaternion.Lerp(
            rotationCenter.transform.localRotation , 
            targetRotation, 
            Time.deltaTime * (isAttacking ? swingingSpeed : cooldownSpeed) 
        );

        cooldownTimer -= Time.deltaTime;
    }

    public void Attack ()
    {
        if ( cooldownTimer > 0 ) { return; }
        targetRotation = Quaternion.Euler (90, 0 , 0);
        cooldownTimer = cooldownDuration;

        StartCoroutine(CooldownWait());
    }

    private IEnumerator CooldownWait()
    {
        isAttacking = true;

        setBladeAttackState(isAttacking);

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;

        setBladeAttackState(isAttacking);

        //Debug.Log("Sword is attacking? " + isAttacking);
        targetRotation = Quaternion.Euler(0,0,0);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sword OnTriggerEnter " + other.name);
    }

    private void setBladeAttackState(bool isAttacking)
    {
        if (blade)
        {
            blade.IsAttacking = isAttacking;
        }
    }
}
