using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HealthAndEnergy : MonoBehaviour
{
    [Header("Settings")]
    public int MaxHealth = 100;
    [Range(-10, 100)] public int health = 100;
    public float maxEnergy = 100;
    [Range(0, 100)] public float energy = 100;
    
    bool isDead;
    bool damaged;
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    //public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 0.9f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);         // The colour the damageImage is set to, to flash.
    public int flashTimer;

    [Header("UI")] 
    public Text HpText;
    public Text energyText;
    public Text TimeText;
    public Image HpBar;
    public Image energyBar;
    public HpAddedText HpAddedTextPrefab;
    public HpAddedText MaxHpAddedTextPrefab;
    public GameOverScreen GameOverScreen;
    
    [Header("Audio")] 
    public AudioSource GameOverSound;
    
    
    public static HealthAndEnergy Instance;
    public static float TimePassed;
    public float TimeM;
    private Animator anim;
    private static readonly int runParam = Animator.StringToHash("Run");

    private float lastHpBarFill;

    private void Start()
    {
        Instance = this;

        lastHpBarFill = health;

        TimePassed = 0;
              
        anim = GetComponent<Animator>();
            //playerAudio = GetComponent<AudioSource>();
            //playerMovement = GetComponent<PlayerMovement>();
            //playerShooting = GetComponentInChildren<PlayerShooting>();

            // Set the initial health of the player.
           // currentHealth = startingHealth;
        
    }

    private void Update()
    {
        if (!MainMenu.IsGameStarted) return;
        // If the player has just been damaged...
        if (damaged)
        {
                // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
            flashTimer = 180;
        }
        // Otherwise...
        else
        {
            if(flashTimer >= 1)
            {
                if (flashTimer <= 90)
                {
                    damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);// ... transition the colour back to clear.
                }
                flashTimer--;
                if (flashTimer == 0) { damageImage.color = Color.clear; }
            }
        }

        // Reset the damaged flag.
        damaged = false;     

        TimePassed += Time.deltaTime;

        int minutes = (int)TimePassed / 60;
        int seconds = (int)TimePassed % 60;
        TimeText.text = $"{minutes:00}:{seconds:00}";

        energy -= Time.deltaTime * 3 / 165;                 // 3 - временный коэф., пока поле маленькое

        if (health <= 0)
        {
            GameOverScreen.gameObject.SetActive(true);
            GameOverSound.Play();
        }

        UpdateUI();
        TimeM = TimePassed;
    }

    private void UpdateUI()
    {
        //int health = (int)(health);
        HpText.text = $"{health}<size=30> / {MaxHealth}</size>";
        lastHpBarFill = Mathf.Lerp(lastHpBarFill, health, Time.deltaTime * 2);
        HpBar.fillAmount = lastHpBarFill / 100;
        energyBar.fillAmount = energy / 100;
        energyText.text = $"{(int)energy}<size=30> / {maxEnergy}</size>";
    }
    public void DamageTaken(int damage)
    {
        damaged = true;
        health -= damage;
        if(health <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects();

        // Tell the animator that the player is dead.
        anim.SetInteger(runParam, 5);

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play();

        // Turn off the movement and shooting scripts.
        GetComponent<CharacterController>().enabled = false;
        //playerShooting.enabled = false;
    }

    //public void AddHealth(int amount)
    //{
    //   if (amount > 0)
    //   {
    //       Health += amount;
    //       if (Health > 100) Health = 100;
    //       HpAddedText hpAddedText = Instantiate(HpAddedTextPrefab, HpText.transform.root);
    //       hpAddedText.SetText(amount);
    //   }
    // }
}