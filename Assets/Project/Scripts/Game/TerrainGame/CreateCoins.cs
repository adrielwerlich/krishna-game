using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateCoins : MonoBehaviour
{
    [SerializeField] private GameObject goldCoinPF;
    [SerializeField] private AudioClip _coinSound;
    public static AudioSource goldCoinsAudioSource;

    void Start()
    {
        goldCoinsAudioSource = gameObject.AddComponent<AudioSource>();
        goldCoinsAudioSource.clip = _coinSound;
        goldCoinsAudioSource.volume = .2f;
        Enemy.EnemyKilled += InstantiateGoldCoin;
    }

    private void InstantiateGoldCoin(Transform obj)
    {
        var coin = Instantiate(goldCoinPF);
        coin.transform.position = new Vector3(
          obj.transform.position.x,
          0,
          obj.transform.position.z
        );
        playCoinSound();
    }

    public static void playCoinSound()
    {
        goldCoinsAudioSource.Play();
    }

}
