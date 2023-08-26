using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이벤트 종류별로 enum으로 묶어 정리 (우리 게임에 맞춰서 수정 필요)

public class Define 
{
    public enum UIEvent
    {
        Click,
        Drag,
        // ESC를 추가해야 겠지요
        // Enter도?
    }

    public enum MouseEvent
    {
        Press,
        Click,
    }

    //public enum CameraMode
    //{
    //    QuarterView,
    //}
}
