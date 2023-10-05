using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Floof
{
    public enum GameScene
    {
        LivingRoom,
        BathRoom
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ViewManager _viewManager;
        [SerializeField] private AudioManager _audioManager;


        private GameFlowStateMachine _stateMachine;


        private void Awake()
        {
            (_viewManager as IConstructable).Construct();
            (_audioManager as IConstructable).Construct();
        }

        private IEnumerator Start()
        {
            _stateMachine = new GameFlowStateMachine();

            while (true)
            {
                yield return null;
                _stateMachine.StateUpdate();
            }
        }

    }
}