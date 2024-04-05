using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] components_to_disable;

    [SerializeField]
    Camera defaultCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            // désactive les composants si ce n'est pas à nous
            if (components_to_disable != null)
            {
                for (int i = 0; i < components_to_disable.Length; i++)
                {
                    components_to_disable[i].enabled = false;
                }
            }
        }
        else
        {
            if (defaultCamera != null)
            {
                Camera.main.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        if (defaultCamera != null)
            Camera.main.gameObject.SetActive(true);
    }
}