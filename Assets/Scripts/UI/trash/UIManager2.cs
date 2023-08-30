using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� �� ���� ���� UI ĵ���� �����յ��� ���� �� ���� ����
////  SetCanvas : go ������Ʈ�� ĵ���� ������Ʈ �����ͼ� sort order�� ����
////  Show :  ĵ���� UI ������ ����
////  Close : ĵ���� UI ������Ʈ �ı�


public class UIManager2 : MonoBehaviour
{
    private MouseCursor mouseCursor;


    // ���� �������� ���� �ֱٿ� ����� order (���� UI�� ��ġ�� �ʵ��� 10���� ++�� count)
    // order�� �������� �ʰ� ������ ������, ���� ���� �׷����� ��
    int _order = 10; 

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); // �˾� UI�� ĵ����(������Ʈ)�� ���ÿ� ���� 

    UI_Scene _sceneUI = null; // ���� Ȱ��ȭ�� ���� ĵ���� UI


    // UI�� �������� �׷�ȭ�ϰ� �����ϱ� ���� ����ϴ� ������Ƽ 
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
        canvas.overrideSorting = true; // ��ø ĵ������ �����ϴ� boolean
    
        if (sort) // �˾� UI
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // ���� UI
        { 
            canvas.sortingOrder = 0;
        }
    }

    
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        // ���� UI�� ĵ���� ������ ���� -> ���ε� -> Root�� �ڽ����� ���� -> ����
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        go.transform.SetParent(Root.transform); //�̰��� �ʿ伺��..

        return sceneUI;
    }


    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) 
            name = typeof(T).Name;

        // �˾� UI�� ĵ���� ������ ���� -> ���ε� & ���ÿ� �߰� -> Root�� �ڽ����� ���� -> ����
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

        // ������ peek(top)�� ��ġ�� UI�� ���� (= ���� ���߿� ������ UI�� ���� ����)
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
