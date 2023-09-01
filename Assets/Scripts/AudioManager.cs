using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    public AudioClip moveSFX;
    public AudioClip missSFX;
    public AudioClip matchSFX;
    public AudioClip gameOverSFX;
    public AudioSource SfxSource;

    // Start is called before the first frame update
    void Start()
    {
        // GameManager.Instance.OnPoints
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
