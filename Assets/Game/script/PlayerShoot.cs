using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : MonoBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    public GameObject hitEffectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log(hit.collider.name);
            GameObject ink = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            Destroy(ink,30f);
        }
    }


}
