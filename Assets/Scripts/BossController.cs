using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    //create singleton
    #region Singleton
    
    static private BossController _S;
    static public BossController S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set Player singleton _S.");
            }

            _S = value;
        }
    }
    #endregion

    #region Varibles
    
    [Header("Boss Parameters")]
    [SerializeField] private float speed = 15;
    [SerializeField] private int health = 10;
    [SerializeField] private int attackPower = 1;

    [Header("Stages time")]
    public float activeTime;
    public float passiveTime;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    
    [Header("Don't set")]
    public Animator eyesAnimation;
    public float eyesFlyingTime;
    public int stateNumber = 0;
    
    #endregion

    void Start()
    {
        S = this;

        eyesAnimation = GetComponent<Animator>();
        AnimationClip[] clips = eyesAnimation.runtimeAnimatorController.animationClips;
        eyesFlyingTime = clips[0].length;

        SetState_1();
    }

    // Update is called once per frame
    void Update()
    {
        if (stateNumber == 1)
        {
            Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            //speed *= -1;
            transform.Rotate(0,180,0);
        }

        if (other.tag == "Player")
        {
            if (stateNumber == 1)
            {
                Attack();
            }
        }
    }

    public void SetState_1()
    {
        stateNumber = 1;
        Invoke("SetState_2",activeTime);
    }

    public void SetState_2()
    {
        stateNumber = 2;
        State_2();
        Invoke("SetState_1",passiveTime);
    }

    public void Hitted(int hp)
    {
        health += hp;
        
        if (health <= 0)
        {
            Death();
        }
    }

    private void Attack()
    {
        Player.S.ChangeHealth(-attackPower);
    }
    
    private void Move()
    {
        transform.Translate(Time.deltaTime * -speed, 0,0);
    }

    private void State_2()
    { 
        eyesAnimation.SetTrigger("FlyEyes");
       Invoke("LasersInstance", eyesFlyingTime);
    }

    private void LasersInstance()
    {
        GameObject[] lasers = new GameObject[2];
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i] = Instantiate(laserPrefab);
            lasers[i].transform.position =
                new Vector3(
                    Random.Range(Camera.main.GetComponent<BoxCollider>().bounds.min.x,
                        Camera.main.GetComponent<BoxCollider>().bounds.max.x), 0, 0);
            lasers[i].transform.rotation = Quaternion.identity;
        }
    }

    private void Death()
    {
        Destroy(this.gameObject);
        Level_2_Manager.S.EnemyDefeated();
    }
}
