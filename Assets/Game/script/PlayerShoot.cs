using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Threading.Tasks;

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

        if (currentWeapon.bullet > 0)
        {
            if (currentWeapon.fireRate <= 0f)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                    currentWeapon.bullet -= 1;
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();

                    //Task.Wait();
                    //Wait();
                    InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
                    
                    Debug.Log(currentWeapon.bullet);
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    CancelInvoke("Shoot");
                    currentWeapon.bullet -= 1;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R)){
            Reload(currentWeapon);
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

    private void Reload(PlayerWeapon _currentWeapon)
    {

        if(_currentWeapon.bulletMax == 0)
        {
            return;
        }
        if(_currentWeapon.bulletMax>= _currentWeapon.bulletCapacity)//60>30
        {

            int bulletUse = _currentWeapon.bulletCapacity - _currentWeapon.bullet;
            _currentWeapon.bulletMax -= bulletUse;//60 -30-25
            _currentWeapon.bullet = _currentWeapon.bulletCapacity;//25 ->30

            
        }
        else if(_currentWeapon.bulletMax < _currentWeapon.bulletCapacity)
        {
            int bulletUse = _currentWeapon.bulletCapacity - _currentWeapon.bullet;//30-25 = 5
            _currentWeapon.bullet += bulletUse;//30-15 29/30 
            _currentWeapon.bulletMax -= bulletUse ;
        }
        if(_currentWeapon.bulletMax < 0)
        {
            _currentWeapon.bulletMax = 0;
        }
    }


}
