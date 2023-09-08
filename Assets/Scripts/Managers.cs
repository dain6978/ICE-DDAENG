//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Managers : MonoBehaviour
//{
//    static Managers _instance;
//    public static Managers Instance
//    {
//        get
//        {
//            Init();
//            return _instance;
//        }
//    }

//    ResourceManager _resource = new ResourceManager();
//    UIManager2 _ui = new UIManager2();

//    public static ResourceManager Resource { get { return Instance._resource; } }
//    public static UIManager2 UI { get { return Instance._ui; } }


//    void Start()
//    {
//        Init();
//    }

//    static void Init()
//    {
//        if (_instance == null)
//        {
//            GameObject obj = GameObject.Find("@Managers");
//            if (obj == null)
//            {
//                obj = new GameObject { name = "@Managers" };
//                obj.AddComponent<Managers>();
//            }

//            DontDestroyOnLoad(obj);
//            _instance = obj.GetComponent<Managers>();
//        }
//    }
//}

