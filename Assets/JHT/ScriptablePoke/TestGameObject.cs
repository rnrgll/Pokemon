using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameObject : MonoBehaviour
{
	public PokeClasses pikacu;
	private PokeManager manager;
    void Start()
    {
		manager = new PokeManager(pikacu);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			manager.GetExp(5);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			manager.TakeDamage(10);
		}
    }
}
