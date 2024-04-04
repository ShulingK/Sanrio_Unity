using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFeet : MonoBehaviour
{
    public bool isGrounded;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "ground")
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "ground")
        {
            isGrounded = false;
        }
    }
}
