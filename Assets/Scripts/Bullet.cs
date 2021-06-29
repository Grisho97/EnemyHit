using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float    bulletSpeed = 10;
    [SerializeField] private float    lifeTime = 3;
    [SerializeField] private int      attackPower = 1;

    void OnEnable()
    {
        Invoke("Hide", lifeTime);
    }

    private void Update()
    {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            BulletAttack();
        }
    }

    private void BulletAttack()
    {
        if (Player.S.underShield == false)
        {
            Player.S.ChangeHealth(-attackPower);
        }
        Hide();   
    }
    
    void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
