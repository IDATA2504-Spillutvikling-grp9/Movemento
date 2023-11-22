using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private PlayerController pc;
    private PlayerHealth ph;
    
    private SpawnController sc;


    void Start()
    {
        pc = GetComponent<PlayerController>();
        ph = GetComponent<PlayerHealth>();
        sc = GetComponent<SpawnController>();
    }


    // OLE LEGG INN RESPAWN LOGIKKEN DIN
    public IEnumerator Death()
    {
        pc.pState.alive = false;
        //Ditta gjør du vell i klassen din?
        Time.timeScale = 1f;
        pc.anim.SetTrigger("Death");

        yield return new WaitForSeconds(3f);
        sc.dieRespawn();
        ph.health = ph.maxHealth;
        pc.anim.SetTrigger("Respawn");
        pc.pState.alive = true;
    }
}

public class Monobehaviour
{
}