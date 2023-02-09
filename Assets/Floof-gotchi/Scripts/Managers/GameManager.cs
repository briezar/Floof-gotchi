using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(Instance.gameObject);
        DontDestroyOnLoad(InputManager.Instance.gameObject);
        DontDestroyOnLoad(AudioManager.Instance.gameObject);
        DontDestroyOnLoad(UIManager.Instance.gameObject);

        Application.targetFrameRate = 60;

        UIManager.PreloadUI<PlayUI>();

        UIManager.SetInteractable(false);
        UIManager.ShowUI<LoadUI>((ui) =>
        {
            UIManager.SetInteractable(true);
        });
    }

}
