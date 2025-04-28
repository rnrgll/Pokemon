using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Transform playerSpawnPosition;

    Animator anim;

    private float dirX, dirY;
    Vector3 movement;

    void Start()
    {
        transform.position = playerSpawnPosition.position;
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxis("Vertical");

        if (dirX < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else if (dirX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (dirY > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }

        movement = new Vector3(dirX, dirY).normalized;
    }

    private void FixedUpdate()
    {
        rigid.velocity = movement * moveSpeed;
    }
}
