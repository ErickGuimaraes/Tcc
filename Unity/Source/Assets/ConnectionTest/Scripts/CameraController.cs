using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerManager player;
    public float sensitivity = 100f;
    public float clampAngle = 65f;

    private float verticalRotation;
    private float horizontalRotation;

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = transform.localEulerAngles.y;
    }

    private void Update()
    {
        Look();
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }

    private void Look()
    {
        float mouserVertical = -Input.GetAxis("Mouse Y");
        float mouserHorizontal = Input.GetAxis("Mouse X");

        verticalRotation += mouserVertical * sensitivity * Time.deltaTime;
        horizontalRotation += mouserHorizontal * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
    }
}
