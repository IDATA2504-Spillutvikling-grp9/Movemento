using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageController : MonoBehaviour
{
    [Header("Hit Detection Settings")]
    [SerializeField] GameObject bloodSpurt;                 // Bloodspurt effect created with Unity's particle system
    [SerializeField] float hitFlashSpeed;                   // Speed of the black/white flash when hit
    public static PlayerDamageController Instance;
    private PlayerController pc;
    private PlayerDeath pd;
    private PlayerHealth ph;
    private SpriteRenderer sr;
    private float restoreTimeSpeed;
    private bool restoreTime;


    
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
    }



    void Start()
    {
        pc = GetComponent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
        pd = GetComponent<PlayerDeath>();
        ph = GetComponent<PlayerHealth>();
    }



    void Update()
    {
        RestoreTimeScale();
        FlashWhileInvincible();
    }



    public void TakeDamage(float _damage)
    {
        if(pc.pState.alive)
        {
            ph.Health -= Mathf.RoundToInt(_damage);
            if(ph.Health <= 0) 
            {
                ph.Health = 0;
                StartCoroutine(pd.Death());
            }
            else
            {
            StartCoroutine(StopTakingDamage());
            }
        }
    }



    IEnumerator StopTakingDamage()
    {
        pc.pState.invincible = true;
        GameObject _bloodSpurtParticles = Instantiate(bloodSpurt, transform.position, Quaternion.identity);
        Destroy(_bloodSpurtParticles, 1.5f);
        pc.anim.SetTrigger("TakeDamage");
        yield return new WaitForSeconds(1f);
        pc.pState.invincible = false;
    }



    void FlashWhileInvincible()
    {
        sr.material.color = pc.pState.invincible ?
        Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * hitFlashSpeed, 1.0f)) : //Pingpong changes it from white to black and black to white as long as parameters are forfilled
        Color.white;
    }



    void RestoreTimeScale()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale += Time.deltaTime * restoreTimeSpeed;
            }
            else
            {
                Time.timeScale = 1;
                restoreTime = false;
            }
        }
    }



    public void HitStopTime(float _newTimeScale, int _restoreSpeed, float _delay)
    {
        restoreTimeSpeed = _restoreSpeed;
        Time.timeScale = _newTimeScale;

        if (_delay > 0)
        {
            StopCoroutine(StartTimeAgain(_delay));
            StartCoroutine(StartTimeAgain(_delay));
        }
        else
        {
            restoreTime = true;
        }
    }



    IEnumerator StartTimeAgain(float _delay)
    {
        restoreTime = true;
        yield return new WaitForSeconds(_delay);
    }
}