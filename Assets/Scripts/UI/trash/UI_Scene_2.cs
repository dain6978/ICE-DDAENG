using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene_2 : UI_Base
{
    public override void Init()
    {
        // ���� UI�̹Ƿ� sorting�� �ʿ� x -> false ���� 
        // ���� UI�� sort order: 0 (�ּҰ�, ���� �Ʒ����� ���� ���� �׷�������)
        Managers.UI.SetCanvas(gameObject, false);
    }
}
