using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(InputManager.Instance.gameObject);
        DontDestroyOnLoad(AudioManager.Instance.gameObject);
        DontDestroyOnLoad(UIManager.Instance.gameObject);

        Application.targetFrameRate = 60;
    }

}
