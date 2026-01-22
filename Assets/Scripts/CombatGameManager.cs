using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGameManager : MonoBehaviour
{
    public static CombatGameManager instance;

    public GameState GameState;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        changeState(GameState.GenerateGrid);
    }

    public void UpdateGameState(GameState newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                break;
            case GameState.SpawnPlayer:
                break;
            case GameState.SpawnEnemies:
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemiesTurn:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }


}

public enum GameState
{
    GenerateGrid = 0,
    SpawnPlayer = 1,
    SpawnEnemies = 2,
    PlayerTurn = 3,
    EnemiesTurn = 4
}