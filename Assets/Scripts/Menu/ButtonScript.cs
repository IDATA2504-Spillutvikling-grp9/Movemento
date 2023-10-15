using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] AudioClip sound_hover;
    [Space(10)] [SerializeField] AudioSource audioSource;

    void Start()
    {
        // Assuming you have assigned an AudioSource component to this script
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void UIHover()
    {
        audioSource.PlayOneShot(sound_hover);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // You can add logic here if you want to stop the sound on hover exit.
    }
}