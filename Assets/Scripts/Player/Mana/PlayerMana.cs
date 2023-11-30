using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Mana Setings")]
    [SerializeField] UnityEngine.UI.Image manaStorage;
    [SerializeField] public float manaDrainSpeed;
    [SerializeField] public float manaGain;
    [SerializeField] public float mana;

    private GameManager gameManager;

    

    void Start()
    {
        Mana = mana;
        manaStorage.fillAmount = mana;
        gameManager = FindObjectOfType<GameManager>();
    }



    public float Mana
    {
        get { return mana; }
        set
        {
            //if mana stats change
            if(mana != value)
            {
                mana = Mathf.Clamp(value, 0, 1);
                manaStorage.fillAmount = Mana;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Healing") {
            gameManager.TurnOnAndOfHealingAbilityScreen();
        }
    }

}
