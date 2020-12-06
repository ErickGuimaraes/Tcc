using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string userName;

    private float moveSpeed = 5f / Constants.TICKS_PER_SECONDS;
    private bool[] inputs;

    public void Initialize(int id, string userName)
    {
        this.id = id;
        this.userName = userName;

        inputs = new bool[4];
    }
    private void FixedUpdate()
    {

        Vector2 inputDirection = Vector2.zero;
        if (inputs[0])
        {
            inputDirection.y += 1;
        }
        if (inputs[1])
        {
            inputDirection.y -= 1;
        }
        if (inputs[2])
        {
            inputDirection.x -= 1;
        }
        if (inputs[3])
        {
            inputDirection.x += 1;
        }
        Move(inputDirection);
    }

    private void Move(Vector2 inputDirection)
    {

        Vector3 moveDirection = transform.right * inputDirection.x + transform.forward * inputDirection.y;
        transform.position += moveDirection * moveSpeed;
        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }
    public void SetInput(bool[] inputs, Quaternion rotation)
    {
        this.inputs = inputs;
        transform.rotation = rotation;
    }
}
