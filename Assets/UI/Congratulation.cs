using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Congratulation : MonoBehaviour
{
    public GameOverScreen GameOverScreen;
    public AudioSource congratsSound;
    [SerializeField] private BoxCollider _bCollidForCongrats;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameOverScreen.gameObject.SetActive(true);
            congratsSound.Play();
        }
    }
    
}

