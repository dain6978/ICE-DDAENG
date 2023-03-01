using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValue : MonoBehaviour
{
    public static StaticValue staticValue;

    public bool leaveGameRoom;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (staticValue == null)
        {
            staticValue = this;
        }
    }

    private void Start()
    {
        leaveGameRoom = false;
        Debug.Log("static value start");
    }
}
