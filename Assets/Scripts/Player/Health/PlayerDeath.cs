using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour
{
    private PlayerController pc;
    private PlayerHealth ph;
    


    void Start()
    {
        pc = GetComponent<PlayerController>();
        ph = GetComponent<PlayerHealth>();
    }


    // OLE LEGG INN RESPAWN LOGIKKEN DIN
    public IEnumerator Death()
    {
        pc.pState.alive = false;
        //Ditta gjør du vell i klassen din?
        Time.timeScale = 1f;
        pc.anim.SetTrigger("Death");

        yield return new WaitForSeconds(0.9f);
        //Legg til kode som displayer death screen / respawn at last checkpoint?
    }
}

public class Monobehaviour
{
}