using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // Resource ������ ��Ʈ�� �ϴ� path���� T Ÿ���� ������ �ҷ����� ����
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    // path�� GameObject Ÿ���� �������� �����ϰ� �Ҵ�
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
