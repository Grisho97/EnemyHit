using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    //create singleton
    #region Singleton
    
    static private Hammer _S;
    static public Hammer S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set Hammer singleton _S.");
            }

            _S = value;
        }
    }
    #endregion

    #region Varibles
    
    public bool attackPossible; //check the attack possibility
    public Collider2D collisionObj; //hold the object collided with
    
    private Animator attackAnimation;
    private float attackTime; // length of attack animation

    #endregion

    private void Awake()
    {
        attackAnimation = GetComponent<Animator>();
        AnimationClip[] clips = attackAnimation.runtimeAnimatorController.animationClips;
        attackTime = clips[0].length;
    }
    private void Start()
    {
        S = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Turret")
        {
            if (other.gameObject.GetComponent<Turret>().isActive)
            {
                attackPossible = true;
                collisionObj = other;
            }
        }
        
        if (other.gameObject.tag == "Boss")
        {
            if (other.gameObject.GetComponent<BossController>().stateNumber == 2)
            {
                attackPossible = true;
                collisionObj = other;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Turret" || other.tag == "Boss")
        {
            collisionObj = null;
            attackPossible = false;
        }
    }

    public void AttackStart(int attackPower)
    {
        attackAnimation.SetTrigger("Attacking");

        if (collisionObj.tag == "Turret")
        {
            collisionObj.gameObject.GetComponent<Turret>().Hitted(-attackPower);
        }

        if (collisionObj.tag == "Boss")
        {
            collisionObj.gameObject.GetComponent<BossController>().Hitted(-attackPower);
        }
        
        //Disable attack during animation
        attackPossible = false;
        Invoke("PossibilityToAttack",attackTime);
    }

    private void PossibilityToAttack()
    {
        if (collisionObj != null)
        {
            attackPossible = true;
        }
    }
}
