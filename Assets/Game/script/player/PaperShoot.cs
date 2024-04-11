using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Threading.Tasks;
using System.Threading;

[RequireComponent(typeof(WeaponManager))]
public class PaperShoot : MonoBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;
    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;



    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();

    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }

    }

    public GameObject shuriken;
    private void Shoot()
    {
        GameObject newShuriken = Instantiate(shuriken, transform.position, transform.rotation);
        newShuriken.GetComponent<Rigidbody>().velocity = transform.rotation * transform.forward;
/*        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range))
        {

        }*/
    }

    private void Reload(PlayerWeapon _currentWeapon)
    {
        //au bout de 30
    }


}
