using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Time")]
    [SerializeField]
    public float time = 0f;

    [SerializeField]
    private float startTime;

    [SerializeField]
    private float gameDuration = 480f;

    [SerializeField]
    private TextMeshProUGUI timer;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        time = gameDuration; 
        startTime = Time.time;
    }

    void Update()
    {
        time = gameDuration - (int)Time.time - startTime;
        
        timer.text = string.Format("{0:0}:{1:00}", Mathf.Floor(time / 60), time % 60);

    }
}
