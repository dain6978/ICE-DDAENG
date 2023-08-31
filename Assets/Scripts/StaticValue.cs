using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValue : MonoBehaviour
{
    public static StaticValue staticValue;

    public bool leaveGameRoom;
    public string roomName = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (staticValue == null)
        {
            staticValue = this;
        }
    }
}