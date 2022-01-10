using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapRoom : MonoBehaviour
{
    public bool isInThisRoom = false;//现在是否在房间内,玩家所在房间高亮显示
    public bool haveComeThisRoom = false;//是否来过该房间，只有来过才显示房间边框
}
