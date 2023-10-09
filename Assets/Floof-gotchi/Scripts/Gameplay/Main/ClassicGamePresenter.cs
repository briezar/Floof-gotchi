using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Floof
{
    public class ClassicGamePresenter : MonoBehaviour
    {
        [SerializeField] private CameraFollow _camFollow;
        [SerializeField] private FloofPresenter _floof;
        [SerializeField] private Scene[] _scenePrefabs;

        private PlayView _playView;

        public Scene CurrentScene { get; private set; }
        public Scene[] _scenes;

        public void Init(PlayView playView)
        {
            _playView = playView;

            playView.GetNeeds(NeedsType.Happiness).SetOnClick(() => GoToScene(GameSceneType.LivingRoom));
            playView.GetNeeds(NeedsType.Hygiene).SetOnClick(() => GoToScene(GameSceneType.Bathroom));

            SetupScene();

            GoToScene(GameSceneType.LivingRoom);
        }

        private void SetupScene()
        {
            var sceneCount = _scenePrefabs.Length;
            _scenes = new Scene[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                _scenes[i] = Instantiate(_scenePrefabs[i], transform);
                _scenes[i].Init();
                _scenes[i].gameObject.SetActive(false);
            }
        }


        public void GoToScene(GameSceneType sceneType)
        {
            if (CurrentScene != null)
            {
                if (CurrentScene.SceneType == sceneType) { return; }
                CurrentScene.gameObject.SetActive(false);
            }

            var scene = _scenes[(int)sceneType];

            scene.gameObject.SetActive(true);

            _floof.transform.SetParent(scene.transform);
            _floof.SetMoveSpace(scene.GetMoveSpace());
            _floof.StartWandering(2f);

            _camFollow.SetBounds(scene.GetCameraBounds());
            _camFollow.SetTarget(_floof.transform);

            CurrentScene = scene;
        }
    }
}
