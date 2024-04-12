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
        Debug.Log("shoot");
        //GetComponentInParent<Transform>
        if (currentWeapon.bullet <= 0)
        {
            Debug.Log("tamere");
            StartCoroutine(Reload());
            return;
        }

        GameObject newShuriken = Instantiate(shuriken, cam.transform.position + new Vector3(0,-1, 1), GetComponentInParent<Transform>().rotation) ;
        newShuriken.GetComponent<Rigidbody>().velocity += GetComponentInParent<Transform>().rotation * cam.transform.forward * 100 ;
        

        currentWeapon.bullet--;
        /*        RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range))
                {

                }*/
    }

    IEnumerator Reload()
    {
        //Print the time of when the function is first called.
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(30);

        //After we have waited 5 seconds print the time again.
        Debug.Log("reloading");
        currentWeapon.bullet++;
    }
}
