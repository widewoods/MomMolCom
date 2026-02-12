using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectsToDisable;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private MomController momController;


    void OnEnable()
    {
        canvas.enabled = false;
        momController.OnGameOver += HandleGameOver;
    }

    void OnDisable()
    {
        momController.OnGameOver -= HandleGameOver;
    }

    public void HandleGameOver()
    {
        DisableGame();
        StartCoroutine(FadeToBlack(1f));
    }

    private void DisableGame()
    {
        foreach (GameObject gameObject in gameObjectsToDisable)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeToBlack(float fadeDuration)
    {
        canvas.enabled = true;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            background.color = new Color(0.05f, 0.05f, 0.05f, timer / fadeDuration);
            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, timer / fadeDuration);
            yield return new WaitForEndOfFrame();
        }
    }
}
