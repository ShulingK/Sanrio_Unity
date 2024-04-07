using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [Header("Values")]
    [SerializeField]
    public float healValue;
    [SerializeField]
    private float secondsToWait = 2f;

    [Header("Components")]
    [SerializeField]
    private List<GameObject> ObjectToUnactive; 

    public void Unactive()
    {
        StartCoroutine(Cooldown());
    }


    private IEnumerator Cooldown()
    {
        foreach (GameObject obj in ObjectToUnactive)
        {
            obj.SetActive(false);
        }

        yield return new WaitForSeconds(2);

        foreach (GameObject obj in ObjectToUnactive)
        { 
            obj.SetActive(true); 
        }
    }
}
