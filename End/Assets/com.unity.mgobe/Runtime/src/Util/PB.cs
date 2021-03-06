using System;
using System.Collections.Generic;
using Google.Protobuf;
using GooglePB = global::Google.Protobuf;

namespace com.unity.mgobe.src.Util {
    public struct DecodeBstResult {
        public ServerSendClientBstWrap1 BstWrap1 { get; set; }

        public ServerSendClientBstWrap2 BstWrap2 { get; set; }

        public object Body { get; set; }
    }

    public struct DecodeRspResult {
        // public byte[] body;
        public DecodeRspResult (ClientSendServerRspWrap1 wrap1, ClientSendServerRspWrap2 wrap2, object data) : this()
        {
            RspWrap1 = wrap1;
            RspWrap2 = wrap2;
            Body = data;
        }

        public ClientSendServerRspWrap1 RspWrap1 { get; set; }

        public ClientSendServerRspWrap2 RspWrap2 { get; set; }

        public object Body { get; set; }
    }

    public class Pb {
        
        public static Dictionary<int, Func<ByteString, object>> rspDic = new Dictionary<int, Func<ByteString, object>> ();
        public static Dictionary<int, Func<ByteString, object>> bstDic = new Dictionary<int, Func<ByteString, object>> ();

        public static void Init () {

            if (rspDic.Count + bstDic.Count != 0)
            {
                return;
            }
            
            // 设置解包方法
            ///////////////////////////// 响应 /////////////////////////////
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdLoginReq, (data) => convert(data, new LoginRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdLogoutReq, (data) => convert(data, new LogoutRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdChangePlayerStateReq, (data) => convert(data, new ChangeCustomPlayerStatusRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdChangeRoomPlayerProfile, (data) => convert(data, new ChangeRoomPlayerProfileRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdRelayClientSendtoGamesvrReq, (data) => convert(data, new SendToGameSvrRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdRelaySendFrameReq, (data) => convert(data, new SendFrameRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdRoomChatReq, (data) => convert(data, new SendToClientRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdCheckLoginReq, (data) => convert(data, new CheckLoginRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdRelayRequestFrameReq, (data) => convert(data, new RequestFrameRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdStartFrameSyncReq, (data) => convert(data, new StartFrameSyncRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdStopFrameSyncReq, (data) => convert(data, new StopFrameSyncRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdCreateRoomReq, (data) => convert(data, new CreateRoomRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdJoinRoomReq, (data) => convert(data, new JoinRoomRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdQuitRoomReq, (data) => convert(data, new LeaveRoomRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdDissmissRoomReq, (data) => convert(data, new DismissRoomRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdChangeRoomPropertisReq, (data) => convert(data, new ChangeRoomRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdRemoveMemberReq, (data) => convert(data, new RemovePlayerRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdGetRoomDetailReq, (data) => convert(data, new GetRoomByRoomIdRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdGetRoomListReq, (data) => convert(data, new GetRoomListRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdGetRoomListV2Req, (data) => convert(data, new GetRoomListRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdHeartBeatReq, (data) => convert(data, new HeartBeatRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdMatchPlayerComplexReq, (data) => convert(data, new MatchPlayersRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdMatchGroupReq, (data) => convert(data, new MatchGroupRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdMatchRoomSimpleReq, (data) => convert(data, new MatchRoomSimpleRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdMatchCancelMatchReq, (data) => convert(data, new CancelPlayerMatchRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdCreateGroupReq, (data) => convert(data, new CreateGroupRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdJoinGroupReq, (data) => convert(data, new JoinGroupRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdQuitGroupReq, (data) => convert(data, new LeaveGroupRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdDismissGroupReq, (data) => convert(data, new DismissGroupRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdChangeGroupPropertiesReq, (data) => convert(data, new ChangeGroupRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdRemoveGroupMemberReq, (data) => convert(data, new RemoveGroupPlayerRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdGetGroupDetailReq, (data) => convert(data, new GetGroupByGroupIdRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdGetGroupListReq, (data) => convert(data, new GetMyGroupsRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdChangeGroupPlayerStateReq, (data) => convert(data, new ChangeCustomGroupPlayerStatusRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdChangeGroupPlayerProfile, (data) => convert(data, new ChangeGroupPlayerProfileRsp()));
            rspDic.Add((int) ClientSendServerReqWrap2Cmd.ECmdGroupChatReq, (data) => convert(data, new SendToGroupClientRsp()));
           
            ///////////////////////////// 广播 /////////////////////////////
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeGamesvr, (data) => convert(data, new RecvFromGameSvrBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeRoomChat, (data) => convert(data, new RecvFromClientBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeStartGame, (data) => convert(data, new StartFrameSyncBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeStopGame, (data) => convert(data, new StopFrameSyncBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeRelay, (data) => convert(data, new RecvFrameBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeJoinRoom, (data) => convert(data, new JoinRoomBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeLeaveRoom, (data) => convert(data, new LeaveRoomBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeDismissRoom, (data) => convert(data, new DismissRoomBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeModifyRoomProperty, (data) => convert(data, new ChangeRoomBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeRemovePlayer, (data) => convert(data, new RemovePlayerBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypePlayerState, (data) => convert(data, new ChangeCustomPlayerStatusBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeModifyRoomPlayerProfile, (data) => convert(data, new ChangeRoomPlayerProfileBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeNetworkState, (data) => convert(data, new ChangePlayerNetworkStateBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeMatchTimeout, (data) => convert(data, new MatchTimeoutBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeMatchSuccess, (data) => convert(data, new MatchPlayersBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeMatchCancel, (data) => convert(data, new CancelMatchBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeJoinGroup, (data) => convert(data, new JoinGroupBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeLeaveGroup, (data) => convert(data, new LeaveGroupBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeDismissGroup, (data) => convert(data, new DismissGroupBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeModifyGroupProperty, (data) => convert(data, new ChangeGroupBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeRemoveGroupPlayer, (data) => convert(data, new RemoveGroupPlayerBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeGroupPlayerState, (data) => convert(data, new ChangeCustomGroupPlayerStatusBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeModifyGroupPlayerProfile, (data) => convert(data, new ChangeGroupPlayerProfileBst()));
            bstDic.Add((int) ServerSendClientBstWrap2Type.EPushTypeGroupChat, (data) => convert(data, new RecvFromGroupClientBst()));
        }

        private static object convert(ByteString data, GooglePB::IMessage tmp)
        {
            tmp.MergeFrom ((ByteString) data);
            return tmp;
        }

        public static byte[] EncodeReq (ClientSendServerReqWrap1 wrap1, ClientSendServerReqWrap2 wrap2, GooglePB::IMessage data) {
            wrap2.Body = data.ToByteString ();
            wrap1.Body = wrap2.ToByteString ();
            return wrap1.ToByteArray ();
        }

        public static DecodeRspResult DecodeRsp (byte[] data, Func<string, int> getReqCmd) {
            var wrap1 = new ClientSendServerRspWrap1 ();
            wrap1.MergeFrom (data);
            var wrap2 = new ClientSendServerRspWrap2 ();
            wrap2.MergeFrom (wrap1.Body);
            
            object rsp = null;
            int cmd = getReqCmd(wrap1.Seq);

            if (cmd > 0 && rspDic.ContainsKey(cmd) && wrap2.Body != null)
            {
                Func<ByteString,object> func = null;
                rspDic.TryGetValue(cmd, out func);

                if (func != null)
                {
                    rsp = func(wrap2.Body);
                }
            }

            var rspResult = new DecodeRspResult {
                RspWrap1 = new ClientSendServerRspWrap1 (wrap1),
                RspWrap2 = new ClientSendServerRspWrap2 (wrap2),
                Body = rsp
            };
            return rspResult;
        }

        public static DecodeBstResult DecodeBst (byte[] data) {
            var wrap1 = new ServerSendClientBstWrap1 ();
            wrap1.MergeFrom (data);
            var wrap2 = new ServerSendClientBstWrap2 ();
            wrap2.MergeFrom (wrap1.Body);

            object rsp =null;

            if (bstDic.ContainsKey((int)wrap2.Type))
            {
                Func<ByteString,object> func = null;
                bstDic.TryGetValue((int)wrap2.Type, out func); 
                
                if (func != null)
                {
                    rsp = func(wrap2.Msg);
                }
            }

            return new DecodeBstResult {
                BstWrap1 = wrap1,
                BstWrap2 = wrap2,
                Body = rsp
            };
        }
    }
}