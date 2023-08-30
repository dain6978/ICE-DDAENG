using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// ��� UI�� �������� �������� ���� �ڵ忡 ���� ��ũ��Ʈ
//// Bind UI : ������Ʈ �̸����� ã�� ���ε����ֱ�
//// Get UI : ������Ʈ ��������
//// BindEvent UI : ������Ʈ�� �̺�Ʈ ����ϱ�


public abstract class UI_Base : MonoBehaviour
{
    // protected: �Ļ� Ŭ�������� ���� ���� 
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();


    // Bind �Լ� : UI enum�� ��� ��� ������Ʈ�� �ε��Ͽ� ���ε��� ����
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        // T �� ���ϴ� ������Ʈ���� Dictionary�� Value�� objects �迭�� ���ҵ鿡 �ϳ��ϳ� �߰�
        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);


            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }


    // Get �Լ� : T ������Ʈ�� ������ idx �ε����� �ش��ϴ¿�����Ʈ�� T Ÿ������ ����
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        // _objects ��ųʸ����� key(Type)�� T�� ã��, �������� ���� ��� false ���� | ������ ��� true ����
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        // ������ ��� objects �迭�� �ش� key�� value�� �����ϰ� ����
        return objects[idx] as T;
    }

    // �Ķ���� idx�� enum�� int�� ����ȯ�ؼ� ����
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }


    // BindEvent �Լ�
    // : go ������Ʈ�� UI_EventHandler�� ������ �̺�Ʈ �ݹ��� ���� �� �ְ� ��
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // go ������Ʈ�� UI_EventHandler ������Ʈ�� ���ٸ� �߰�
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action; // �̹� ��ϵ� �׼����� ���� ���� �����ϱ� ���� ��� ���� �̸� ���ֱ�...
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
