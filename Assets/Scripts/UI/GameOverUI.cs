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

    [SerializeField] private Transform cameraTransform;


    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void OnEnable()
    {
        canvas.enabled = false;
        originalRotation = cameraTransform.rotation;
        momController.OnGameOver += HandleGameOver;
    }

    void OnDisable()
    {
        momController.OnGameOver -= HandleGameOver;
    }

    public void HandleGameOver()
    {
        DisableGame();
        StartCoroutine(FaceMom());
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

    private IEnumerator FaceMom()
    {
        float timer = 0f;

        while (timer < 1)
        {
            timer += Time.deltaTime;

            targetRotation = originalRotation * Quaternion.Euler(0, -90f, 0);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, Time.deltaTime * 10f);
            yield return new WaitForEndOfFrame();
        }
    }
}
