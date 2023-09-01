using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiPints : MonoBehaviour
{
    int displayedPoint = 0;
    public GameObject bronzeMedal;
    public GameObject goldMedal;
    public GameObject silverMedal;
    public TextMeshProUGUI pointsLabel;

    void Start()
    {
        GameManager.Instance.OnPintsUpdated.AddListener(UpdatePoint);
        GameManager.Instance.OnGameStateUpdated.AddListener(GameStateUpdated);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPintsUpdated.RemoveListener(UpdatePoint);
        GameManager.Instance.OnGameStateUpdated.RemoveListener(GameStateUpdated);
    }
    private void GameStateUpdated(GameManager.GameState newState)
    {
        if(newState == GameManager.GameState.GameOver)
        {
            
        }
    }
    void UpdatePoint()
    {
        StartCoroutine(UpdatePointsCoroutine());
    }
    IEnumerator UpdatePointsCoroutine()
    {
        while (displayedPoint < GameManager.Instance.Points)
        {
            displayedPoint++;
            pointsLabel.text = displayedPoint.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }
    private void Update()
    {
        if (displayedPoint >= 30)
        {
            bronzeMedal.SetActive(true);
        }
        if (displayedPoint >= 60)
        {
            silverMedal.SetActive(true);
        }
        if (displayedPoint >= 90)
        {
            goldMedal.SetActive(true);
        }
    }

}
