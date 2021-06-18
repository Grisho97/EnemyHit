using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretsController : MonoBehaviour
{
    //create singleton

    #region Singleton
    
    static private TurretsController _S;
    static public TurretsController S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set TurretsController singleton _S.");
            }

            _S = value;
        }
    }
    #endregion

    #region Varibles
    
    [FormerlySerializedAs("EmptyObject")] public GameObject DefeatedTurret;

    //Turret activation logic
    public List<GameObject> chain1;
    public List<GameObject> chain2;

    //Right order to beat turrets
    public List<GameObject> orderToBeat;

    private int currentOrder;//Number of turret that should be beaten
    
    private int numberOfChainsAlive = 2;

    #endregion
    private void Start()
    {
        S = this;
    }

    private void Update()
    {
        if (DefeatedTurret != null)
        {
            CheckOrder();
            CheckActivation();
        }
    }

    private void ChainDeath(List<GameObject> chain)
    {
        foreach (var turret in chain)
        {
            turret.GetComponent<Turret>().Death();
        }

        if (numberOfChainsAlive <= 0)
        {
            Level_1_Manager.S.EnemyDefeated();
        }
    }

    private void CheckActivation()
    {
        if (chain1.Contains(DefeatedTurret))
        {
            var i = chain1.FindIndex(d => d == DefeatedTurret); 
            if (i < chain1.Count - 1)
            {
                chain1[i+1].GetComponent<Turret>().Activate();
            }
            else
            {
                numberOfChainsAlive -= 1;
                ChainDeath(chain1);
            }
        }
        
        else if (chain2.Contains(DefeatedTurret))
        {
            var i = chain2.FindIndex(d => d == DefeatedTurret);
            if (i < chain2.Count - 1)
            {
                chain2[i+1].GetComponent<Turret>().Activate();
            }
            else
            {
                numberOfChainsAlive -= 1;
                ChainDeath(chain2);
            }
        }

        else
        {
            Debug.Log("Wrong Object");
        }

        DefeatedTurret = null;
    }

    private void CheckOrder()
    {
        if (orderToBeat.Contains(DefeatedTurret))
        {
            var i = orderToBeat.FindIndex(d => d == DefeatedTurret);
            if (i != currentOrder)
            {
                UIManager.S.Loosing();
            }
            else
            {
                currentOrder++;
            }
        }
    }
}
