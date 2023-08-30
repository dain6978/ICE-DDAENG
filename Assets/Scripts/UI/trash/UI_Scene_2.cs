using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene_2 : UI_Base
{
    public override void Init()
    {
        // 고정 UI이므로 sorting할 필요 x -> false 전달 
        // 고정 UI의 sort order: 0 (최소값, 가장 아래에서 가장 먼저 그려지도록)
        Managers.UI.SetCanvas(gameObject, false);
    }
}
