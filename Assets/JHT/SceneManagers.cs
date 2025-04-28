using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync("Scene2", LoadSceneMode.Additive);
        }
    }
    public void GetFightScene()
    {
        SceneManager.LoadScene("FightScene", LoadSceneMode.Single);
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);
    }
}
