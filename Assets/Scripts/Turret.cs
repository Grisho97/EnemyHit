using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    #region Varibles
    
    [Header("Parameters")]
    [SerializeField] private int lives = 2;
    
    [Tooltip("Speed of shooting")]
    [SerializeField] private float cooldown = 2;
    
    [Header("Prefabs")]
    [SerializeField] GameObject BulletPrefab;
    [Tooltip("Start point fpr bullet")]
    [SerializeField] GameObject rifleStart;
    
    [Header("State of the turret")]
    public bool isActive = true;
    public bool isAlive = true;
    
    private GameObject playerCenter;//needs for correct shooting
    private float angleCorrection = 90;
    private float timer = 0;
    
    
    #endregion
    
    void Start()
    {
       playerCenter = GameObject.FindGameObjectWithTag("PlayerCenter");

       if (isActive == false)
       {
           GetComponent<Animator>().SetTrigger("Disactivate");
       }
    }
    
    void Update()
    {
        if (isActive == true)
        {
            TurretRotate();
            ShootingCheck();
        }
    }

    private void Shooting(float angle)
    {
        timer += Time.deltaTime;
        if (timer > cooldown)
        {
            timer = 0;
            GameObject buf = PoolManager.S.RequestBullet();
            buf.transform.position = rifleStart.transform.position;
            buf.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - angleCorrection));
        }
    }

    private void TurretRotate()
    {
        if (Player.S.transform.position.x < transform.position.x -1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (Player.S.transform.position.x > transform.position.x +1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void ShootingCheck()
    {
        //Check the angle
        Vector3 targetPos = playerCenter.transform.position;
        Vector3 thisPos = rifleStart.transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            
        if ((Mathf.Abs(angle) > 110 && transform.eulerAngles == new Vector3(0, 0, 0)) || (Mathf.Abs(angle) < 70 && transform.eulerAngles == new Vector3(0, 180, 0)))
        {
            Shooting(angle);
        }
    }
    
    public void Hitted(int hp)
    {
        lives += hp;

        if (lives <= 0)
        {
            Disactivate();
        }
    }
    public void Disactivate()
    {
        if (isAlive)
        {
            isActive = false;
            TurretsController.S.DefeatedTurret = this.gameObject;
            GetComponent<Animator>().SetTrigger("Disactivate");
            //Invoke("Activate",10f);
        }
    }

    public void Death()
    {
        isActive = false;
        isAlive = false;
    }

    public void Activate()
    {
        if (isAlive)
        {
            GetComponent<Animator>().SetTrigger("Activate");
            isActive = true;
        }
    }
}
