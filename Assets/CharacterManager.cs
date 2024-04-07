using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] 
    private float maxLife = 100f;
    [SerializeField]
    private float life;

    public void Start()
    {
        life = maxLife;
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Death();
        }
    }
    
    public void GetHeal(float heal)
    {
        life += heal;
        if (life > maxLife)
        {
            life = maxLife;
        }
    }

    public void Death()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "heal")
        {
            Debug.Log("collideeeee");

            GetHeal(other.GetComponent<Heal>().healValue);
            other.GetComponent<Heal>().Unactive();
        }
    }
}
