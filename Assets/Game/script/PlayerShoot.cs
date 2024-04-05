using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerShoot : MonoBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;
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
        if (Physics.Raycast(cam.transform.position,cam.transform.forward, out hit , weapon.range, mask))
        {
            Debug.Log(hit.collider.name);
        }
    }
}
