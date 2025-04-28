using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JHTFightScene : MonoBehaviour
{
    [Header("UIMaanger")]
    [SerializeField] GameObject choiceUI;


    [Header("Monster Settings")]
    [SerializeField] Transform myPosition;
    [SerializeField] Transform enemyPosition;
    [SerializeField] GameObject myPoke;
    [SerializeField] GameObject enemyPoke;
    GameObject myPokeInstance;
    GameObject enemyPokeInstance;

    JHTTestExp testExp;
    private void Awake()
    {
        testExp = FindObjectOfType<JHTTestExp>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testExp.GetExp(5);
        }
    }

    public void SetPosition()
    {
        if (myPokeInstance == null) //pokemonStat.isMine && 
        {
            myPokeInstance = Instantiate(myPoke,myPosition.position,Quaternion.Euler(0,180,0));
        }
        if (enemyPokeInstance == null) //!pokemonStat.isMine && 
        {
            enemyPokeInstance = Instantiate(enemyPoke, enemyPosition.position, Quaternion.identity);
        }

        //if (choiceUI != null)
        //{
        //    choiceUI.SetActive(false);
        //}
    }

}
