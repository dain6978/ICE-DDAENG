using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

//public class UI_Button : UI_Popup
//{
//    enum Buttons
//    {
//        CloseButton,
//        OpenButton,
//    }

//    enum Texts
//    {
//    }

//    enum GameObjects
//    {
//    }

//    enum Images
//    {
//    }

//    private void Start()
//    {
//        Init();
//    }


//    public override void Init()
//    {
//        base.Init();

//        Bind<Button>(typeof(Buttons));
//        Bind<Text>(typeof(Texts));
//        Bind<GameObject>(typeof(GameObjects));
//        Bind<Image>(typeof(Images));

//        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnCloseButtonClicked);
//        GetButton((int)Buttons.OpenButton).gameObject.BindEvent(OnOpenButtonClicked);
//    }
    

//    public void OnCloseButtonClicked(PointerEventData data)
//    {
//        Debug.Log("클로즈버튼");
//    }

//    public void OnOpenButtonClicked(PointerEventData data)
//    {
//        Debug.Log("오픈버튼");
//    }
//}
