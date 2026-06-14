using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public enum GameState { Menu, Showing, Shuffling, Playing, GameOver }

    [SerializeField] private GameState currentState = GameState.Menu;
    [SerializeField] private CupManager cupManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private float showDuration = 3f;
    [SerializeField] private float shuffleDuration = 5f;
    [SerializeField] private int maxAttempts = 3;

    private int currentAttempt = 0;
    private int correctGuesses = 0;
    private List<int> ballPositions = new List<int>();
    private bool gameActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (cupManager == null)
            cupManager = GetComponent<CupManager>();
        if (uiManager == null)
            uiManager = GetComponent<UIManager>();

        StartGame();
    }

    public void StartGame()
    {
        currentState = GameState.Showing;
        currentAttempt = 0;
        correctGuesses = 0;
        gameActive = true;
        ballPositions.Clear();

        // Nasumično odaberi 2 čaše od 5 za loptice
        int[] positions = new int[5];
        for (int i = 0; i < 5; i++) positions[i] = i;
        
        for (int i = 0; i < 2; i++)
        {
            int randomIndex = Random.Range(i, 5);
            int temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
            ballPositions.Add(positions[i]);
        }

        cupManager.SetBallPositions(ballPositions);
        uiManager.ShowMessage("Gledaj gde su loptice!");
        
        StartCoroutine(ShowPhase());
    }

    private IEnumerator ShowPhase()
    {
        // Prikaži gde su loptice
        cupManager.ShowBalls();
        yield return new WaitForSeconds(showDuration);
        
        // Sakrij loptice
        cupManager.HideBalls();
        yield return new WaitForSeconds(1f);

        // Počni sa mešanjem
        currentState = GameState.Shuffling;
        uiManager.ShowMessage("Čašice se mešaju...");
        
        StartCoroutine(ShufflePhase());
    }

    private IEnumerator ShufflePhase()
    {
        yield return cupManager.ShuffleCups(shuffleDuration);
        
        // Počni sa igrom
        currentState = GameState.Playing;
        currentAttempt = 0;
        uiManager.ShowMessage("Sada pokušaj da nađeš loptice!");
        uiManager.UpdateAttempts(maxAttempts - currentAttempt);
        
        gameActive = true;
        cupManager.EnableAllCups();
    }

    public void OnCupSelected(int cupIndex)
    {
        if (!gameActive || currentState != GameState.Playing)
            return;

        currentAttempt++;
        bool hasBall = ballPositions.Contains(cupIndex);

        if (hasBall)
        {
            correctGuesses++;
            uiManager.ShowMessage("✓ Pronašao si loptu!");
            cupManager.HighlightCup(cupIndex, true);
            
            if (correctGuesses >= 2)
            {
                // Svi nalazi su pronađeni
                gameActive = false;
                StartCoroutine(GameOverPhase(true));
            }
        }
        else
        {
            uiManager.ShowMessage("✗ Nema loptice ovde!");
            cupManager.HighlightCup(cupIndex, false);

            if (currentAttempt >= maxAttempts)
            {
                // Iscrpeo pokušaje
                gameActive = false;
                StartCoroutine(GameOverPhase(false));
            }
        }

        uiManager.UpdateAttempts(maxAttempts - currentAttempt);
    }

    private IEnumerator GameOverPhase(bool won)
    {
        currentState = GameState.GameOver;
        gameActive = false;

        if (won)
        {
            uiManager.ShowMessage("🎉 Bravo! Pronašao si obe loptice!");
        }
        else
        {
            uiManager.ShowMessage("💔 Nažalost, nema više pokušaja!");
            cupManager.RevealAllBalls();
        }

        yield return new WaitForSeconds(3f);
        
        uiManager.ShowRestartButton(() => StartGame());
    }

    public GameState GetCurrentState() => currentState;
    public bool IsGameActive() => gameActive;
    public List<int> GetBallPositions() => ballPositions;
}
