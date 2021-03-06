﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirrorMPlayer
{
    public class MPMainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel;

        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }
    }
}