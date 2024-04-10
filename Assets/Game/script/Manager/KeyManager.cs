using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public void AddKey()
    {
        GameManager.Instance.AddKey();
        Destroy(gameObject);
    }

    public bool IsAllKeysCollected()
    {
        if(GameManager.Instance.GetKeyCount() == GameManager.Instance.keyCountMax) 
        {
            return true;
        }
        return false;
    }
}