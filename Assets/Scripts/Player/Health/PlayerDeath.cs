using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private PlayerController pc;
    private PlayerHealth ph;
    private Rigidbody2D rb;
    private SpawnController sc;


    void Start()
    {
        pc = GetComponent<PlayerController>();
        ph = GetComponent<PlayerHealth>();
        sc = GetComponent<SpawnController>();
        rb = GetComponent<Rigidbody2D>();
    }


    // OLE LEGG INN RESPAWN LOGIKKEN DIN
    public IEnumerator Death()
    {
        pc.pState.alive = false;
        Time.timeScale = 1f;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        pc.anim.SetTrigger("Death");

        yield return new WaitForSeconds(3f);
        sc.dieRespawn();
        rb.isKinematic = false;
        ph.health = ph.maxHealth;
        pc.anim.SetTrigger("Respawn");
        pc.pState.alive = true;
    }
}

public class Monobehaviour
{
}