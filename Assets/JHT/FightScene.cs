using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FightScene : MonoBehaviour
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

    Inventory inventory;
    TestPokeClass testPoke;
    private void Awake()
    {
        inventory =FindObjectOfType<Inventory>();
        testPoke = FindObjectOfType<TestPokeClass>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            testPoke.Attack();
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
