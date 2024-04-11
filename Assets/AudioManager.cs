using UnityEngine;

public class AudioScheduler : MonoBehaviour
{
    public AudioClip babyClip; // Ajoutez votre premier clip audio ici (par exemple, "baby")
    public AudioClip paperClip; // Ajoutez votre deuxi�me clip audio ici (par exemple, "paper")
    private AudioSource audioSource;

    public float babyDelay = 0f; // d�lai avant la premi�re lecture du clip "baby"
    public float paperDelay = 20f; // d�lai avant la premi�re lecture du clip "paper"

    private bool babyStarted = false;
    private bool paperStarted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si aucun AudioSource n'est attach� � cet objet, en cr�er un nouveau
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assurez-vous que les clips audio sont d�finis
        if (babyClip == null || paperClip == null)
        {
            Debug.LogError("Veuillez assigner les AudioClips au composant AudioScheduler.");
            enabled = false; // D�sactive le script s'il n'y a pas de clips audio assign�s
        }
    }

    void Update()
    {
        // V�rifier si le d�lai pour le clip "baby" n'a pas encore �t� �coul�
        if (!babyStarted && Time.time >= babyDelay)
        {
            babyStarted = true;
            PlaySound(babyClip); // Commencer � jouer le clip "baby"
        }

        // V�rifier si le d�lai pour le clip "paper" n'a pas encore �t� �coul�
        if (!paperStarted && Time.time >= paperDelay)
        {
            paperStarted = true;
            PlaySound(paperClip); // Commencer � jouer le clip "paper"
        }
    }

    void PlaySound(AudioClip clip)
    {
        // Jouer le son sp�cifi�
        audioSource.PlayOneShot(clip);
    }
}
