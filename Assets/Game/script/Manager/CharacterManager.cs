using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class CharacterManager : NetworkBehaviour
{
    [Header("Stats")]
    [SerializeField] 
    private float maxLife = 100f;

    [SyncVar]
    [SerializeField]
    private float life;

    public Image bar;
    public Sprite full;
    public Sprite three;
    public Sprite two;
    public Sprite one;

    public Renderer renderer;

    public void Start()
    {
        life = maxLife;
    }

    public void Update()
    {
        if (life == 100)
        {
            bar.sprite = full;
        }
        if (life == 75)
        {
            bar.sprite = three;
        }
        if (life == 50)
        {
            bar.sprite = two;
        }
        if (life == 25)
        {
            bar.sprite = one;
        }
        if (life == 0)
        {
            gameObject.GetComponent<PlayerSetup>().loseUI.SetActive(true);
            
            renderer.enabled = false;

            StartCoroutine(gameObject.GetComponent<PlayerSetup>().Delay(5));
    
            SceneManager.LoadScene(0);
        }
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
        Debug.Log("dead");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "heal")
        {
            GetHeal(other.GetComponent<Heal>().healValue);
            other.GetComponent<Heal>().Unactive();
        }

        if (other.tag == "key")
        {
            other.GetComponent<KeyManager>().AddKey();
        }
        if (other.tag == "finishzone" && !GetComponent<PlayerSetup>().isBabyboo && GameManager.Instance.GetKeyCount() == GameManager.Instance.keyCountMax)
        {
            GameManager.Instance.isPapermenWon = true;
        }
    }
}
