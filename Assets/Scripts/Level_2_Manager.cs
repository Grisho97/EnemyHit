using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_2_Manager : MonoBehaviour
{
    //create singleton
    #region Singleton
    
    static private Level_2_Manager _S;
    static public Level_2_Manager S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set Level_2_Manager singleton _S.");
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
