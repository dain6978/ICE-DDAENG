//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;


//// UI Event ���� 
//// �ش� ��ũ��Ʈ�� ������ ������Ʈ���� ���콺 & �巡�� ���� �̺�Ʈ�� ���� �� ���� (������ ���� �ƴ� �ڵ�� ����)

//public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
//{
//	// PointerEventData eventData : ����Ƽ �󿡼� �߻��� �°� �̺�Ʈ�� ���õ� ���� ��� ����ü
//	// Action�� : C#�� �⺻���� ����� ���ʸ� ��������Ʈ
//	public Action<PointerEventData> OnClickHandler = null;
//	public Action<PointerEventData> OnDragHandler = null;

//	// ���콺 Ŭ�� �̺�Ʈ : OnClickHandler�� ��ϵ� ��� �Լ� �ڵ� ����
//	public void OnPointerClick(PointerEventData eventData)
//	{
//		if (OnClickHandler != null)
//			OnClickHandler.Invoke(eventData); 
//	}

//	// ���콺 �巡�� �̺�Ʈ : OnDragHandler ��ϵ� ��� �Լ� �ڵ� ����
//	public void OnDrag(PointerEventData eventData) 
//	{
//		if (OnDragHandler != null)
//			OnDragHandler.Invoke(eventData); 
//	}
//}
