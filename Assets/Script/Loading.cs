using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadScene("menu"));
    }

    private IEnumerator LoadScene(string name)
    {
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
