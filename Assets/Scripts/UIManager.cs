using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //create singleton

    #region Singleton
    
    static private UIManager _S;
    static public UIManager S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set UIManager singleton _S.");
            }

            _S = value;
        }
    }
    #endregion
    
    [SerializeField] private GameObject losePanel;
    void Start()
    {
        S = this;
        losePanel.SetActive(false);
    }

    public void Loosing()
    {
        losePanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void ReturnToLobby()
    {
        SceneManager.LoadScene(0);
    }
}
