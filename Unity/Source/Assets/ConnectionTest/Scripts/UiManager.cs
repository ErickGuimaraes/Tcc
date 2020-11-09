using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public InputField inputField;

    public static UiManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ConnectToServer()
    {
        inputField.interactable = false;
        Client.instance.ConnectToServer();
    }
}
