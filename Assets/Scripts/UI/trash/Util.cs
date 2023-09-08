//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//// UI 바인딩 관련해 컴포넌트 및 오브젝트 Find

//public class Util
//{
//    // GetOrAddCompone 함수 : go 오브젝트의 T 컴포넌트 리턴. 오브젝트에 컴포넌트가 없을 경우 컴포넌트를 추가한 후 리턴
//    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
//    {
//        T component = go.GetComponent<T>();

//        if (component == null)
//            component = go.AddComponent<T>();

//        return component;
//    }

//    // FindChild (게임 오브젝트가 컴포넌트 T를 가질 경우)
//    // : 게임 오브젝트 go의 자식들 중, T component를 가지며 name과 이름이 일치하는 오브젝트를 찾고 리턴
//    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
//    {
//        if (go == null)
//            return null;

//        // recursive가 false일 경우, go의 직속 자식에서 Find 
//        if (recursive == false)
//        {
//            for (int i = 0; i < go.transform.childCount; i++)
//            {
//                Transform transform = go.transform.GetChild(i);
//                if (string.IsNullOrEmpty(name) || transform.name == name)
//                {
//                    T component = transform.GetComponent<T>();
//                    if (component != null)
//                        return component;
//                }
//            }
//        }
//        // recursive가 true일 경우, go의 모든 자식에서 find
//        else
//        {
//            foreach (T component in go.GetComponentsInChildren<T>())
//            {
//                if (string.IsNullOrEmpty(name) || component.name == name)
//                    return component;
//            }
//        }

//        return null; 
//    }

    
//    // FindChild (게임 오브젝트가 컴포넌트를 가지지 않는 빈 오브젝트일 경우)
//    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
//    {
//        Transform transform = FindChild<Transform>(go, name, recursive);
//        if (transform == null)
//            return null;

//        return transform.gameObject;
//    }
//}
