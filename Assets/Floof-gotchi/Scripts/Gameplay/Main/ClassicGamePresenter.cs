using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Floof
{
    public class ClassicGamePresenter : MonoBehaviour
    {
        private PlayView _playView;
        private FloofPresenter _floof;

        public void Setup(PlayView playView, FloofPresenter floof)
        {
            _playView = playView;
            playView.GoToScene(GameSceneType.LivingRoom);

            _floof = floof;

            floof.transform.SetParent(transform);
            floof.Setup(_playView.CurrentScene.MoveSpace);
            floof.StartWandering(2f);
        }
    }
}
