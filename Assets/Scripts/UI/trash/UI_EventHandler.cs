//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;


//// UI Event 연동 
//// 해당 스크립트를 부착한 오브젝트들은 마우스 & 드래그 관련 이벤트를 받을 수 있음 (부착도 툴이 아닌 코드로 관리)

//public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
//{
//	// PointerEventData eventData : 유니티 상에서 발생한 온갖 이벤트와 관련된 정보 담는 구조체
//	// Action은 : C#에 기본으로 선언된 제너릭 델리게이트
//	public Action<PointerEventData> OnClickHandler = null;
//	public Action<PointerEventData> OnDragHandler = null;

//	// 마우스 클릭 이벤트 : OnClickHandler에 등록된 모든 함수 자동 실행
//	public void OnPointerClick(PointerEventData eventData)
//	{
//		if (OnClickHandler != null)
//			OnClickHandler.Invoke(eventData); 
//	}

//	// 마우스 드래그 이벤트 : OnDragHandler 등록된 모든 함수 자동 실행
//	public void OnDrag(PointerEventData eventData) 
//	{
//		if (OnDragHandler != null)
//			OnDragHandler.Invoke(eventData); 
//	}
//}
