using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class LevelSelector : MonoBehaviour
{
    [SerializeField] float delayBeforeLoading = 3f;
    public void LoadScene(string name)
    {
        StartCoroutine(WaitToLodeLevel(name));
    }

    IEnumerator WaitToLodeLevel(string name)
    {
        yield return new WaitForSeconds(delayBeforeLoading);
        SceneManager.LoadScene(name);
    }
}