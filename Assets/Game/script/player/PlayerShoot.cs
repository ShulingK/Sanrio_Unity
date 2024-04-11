using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Threading.Tasks;
using System.Threading;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : MonoBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    public PlayerWeapon currentWeapon;
    public WeaponManager weaponManager;


    public ParticleSystem muzzleFlash;
    public GameObject hitEffectPrefab;
    
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
        if(currentWeapon.bullet <= 0)
        {
            CancelInvoke("Shoot");
        }
        if (Input.GetKeyDown(KeyCode.R)){
            Reload(currentWeapon);
        }
        
    }

    private void Shoot()
    {

        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            GameObject ink = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            if(hit.collider.tag == "Player")
            {
                CharacterManager test = hit.collider.gameObject.GetComponent<CharacterManager>();
                test.TakeDamage(currentWeapon.damage);

            }
            Destroy(ink,30f);
            
        }
        currentWeapon.bullet -= 1;
        if (currentWeapon.bullet <= 0)
        {
            return;
        }
    }

    private void Reload(PlayerWeapon _currentWeapon)
    {
        int bulletUse = _currentWeapon.bulletCapacity - _currentWeapon.bullet;
        if (_currentWeapon.bulletMax == 0)
        {
            return;
        }
        if(_currentWeapon.bulletMax < bulletUse)
        {
            _currentWeapon.bullet += _currentWeapon.bulletMax;
            _currentWeapon.bulletMax = 0;
        }
        else
        {
            _currentWeapon.bulletMax -= bulletUse;
            _currentWeapon.bullet += bulletUse; 
        }
        if(_currentWeapon.bulletMax < 0)
        {
            _currentWeapon.bulletMax = 0;
        }
    }


}
