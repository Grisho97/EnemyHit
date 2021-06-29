using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //create singleton
    #region Singleton
    
    private static PoolManager _S;
     
    public static PoolManager S
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
    [SerializeField] private GameObject bulletContainer;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private List<GameObject> _listOfBullets;

    public void Awake()
    {
        _S = this;
    }

    public void Start()
    {
        _listOfBullets = GenarateBullets(10);
    }

    List<GameObject> GenarateBullets(int ammountOfBullets)
    {
        for (int i = 0; i < ammountOfBullets; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab);
            bullet.transform.parent = bulletContainer.transform;
            bullet.SetActive(false);
            _listOfBullets.Add(bullet);
        }
        
        return _listOfBullets;
    }

    public GameObject RequestBullet()
    {
        foreach (var bull in _listOfBullets)
        {
            if (bull.activeInHierarchy == false)
            {
                bull.SetActive(true);
                return bull;
            }
        }
        GameObject newbullet = Instantiate(_bulletPrefab);
        newbullet.transform.parent = bulletContainer.transform;
        _listOfBullets.Add(newbullet);
        return newbullet;
    }
}
