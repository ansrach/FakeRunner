using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class MyNetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>(); //สิ่งที่ user กรอกใน lobby
        SetupLocalPlayer actualPlayer = gamePlayer.GetComponent<SetupLocalPlayer>(); //สิ่งที่อยู๋ในเกมจริง ๆ
        actualPlayer.pName = lobby.playerName;
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
    }
}
