using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.unity.mgobe;

public class NetworkManager : MonoBehaviour
{
    #region
    public Button loadBtn;
    #endregion

    public Room room;
    // Start is called before the first frame update
    void Start()
    {
        GameInfoPara gameInfo = new GameInfoPara
        {
            // 替换 为控制台上的“游戏ID”
            GameId = "obg-jynewegh",
            // 玩家 openId
            OpenId = "openid_123_test",
            //替换 为控制台上的“游戏Key”
            SecretKey = "f38fe4d1e6a3449689dd6711b23396f50d8090fb"
        };

        ConfigPara config = new ConfigPara
        {
            // 替换 为控制台上的“域名”
            Url = "jynewegh.wxlagame.com",
            ReconnectMaxTimes = 5,
            ReconnectInterval = 1000,
            ResendInterval = 1000,
            ResendTimeout = 10000,
            IsAutoRequestFrame = false
        };


        // 初始化监听器 Listener
        Listener.Init(gameInfo, config, (ResponseEvent eve) => {
            if (eve.Code == 0)
            {
                Debug.Log("初始化成功");
                // 初始化成功之后才能调用其他 API
                // 查询玩家自己的房间
                RoomInfo roomInfo = new RoomInfo
                {
                    Id = "jxy"
                };
                room = new Room(roomInfo);
                //room.InitRoom("jxy");
                room.GetRoomDetail((ResponseEvent e) => {
                    if (e.Code != 0 && e.Code != 20011)
                    {
                        Debug.Log("初始化失败");
                        Debug.Log("e.code=" + e.Code);
                        Debug.Log("e.message=" + e.Msg);
                    }

                    Debug.Log("查询成功");

                    if (e.Code == 20011)
                    {
                        Debug.Log("玩家不在房间内");
                    }
                    else
                    {
                        // 玩家已在房间内
                        var res = (GetRoomByRoomIdRsp)e.Data;
                        Debug.LogFormat("房间名 {0}", res.RoomInfo.Name);
                    }
                   
                });

                //

                Listener.Add(room);
                room.OnUpdate = RoomUpdate;
             
            }
            else
            {
                Debug.LogFormat("初始化失败: {0}", eve.Code);
            }
            // 初始化广播回调事件
            // ...
        });
    }

    private void RoomUpdate(Room room)
    {
        Debug.Log(room.GetNetworkState(0));
        Debug.Log("Player count is:"+ room.RoomInfo.PlayerList.Count);
    }
    public void Load()
    {
        loadBtn.interactable = false;
        PlayerInfoPara playerInfo = new PlayerInfoPara
        {
            Name = "jxy",
            CustomPlayerStatus = 1,
            CustomProfile = "1"
        };
        MatchRoomPara matchRoomPara = new MatchRoomPara
        {
            PlayerInfo = playerInfo,
            MaxPlayers = 4,
            RoomType = "1"
        };

        room.MatchRoom(matchRoomPara, (ResponseEvent e) =>
        {
            if (e.Code != 0)
            {
                Debug.Log("匹配失败！！！");
                loadBtn.interactable = true;
            }
            else
            {
                Debug.Log("匹配成功！！！");
                loadBtn.gameObject.SetActive(false);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}
