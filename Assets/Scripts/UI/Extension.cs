using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// Ȯ�� �޼���
// : Ư���� ������ static �޼����, ��ġ �ٸ� Ŭ������ �޼����� ��ó�� ȣ���� ����� �� ���� 
// : Ȯ�� �޼����� ù ��° �Ű������� �ٸ� Ŭ������ �޼����� ��ó�� ȣ���� �� �ִ� �� ȣ���� ��ü�� ����

// �״ϱ� Util.cs�� GetOrAddComponent �Լ��� UI_Base.cs�� BindEvent �Լ��� ��ġ GameObject�� ���� �ִ� �޼��带 ȣ���ϴ� ��ó�� ȣ���� �� �ִٴ� �ǹ�
// ���� ���! ���� BindEvent�� UI_Base.cs�� �Լ��ϱ� �ٸ� ���� ����Ϸ��� ui_base.BindEvent(gameObject, action, type) �̷� ������ ���ڷ� �Ѱܼ� ȣ���ؾ� �ϴµ�, gameObject.BindEvent(OnButtonClicked); �䷸�� �ٷ� ����� �����ϴٴ� �ǹ�~!


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
