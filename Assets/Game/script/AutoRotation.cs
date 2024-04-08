using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    [SerializeField]
    public Vector3 rotation;


    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(rotation * Time.deltaTime);       
    }
}
