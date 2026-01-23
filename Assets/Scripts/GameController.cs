using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameState CurrentState { get; private set; }

    [Header("Player Stats")]
    public int maxHP = 100;
    public int hp;
    public int resources = 10;
    public int gold = 0;

    [Header("Map")]
    public MapState mapState;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        hp = maxHP;
        SetState(GameState.Exploring);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.Exploring:
                ExplorationManager.Instance?.EnableExploration(true);
                CombatManager.Instance?.EnableCombat(false);
                SceneManager.LoadScene("Exploration");
                break;

            case GameState.Combat:
                ExplorationManager.Instance?.EnableExploration(false);
                CombatManager.Instance?.EnableCombat(true);
                SceneManager.LoadScene("Nicolas 1");
                break;

            case GameState.GameOver:
                ExplorationManager.Instance?.EnableExploration(false);
                CombatManager.Instance?.EnableCombat(false);
                SceneManager.LoadScene("GameOver");
                break;
        }
    }

    public void ChangeHP(int amount)
    {
        hp += amount;
        hp = Mathf.Clamp(hp, 0, maxHP);

        CheckLoseCondition();
    }

    public void ChangeResources(int amount)
    {
        resources += amount;
        resources = Mathf.Max(0, resources);

        CheckLoseCondition();
    }

    private void CheckLoseCondition()
    {
        if (hp <= 0 || resources <= 0)
        {
            SetState(GameState.GameOver);
        }
    }

    public void EnterCombat()
    {
        SetState(GameState.Combat);
    }

    public void ExitCombat()
    {
        SetState(GameState.Exploring);
    }

    private void OnGameOver()
    {
        Debug.Log("GAME OVER!");

        // Option A: show a UI overlay
        //GameOverUI.Instance.Show();

        // Option B: load a GameOver scene
        // SceneManager.LoadScene("GameOverScene");

        // Option C: stop all gameplay logic
        // Could also disable managers or player controller here
    }
}
public enum GameState
{
    Exploring,
    Combat,
    GameOver
}

[System.Serializable]
public class MapNode
{
    public int id;
    public NodeType type;
    public List<int> connections = new List<int>();
    public bool visited = false;
    public bool unlocked = false;
}

public enum NodeType
{
    Combat,
    Shop,
    Event,
    Rest,
    Boss
}

[System.Serializable]
public class MapState
{
    public Dictionary<int, MapNode> nodes = new Dictionary<int, MapNode>();
    public int currentNodeId;

    public void VisitNode(int nodeId)
    {
        if (!nodes.ContainsKey(nodeId)) return;
        currentNodeId = nodeId;
        nodes[nodeId].visited = true;
    }

    public bool IsVisited(int nodeId)
    {
        return nodes.ContainsKey(nodeId) && nodes[nodeId].visited;
    }

    public bool IsUnlocked(int nodeId)
    {
        return nodes.ContainsKey(nodeId) && nodes[nodeId].unlocked;
    }

    public void UnlockNode(int nodeId)
    {
        if (!nodes.ContainsKey(nodeId)) return;
        nodes[nodeId].unlocked = true;
    }
}
