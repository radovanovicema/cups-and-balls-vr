using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI attemptsText;
    [SerializeField] private Canvas uiCanvas;
    [SerializeField] private Button restartButton;
    [SerializeField] private AudioClip uiClickSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        // Sakrij restart dugme na početku
        if (restartButton != null)
            restartButton.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.alpha = 1f;
        }
    }

    public void UpdateAttempts(int attemptsRemaining)
    {
        if (attemptsText != null)
        {
            attemptsText.text = "Pokušaji: " + attemptsRemaining;
        }
    }

    public void ShowRestartButton(Action onRestartCallback)
    {
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
            
            // Resetuj listener
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() =>
            {
                PlayUISound();
                onRestartCallback?.Invoke();
                restartButton.gameObject.SetActive(false);
            });
        }
    }

    private void OnRestartClicked()
    {
        PlayUISound();
    }

    private void PlayUISound()
    {
        if (audioSource != null && uiClickSound != null)
        {
            audioSource.PlayOneShot(uiClickSound);
        }
    }

    private void OnDestroy()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
        }
    }
}
