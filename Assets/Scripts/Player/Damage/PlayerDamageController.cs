using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageController : MonoBehaviour
{
    [Header("Hit Detection Settings")]
    [SerializeField] GameObject bloodSpurt; // Bloodspurt effect prefab for visual feedback when the player takes damage
    [SerializeField] float hitFlashSpeed;   // Speed at which the player sprite flashes when hit
    public static PlayerDamageController Instance; // Singleton instance of this class

    private PlayerController pc;
    private PlayerDeath pd;
    private PlayerHealth ph;
    private SpriteRenderer sr;
    private float restoreTimeSpeed;
    private bool restoreTime;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of this object exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this object if an instance already exists
        }
        else
        {
            Instance = this; // Set this as the Singleton instance
        }
    }

    void Start()
    {
        // Initialize references to other components on the same GameObject
        pc = GetComponent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        pd = GetComponent<PlayerDeath>();
        ph = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Execute these methods every frame
        RestoreTimeScale();
        FlashWhileInvincible();
    }

    public void TakeDamage(float _damage)
    {
        // Handle taking damage if the player is alive
        if (pc.pState.alive)
        {
            // Subtract health and check for death
            ph.Health -= Mathf.RoundToInt(_damage);
            if (ph.Health <= 0)
            {
                ph.Health = 0;
                StartCoroutine(pd.Death()); // Start death process if health is zero
            }
            else
            {
                StartCoroutine(StopTakingDamage()); // Temporarily make the player invincible
            }
        }
    }

    IEnumerator StopTakingDamage()
    {
        // Temporarily make the player invincible and play visual effects
        pc.pState.invincible = true;
        GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f); // Destroy blood spurt effect after a delay
        pc.anim.SetTrigger("TakeDamage"); // Trigger damage animation
        yield return new WaitForSeconds(1f); // Wait for 1 second
        pc.pState.invincible = false; // Revert invincibility
    }

    void FlashWhileInvincible()
    {
        // Flash the player sprite between white and black while invincible
        sr.material.color = pc.pState.invincible ? 
            Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) :
            Color.white; // Reset color to white when not invincible
    }

    void RestoreTimeScale()
    {
        // Gradually restore the time scale back to normal if it has been modified
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1; // Reset time scale to normal
                restoreTime = false; // Stop restoring time scale
            }
        }
    }

    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        // Method to modify the time scale, often used for slow-motion effects
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;

        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay)); // Start restoring time scale after a delay
        }
        else
        {
            restoreTime = true;
        }
    }

    IEnumerator StartTimeAgain(float _delay)
    {
        // Coroutine to delay the restoration of the time scale
        restoreTime = true;
        yield return new WaitForSeconds(_delay); // Wait for the specified delay
    }
}
