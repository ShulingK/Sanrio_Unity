using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFeet : MonoBehaviour
{
    public bool isGrounded;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("stay");
        if (other.tag == "ground" || other.tag == "jumpable")
        {
            Debug.Log("Collide");
            isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "ground" || collision.tag == "jumpable")
        {
            isGrounded = false;
        }
    }
}
