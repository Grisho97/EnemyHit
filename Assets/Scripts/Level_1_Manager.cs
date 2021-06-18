using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_1_Manager : MonoBehaviour
{
    //create singleton
    #region Singleton
    
    static private Level_1_Manager _S;
    static public Level_1_Manager S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set Level_1_Manager singleton _S.");
            }

            _S = value;
        }
    }
    #endregion

    [SerializeField] private GameObject door;

    private void Start()
    {
        S = this;
    }

    public void EnemyDefeated()
    {
        door.GetComponent<Door>().Activate();
    }

}
