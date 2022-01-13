using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Slider slider;
    public static Game instance;
    public GameObject defaltBullet;
    public GameObject playerBullet;
    public Shader RoleShader;
    public PhysicsMaterial2D PhysicsMaterial;
    public GameObject circle;
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public CursorUI cursorSetting;
    //
    public List<DeliveryDoor> deliveryDoors;
    //
    public GameObject MinMap;

    [Header("Room1")]
    public List<Enemy> enemies1;
    [Header("Room2")]
    public List<Enemy> enemies2;
    [Header("Room3")]
    public List<Enemy> enemies3;
    [Header("Room4")]
    public List<Enemy> enemies4;
    [Header("Room5")]
    public List<Enemy> enemies5;

    //
    [Header("Current Room")]
    public int RoomId = 1;
    private int CurEnemyCount;
    private List<Enemy> curEnemies;

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
                curEnemies = enemies1;
                break;
            case 2:
                curEnemies = enemies2;
                break;
            case 3:
                curEnemies = enemies3;
                break;
            case 4:
                curEnemies = enemies4;
                break;
            case 5:
                curEnemies = enemies5;
                break;

        }
        CurEnemyCount = curEnemies.Count;
    }

    //敌人死亡时调用，检查是否通关，通关则出现传送门
    /// <summary>
    /// 敌人数量减一顺便检查是否通关
    /// </summary>
    public void CheckIfPass()
    {
        CurEnemyCount--;
        KillEnemy();
        if (CurEnemyCount <= 0)
        {
            //通关
            print("通关");
            MinMap.SetActive(true);
            deliveryDoors[RoomId - 1].gameObject.SetActive(true);
            return;
            if (RoomId >= 4) return;
            deliveryDoors[RoomId-1].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 通知当前房间所有敌人进入攻击状态，目标为player
    /// </summary>
    /// <param name="player"></param>
    public void InformEnemie(Player player)
    {
        foreach(Enemy enemy in curEnemies)
        {
            if (enemy != null)
            {
                enemy.FindPlayer(player);
            }
        }
    }

    public void TranslateToNewRoom(Role role)
    {
        RoomId++;
        UpdateRoomEnemyList();
        if (role.TryGetComponent<Player>(out player))
        {
            InformEnemie(player);
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
            player.SetXinwu(true);
        }
        XinWuTips.UpdateText();
    }
}
