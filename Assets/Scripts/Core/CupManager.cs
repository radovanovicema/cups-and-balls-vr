using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CupManager : MonoBehaviour
{
    [SerializeField] private Cup[] cups = new Cup[5];
    [SerializeField] private float shuffleSpeed = 2f;
    [SerializeField] private float shuffleMoveDistance = 2f;
    
    private List<int> ballPositions = new List<int>();
    private Vector3[] originalPositions = new Vector3[5];

    private void Start()
    {
        // Pronađi sve čaše u sceni
        cups = GetComponentsInChildren<Cup>();
        
        if (cups.Length != 5)
        {
            Debug.LogError("CupManager: Trebalo bi 5 čaša, pronađeno: " + cups.Length);
        }

        // Spremi originalne pozicije
        for (int i = 0; i < cups.Length; i++)
        {
            originalPositions[i] = cups[i].transform.position;
            cups[i].SetIndex(i);
        }
    }

    public void SetBallPositions(List<int> positions)
    {
        ballPositions = new List<int>(positions);
        
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].SetHasBall(ballPositions.Contains(i));
        }
    }

    public void ShowBalls()
    {
        foreach (var cup in cups)
        {
            if (cup.HasBall())
            {
                cup.ShowBall();
            }
        }
    }

    public void HideBalls()
    {
        foreach (var cup in cups)
        {
            cup.HideBall();
        }
    }

    public void RevealAllBalls()
    {
        foreach (var cup in cups)
        {
            cup.ShowBall();
        }
    }

    public IEnumerator ShuffleCups(float duration)
    {
        float elapsedTime = 0f;
        List<int> cupIndices = new List<int> { 0, 1, 2, 3, 4 };

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Nasumično pomicanje čaša
            int randomIndex1 = Random.Range(0, 5);
            int randomIndex2 = Random.Range(0, 5);

            if (randomIndex1 != randomIndex2)
            {
                // Zameni pozicije
                StartCoroutine(SwapCups(randomIndex1, randomIndex2, 0.3f));
            }

            yield return new WaitForSeconds(0.3f);
        }

        // Vrati čaše na originalne pozicije nakon mešanja
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].transform.position = originalPositions[i];
        }
    }

    private IEnumerator SwapCups(int index1, int index2, float duration)
    {
        Vector3 pos1 = cups[index1].transform.position;
        Vector3 pos2 = cups[index2].transform.position;
        
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            cups[index1].transform.position = Vector3.Lerp(pos1, pos2, t);
            cups[index2].transform.position = Vector3.Lerp(pos2, pos1, t);

            yield return null;
        }

        cups[index1].transform.position = pos2;
        cups[index2].transform.position = pos1;

        // Zameni podatke o loptici između čaša
        bool tempBall = cups[index1].HasBall();
        cups[index1].SetHasBall(cups[index2].HasBall());
        cups[index2].SetHasBall(tempBall);
    }

    public void EnableAllCups()
    {
        foreach (var cup in cups)
        {
            cup.EnableInteraction();
        }
    }

    public void DisableAllCups()
    {
        foreach (var cup in cups)
        {
            cup.DisableInteraction();
        }
    }

    public void HighlightCup(int index, bool correct)
    {
        if (index >= 0 && index < cups.Length)
        {
            cups[index].Highlight(correct);
        }
    }

    public Cup GetCup(int index)
    {
        if (index >= 0 && index < cups.Length)
            return cups[index];
        return null;
    }

    public void ResetAllCups()
    {
        foreach (var cup in cups)
        {
            cup.Reset();
        }
    }
}
