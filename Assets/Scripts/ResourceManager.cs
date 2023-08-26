using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // Resource 폴더를 루트로 하는 path에서 T 타입의 에셋을 불러오고 리턴
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    // path에 GameObject 타입의 프리팹을 생성하고 할당
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
