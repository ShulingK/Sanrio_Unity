using UnityEngine;

public class AudioScheduler : MonoBehaviour
{
    public AudioClip babyClip; // Ajoutez votre premier clip audio ici (par exemple, "baby")
    public AudioClip paperClip; // Ajoutez votre deuxième clip audio ici (par exemple, "paper")
    private AudioSource audioSource;

    public float babyDelay = 0f; // délai avant la première lecture du clip "baby"
    public float paperDelay = 20f; // délai avant la première lecture du clip "paper"

    private bool babyStarted = false;
    private bool paperStarted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si aucun AudioSource n'est attaché à cet objet, en créer un nouveau
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assurez-vous que les clips audio sont définis
        if (babyClip == null || paperClip == null)
        {
            Debug.LogError("Veuillez assigner les AudioClips au composant AudioScheduler.");
            enabled = false; // Désactive le script s'il n'y a pas de clips audio assignés
        }
    }

    void Update()
    {
        // Vérifier si le délai pour le clip "baby" n'a pas encore été écoulé
        if (!babyStarted && Time.time >= babyDelay)
        {
            babyStarted = true;
            PlaySound(babyClip); // Commencer à jouer le clip "baby"
        }

        // Vérifier si le délai pour le clip "paper" n'a pas encore été écoulé
        if (!paperStarted && Time.time >= paperDelay)
        {
            paperStarted = true;
            PlaySound(paperClip); // Commencer à jouer le clip "paper"
        }
    }

    void PlaySound(AudioClip clip)
    {
        // Jouer le son spécifié
        audioSource.PlayOneShot(clip);
    }
}
