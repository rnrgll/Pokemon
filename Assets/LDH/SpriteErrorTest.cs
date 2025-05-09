using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteErrorTest : MonoBehaviour
{
	private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
	    sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"이미지 : {sr.sprite.name}");
    }
}
