using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Varibles

    [SerializeField] private GameObject ActiveBeam; //prefab with active laser

    [SerializeField] private int laserPower = 1;
    [SerializeField] private float timeOfPassiveBeam = 3;//time before active laser

    private bool isActive;
    private bool playerUnderLaser;
    
    #endregion
    void Start()
    {
        isActive = false;
        Invoke("ActiveMode", timeOfPassiveBeam);
        Invoke("Hide", BossController.S.passiveTime - BossController.S.eyesFlyingTime);
    }

    private void ActiveMode()
    {
        ActiveBeam.SetActive(true);
        isActive = true;
        InvokeRepeating("LaserAttackX",0,1);
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
        BossController.S.eyesAnimation.SetTrigger("EyesBack");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerUnderLaser = true;
            
            if (isActive == true)
            {
                Player.S.ChangeHealth(-laserPower);
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerUnderLaser = false;
        }
    }

    private void LaserAttackX()
    {
        if (playerUnderLaser)
        {
            Player.S.ChangeHealth(-laserPower);
        }
    }
}
