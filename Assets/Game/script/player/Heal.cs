using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [Header("Values")]
    [SerializeField]
    public float healValue;
    [SerializeField]
    private float secondsToWait = 30f;

    [Header("Components")]
    [SerializeField]
    public List<MeshRenderer> ObjectToUnactive; 

    public void Unactive()
    {
        StartCoroutine(Cooldown());
    }


    private IEnumerator Cooldown()
    {
        foreach (MeshRenderer obj in ObjectToUnactive)
        {
            obj.enabled = false;
        }

        yield return new WaitForSeconds(secondsToWait);

        foreach (MeshRenderer obj in ObjectToUnactive)
        { 
            obj.enabled = true; 
        }
    }
}
