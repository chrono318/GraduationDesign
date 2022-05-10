using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public PlayerController playerController;
    public Slider slider;
    public static Game instance;
    public Shader RoleShader;

    [HideInInspector]
    public CursorUI cursorSetting;
    //
    public List<DeliveryDoor> deliveryDoors;
    //
    public GameObject MinMap;

    [Header("Room1")]
    public Vector4 roomCameraArea1; //x1,x2,y1,y2
    public List<MoveObject> enemies1;
    [Header("Room2")]
    public Vector4 roomCameraArea2;
    public List<MoveObject> enemies2;
    [Header("Room3")]
    public Vector4 roomCameraArea3;
    public List<MoveObject> enemies3;
    [Header("Room4")]
    public Vector4 roomCameraArea4;
    public List<MoveObject> enemies4;
    [Header("Room5")]
    public Vector4 roomCameraArea5;
    public List<MoveObject> enemies5;

    //
    [Header("Current Room")]
    public int RoomId = 1;
    public int CurEnemyCount;
    public Vector4 curRoomCameraArea;
    public List<MoveObject> curEnemies;
    public Weiqi weiqi;
    public Transform FuShenVXF;
    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        UpdateRoomEnemyList();
    }

    public void UpdateRoomEnemyList()
    {
        switch (RoomId)
        {
            case 1:
                curRoomCameraArea = roomCameraArea1;
                curEnemies = enemies1;
                break;
            case 2:
                curRoomCameraArea = roomCameraArea2;
                curEnemies = enemies2;
                break;
            case 3:
                curRoomCameraArea = roomCameraArea3;
                curEnemies = enemies3;
                break;
            case 4:
                Camera.main.backgroundColor = Color.black;
                curRoomCameraArea = roomCameraArea4;
                curEnemies = enemies4;
                break;
            case 5:
                curRoomCameraArea = roomCameraArea5;
                curEnemies = enemies5;
                break;

        }
        CurEnemyCount = curEnemies.Count;
    }

    //敌人死亡时调用，检查是否通关，通关则出现传送门
    /// <summary>
    /// 敌人数量减一顺便检查是否通关
    /// </summary>
    void CheckIfPass()
    {
        CurEnemyCount = curEnemies.Count;
        KillEnemy();
        if (CurEnemyCount <= 0)
        {
            //通关
            print("通关");
            MinMap.SetActive(true);
            deliveryDoors[RoomId - 1].Pass();
            return;
        }
    }
    public void DeleteEnemyMO(MoveObject moveObject)
    {
        MoveObject _mo = new MoveObject();
        foreach(MoveObject mo in curEnemies)
        {
            if (mo != null)
            {
                if (mo == moveObject)
                {
                    _mo = mo;
                }
            }
        }
        curEnemies.Remove(_mo);
        CheckIfPass();
    }
    /// <summary>
    /// 通知当前房间所有敌人进入攻击状态，目标为player
    /// </summary>
    /// <param name="player"></param>
    public void InformEnemie(MoveObject playerMO)
    {
        foreach (MoveObject enemy in curEnemies)
        {
            if (enemy != null)
            {
                if(enemy.TryGetComponent<EnemyController>(out EnemyController enemyController))
                {
                    enemyController.SetPlayer(playerMO);
                }
            }
        }
    }

    public void TranslateToNewRoom(MoveObject role)
    {
        RoomId++;
        UpdateRoomEnemyList();
        if (role.TryGetComponent<MoveObject>(out MoveObject p))
        {
            InformEnemie(p);
        }
        Game.instance.MinMap.SetActive(false);
    }

    private int killNum = 0;
    public GameObject UITips;
    public void KillEnemy()
    {
        killNum++;
        float s = killNum / 3f;
        slider.value = s;
        if (killNum == 3)
        {
            XinwuPlu();
        }
    }
    private int xinwunum = 10;
    public Text text;
    public XinWuTips XinWuTips;
    public void XinwuPlu()
    {
        GameObject tips = GameObject.Instantiate(UITips);
        tips.GetComponent<UITips>().ShowTips("信物 + 1");
        print(tips.name);
        //to do
        xinwunum++;
        text.text = xinwunum.ToString();
        XinWuTips.level = xinwunum / 3;
        if (xinwunum == 12)
        {
            //player.SetXinwu(true);
        }
        XinWuTips.UpdateText();
    }
}
