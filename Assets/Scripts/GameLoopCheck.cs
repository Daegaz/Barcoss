using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoopCheck : MonoBehaviour
{
    bool hasSwitchedScene = false;
    
    void Start()
    {
        if (SaveData.Instance.isBossDone)
            SceneManager.LoadScene("GameOver");
    }

    void Update()
    {
        if (hasSwitchedScene) return;

        if (SaveData.Instance.HP == 0 || SaveData.Instance.energy == 0)
        {
            hasSwitchedScene = true;
            SceneManager.LoadScene("GameOver");
        }
    }
}
