using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 10f * Time.deltaTime, 0);       
    }
}
