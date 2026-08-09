using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using static Define;

public class GameManager
{
    // Map
    public EGameState GameState { get; private set; } = EGameState.Playing;
    public Stage CurrentStage { get; set; }
    public int CurrentStageIndex { get; set; } = -1;

    #region Teleport
    public void TeleportPlayer(Vector3 teleportPosition)
    {
        Managers.Object.Player.transform.position = teleportPosition; 
    }
    #endregion
}
