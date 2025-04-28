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
    public GameObject myPoke;
    [SerializeField] GameObject enemyPoke;
    GameObject myPokeInstance;
    GameObject enemyPokeInstance;

    Inventory inventory;

    private void Awake()
    {
        inventory =FindObjectOfType<Inventory>();
    }


    public void SetPosition()
    {
        if (myPokeInstance == null) //pokemonStat.isMine && 
        {
            myPokeInstance = Instantiate(myPoke,myPosition.position,Quaternion.identity);
        }
        if (enemyPokeInstance == null) //!pokemonStat.isMine && 
        {
            enemyPokeInstance = Instantiate(enemyPoke, enemyPosition.position, Quaternion.identity);
        }

        if (choiceUI != null)
        {
            choiceUI.SetActive(false);
        }
    }

}
