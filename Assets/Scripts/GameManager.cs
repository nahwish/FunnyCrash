using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    } 
    public int Points = 0;
    public UnityEvent OnPintsUpdated;
    public void AddPoint(int newPoint)
    {
        Points += newPoint;
        OnPintsUpdated?.Invoke();
    }
}
