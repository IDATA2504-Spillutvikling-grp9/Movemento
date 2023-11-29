using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttakingUI : MonoBehaviour
{

    [Header("SwordCoolDownSettings")]
    [SerializeField] UnityEngine.UI.Image swordImage;
    [SerializeField] float cooldownDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        swordImage.fillAmount = 0f;
    }

    public void UpdateSliderValue(float startValue, float endValue) {
        StartCoroutine(UpdateSliderOverTime(startValue, endValue, cooldownDuration));
    }

    IEnumerator UpdateSliderOverTime(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            swordImage.fillAmount = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        swordImage.fillAmount = endValue;
    }
}
