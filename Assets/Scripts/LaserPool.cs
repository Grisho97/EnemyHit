using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPool : MonoBehaviour
{
    //create singleton
    #region Singleton
    
    private static LaserPool _S;
     
    public static LaserPool S
    {
        get
        {
            if (_S == null)
            {
                Debug.Log("error");
            }
     
            return _S;
        }
    }
    #endregion

    //Bullet pooling
    [SerializeField] private GameObject LaserContainer;
    [SerializeField] private GameObject _LaserPrefab;
    [SerializeField] private List<GameObject> _listOfLasers;

    public void Awake()
    {
        _S = this;
    }

    public void Start()
    {
        _listOfLasers = GenarateLasers(2);
    }

    List<GameObject> GenarateLasers(int ammountOfBullets)
    {
        for (int i = 0; i < ammountOfBullets; i++)
        {
            GameObject laser = Instantiate(_LaserPrefab);
            laser.transform.parent = LaserContainer.transform;
            laser.SetActive(false);
            _listOfLasers.Add(laser);
        }
        
        return _listOfLasers;
    }

    public GameObject RequestLaser()
    {
        foreach (var bull in _listOfLasers)
        {
            if (bull.activeInHierarchy == false)
            {
                bull.SetActive(true);
                return bull;
            }
        }
        GameObject newLaser = Instantiate(_LaserPrefab);
        newLaser.transform.parent = LaserContainer.transform;
        _listOfLasers.Add(newLaser);
        return newLaser;
    }
}
