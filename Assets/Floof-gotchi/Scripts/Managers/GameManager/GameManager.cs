using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Floof
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ViewManager _viewManager;
        [SerializeField] private AudioManager _audioManager;


        private GameFlowStateMachine _stateMachine;


        private void Awake()
        {
            DOTween.Init();

            Debug.unityLogger.logEnabled = false;
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#endif
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;

            (_viewManager as IConstructable).Construct();
            (_audioManager as IConstructable).Construct();
        }

        private void Start()
        {
            _stateMachine = new GameFlowStateMachine(this);
        }

    }
}