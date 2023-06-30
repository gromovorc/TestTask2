using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Level_Loader : MonoBehaviour
{
    [SerializeField] private string levelName;

    [SerializeField] private float totalProgress;
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private TMP_Text _loadingText;

    private void Start()
    {
        StartCoroutine(LoadLevelAsync("CarRoad"));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        while (!operation.isDone)
        {
            totalProgress = Mathf.Clamp01(operation.progress);
            _loadingSlider.value = totalProgress;
            _loadingText.text = (totalProgress * 100f).ToString() + '%';

            yield return null;
        }
    }
}
