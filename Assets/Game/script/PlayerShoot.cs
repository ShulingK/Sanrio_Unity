using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;
using static UnityEditor.PlayerSettings;
public class PlayerShoot : MonoBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))//GetKeyDown("mouse0")
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            Debug.Log(hit.collider.name);
            Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }


}
