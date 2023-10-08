using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dre0Dru.AddressableAssets.Loaders;
using UnityEngine;

namespace Floof
{
    [CreateAssetMenu]
    public class ClassicGameAsset : ScriptableObject
    {
        [SerializeField] private PrefabReference<ClassicGamePresenter> _gamePresenterRef;
        [SerializeField] private PrefabReference<FloofPresenter> _floofPresenterRef;
        [SerializeField] private PrefabReference<Scene>[] _scenesPrefabRef;

        private List<PrefabReference> _allReferences;

        private void SetReferences()
        {
            _allReferences = new() { _gamePresenterRef, _floofPresenterRef };
            _allReferences.AddRange(_scenesPrefabRef);
        }

        public async UniTask LoadAll()
        {
            SetReferences();
            await AssetManager.PrefabLoader.LoadAssetsAsync(_allReferences);
        }

        public void UnloadAll()
        {
            AssetManager.PrefabLoader.UnloadAssets(_allReferences);
        }

        public ClassicGamePresenter GetClassicGamePresenter()
        {
            return _gamePresenterRef.Component;
        }

        public FloofPresenter GetFloofPresenter()
        {
            return _floofPresenterRef.Component;
        }

        public Scene GetScene(GameSceneType gameSceneType)
        {
            return _scenesPrefabRef[(int)gameSceneType].Component;
        }

    }
}
