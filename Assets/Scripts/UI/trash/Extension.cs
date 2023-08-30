using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// 확장 메서드
// : 특수한 종료의 static 메서드로, 마치 다른 클래스의 메서드인 것처럼 호출해 사용할 수 있음 
// : 확장 메서드의 첫 번째 매개변수는 다른 클래스의 메서드인 것처럼 호출할 수 있는 그 호출의 주체로 정의

// 그니까 Util.cs의 GetOrAddComponent 함수와 UI_Base.cs의 BindEvent 함수를 마치 GameObject에 원래 있던 메서드를 호출하는 것처럼 호출할 수 있다는 의미
// 예를 들면! 원래 BindEvent는 UI_Base.cs의 함수니까 다른 데서 사용하려면 ui_base.BindEvent(gameObject, action, type) 이런 식으로 인자로 넘겨서 호출해야 하는데, gameObject.BindEvent(OnButtonClicked); 요렇게 바로 사용이 가능하다는 의미~!


public static class Extension 
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }
}
