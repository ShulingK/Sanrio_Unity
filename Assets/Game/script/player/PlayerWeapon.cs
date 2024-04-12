using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

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
        text1.text = bullet.ToString();
        text2.text = bulletMax.ToString();
    }


}
