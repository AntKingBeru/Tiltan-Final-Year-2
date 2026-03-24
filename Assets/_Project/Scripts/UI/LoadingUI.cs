using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text percentageText;

    private void Start()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        var sceneName = SceneLoader.Instance.GetTargetScene();

        var op = SceneManager.LoadSceneAsync(sceneName);
        if (op == null) yield break;
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            var progress = Mathf.Clamp01(op.progress / 0.9f);

            progressBar.fillAmount = progress;
            percentageText.text = $"{(int)(progress * 100)}%";

            if (op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.3f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}