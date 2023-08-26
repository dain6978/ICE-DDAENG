using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 게임 씬 상의 여러 UI 캔버스 프리팹들의 생성 및 삭제 관리
////  SetCanvas : go 오브젝트의 캔버스 컴포넌트 가져와서 sort order값 세팅
////  Show :  캔버스 UI 프리팹 생성
////  Close : 캔버스 UI 오브젝트 파괴


public class UIManager2 : MonoBehaviour
{
    private MouseCursor mouseCursor;


    // 현재 시점에서 가장 최근에 사용한 order (고정 UI와 겹치지 않도록 10부터 ++로 count)
    // order가 높을수록 늦게 생성된 것으로, 가장 위에 그려져야 함
    int _order = 10; 

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); // 팝업 UI의 캔버스(컴포넌트)를 스택에 저장 

    UI_Scene _sceneUI = null; // 현재 활성화된 고정 캔버스 UI


    // UI를 종류별로 그룹화하고 정리하기 위해 사용하는 프로퍼티 
    public GameObject Root 
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            
            return root;
        }
    }


    private void Start()
    {
        mouseCursor = GetComponent<MouseCursor>();
    }


    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // 중첩 캔버스를 관리하는 boolean
    
        if (sort) // 팝업 UI
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // 고정 UI
        { 
            canvas.sortingOrder = 0;
        }
    }

    
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // 고정 UI의 캔버스 프리팹 생성 -> 바인딩 -> Root의 자식으로 설정 -> 리턴
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        go.transform.SetParent(Root.transform); //이거의 필요성도..

        return sceneUI;
    }


    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) 
            name = typeof(T).Name;

        // 팝업 UI의 캔버스 프리팹 생성 -> 바인딩 & 스택에 추가 -> Root의 자식으로 설정 -> 리턴
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }


    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        // 스택의 peek(top)에 위치한 UI만 삭제 (= 가장 나중에 생성된 UI를 먼저 삭제)
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Failed to Close Popup");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop(); 
        Managers.Resource.Destroy(popup.gameObject); 
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
}
