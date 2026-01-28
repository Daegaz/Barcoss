using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [Header("UI References")]
    public GameObject loadingScreen; // The Panel containing the loading bar
    public Slider slider;            // The Slider component

    // Call this function from your Button or Trigger    
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        // 1. Turn on the loading screen
        loadingScreen.SetActive(true);

        // 2. Start the load operation in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // 3. While the operation is not finished...
        while (!operation.isDone)
        {
            // Unity's progress stops at 0.9, so we divide by 0.9 to get a 0-1 range
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the slider UI
            slider.value = progress;

            // Wait for the next frame
            yield return null;
        }
    }
}
