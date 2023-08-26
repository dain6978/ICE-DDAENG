using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// 모든 UI의 조상이자 공통으로 갖는 코드에 대한 스크립트
//// Bind UI : 오브젝트 이름으로 찾아 바인딩해주기
//// Get UI : 오브젝트 가져오기
//// BindEvent UI : 오브젝트에 이벤트 등록하기


public abstract class UI_Base : MonoBehaviour
{
    // protected: 파생 클래스에서 접근 가능 
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();


    // Bind 함수 : UI enum에 담긴 모든 오브젝트를 로드하여 바인딩해 보관
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        // T 에 속하는 오브젝트들을 Dictionary의 Value인 objects 배열의 원소들에 하나하나 추가
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


    // Get 함수 : T 컴포넌트를 가지며 idx 인덱스에 해당하는오브젝트를 T 타입으로 리턴
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        // _objects 딕셔너리에서 key(Type)가 T를 찾고, 존재하지 않을 경우 false 리턴 | 존재할 경우 true 리턴
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        // 존재할 경우 objects 배열에 해당 key의 value를 저장하고 리턴
        return objects[idx] as T;
    }

    // 파라미터 idx는 enum을 int로 형변환해서 전달
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }


    // BindEvent 함수
    // : go 오브젝트에 UI_EventHandler를 부착해 이벤트 콜백을 받을 수 있게 함
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // go 오브젝트에 UI_EventHandler 컴포넌트가 없다면 추가
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action; // 이미 등록된 액션으로 인한 오류 방지하기 위해 등록 전에 미리 빼주기...
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
