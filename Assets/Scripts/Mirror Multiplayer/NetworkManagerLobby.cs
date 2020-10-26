using Mirror;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace MirrorMPlayer
{
    public class NetworkManagerLobby : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;
        [Scene] [SerializeField] private string menuScene = string.Empty;

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

        [Header("Game")]
        [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;


        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;

        public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
        public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();

        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }

        public override void OnStartClient()
        {
            //normally we should run the method from the base/parent class.
            //but the OnStartClient() from NetworkManager is empty;
            //base.OnStartClient();

            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach(var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            if(numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }

            //.GetActiveScene().name 
            if (SceneManager.GetActiveScene().path != menuScene)
            {
                conn.Disconnect();
                return;
            }
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if(conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                roomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }


            base.OnServerDisconnect(conn);
        }


        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            if(SceneManager.GetActiveScene().path == menuScene)
            {
                bool isLeader = roomPlayers.Count == 0; 

                NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.IsLeader = isLeader;

                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }

        public override void OnStopServer()
        {
            roomPlayers.Clear();
        }

        public void NotifyPlayersOfReadyState()
        {
            foreach(var player in roomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }

        private bool IsReadyToStart() 
        {
            if(numPlayers < minPlayers )
            {
                return false;
            }
            foreach(var player in roomPlayers)
            {
                if(!player.isReady)
                {
                    return false;
                }
            }
            return true;
        }


        public void StartGame()
        {
            if(SceneManager.GetActiveScene().path == menuScene)
            {
                if(!IsReadyToStart())
                { 
                    return;
                }

                ServerChangeScene("Scene_Map_01");
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            //from menu to game
            if(SceneManager.GetActiveScene().path == menuScene
                && newSceneName.StartsWith("Scene_Map"))
            {
                for(int i = roomPlayers.Count -1; i >= 0; i--)
                {
                    var conn = roomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetDisplayName(roomPlayers[i].displayName);

                    NetworkServer.Destroy(conn.identity.gameObject);
                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject,true);
                }
            }

            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if(sceneName.StartsWith("Scene_Map"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
            }
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }
    }
}