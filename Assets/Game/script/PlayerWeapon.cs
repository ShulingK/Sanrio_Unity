using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon : MonoBehaviour
{
    public string nam;
    public int damage;
    public float range;
    public float fireRate = 7f;

    public int bulletMax;
    public int bulletCapacity = 30;
    public int bullet;

    // Start is called before the first frame update
    void Start()
    {
        bullet = bulletCapacity;
    }

    // Update is called once per frame
    void Update()
    {

    }


}
