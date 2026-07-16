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
    
}
