using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [HideInInspector] public OnHealthChangedDelegate onHealthChangedCallback;
    public delegate void OnHealthChangedDelegate();
    public int health;                                      // Health stat of the player
    public int maxHealth;                                   // Maximum health the player can have

    // Ole har laget health variables for GUI
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public static PlayerHealth Instance { get; private set; } // Singleton instance



    private void Awake()
    {
        if (Instance != null && Instance != this)   // Check if an instance already exists and if it's not the current one
        {
            Destroy(gameObject);                    // Destroy this GameObject to maintain Singleton pattern          
        }
        else
        {
            Instance = this;                        // Set this instance as the Singleton instance
        }
        Health = maxHealth;                         // Initialize health with the maximum health valu
    }



    void Update()
    {
        UpdateHearts();
    }



    public int Health
    {
        get { return health; }
        set
        {
            if (health != value)
            {
                health = Mathf.Clamp(value, 0, maxHealth);

                if (onHealthChangedCallback != null)
                {
                    onHealthChangedCallback.Invoke();
                }
            }
        }
    }



    /* 
        Ole, methods for updating health in UI.
    */
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
