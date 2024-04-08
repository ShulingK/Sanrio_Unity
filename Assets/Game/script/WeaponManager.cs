using UnityEngine;
using Mirror;
using System.Collections;

public class WeaponManager : MonoBehaviour
{

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    [SerializeField]
    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    void Start()
    {
        EquipWeapon(primaryWeapon);
        Debug.Log("hey");
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
    }


}