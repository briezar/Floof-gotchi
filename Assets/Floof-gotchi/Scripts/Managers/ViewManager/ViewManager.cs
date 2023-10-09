using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Floof.ViewManagerControllers;
using Cysharp.Threading.Tasks;

namespace Floof
{
    public class ViewManager : MonoBehaviour, IConstructable
    {
        [field: SerializeField] public Camera UICamera { get; private set; }
        [field: SerializeField] public EventSystem EventSystem { get; private set; }
        [field: SerializeField] public Transform WorldCanvas { get; private set; }

        [SerializeField] private UnityEngine.AddressableAssets.AssetLabelReference _viewLabel;
        [SerializeField] private Transform _viewLayer;
        [SerializeField] private AnimationController _animation;

        private Dictionary<string, PrefabReference> _nameToAddressMap;

        private List<BaseView> _views;

        private Dictionary<string, BaseView> _instantiatedViews;

        public BaseView CurrentView => _views.GetLast();
        public BaseView PreviousView => _views.Count < 2 ? null : _views[_views.Count - 2];

        public static ViewManager Instance { get; private set; }

        public const float DefaultTransitionDuration = 0.3f;
        public static bool IsInteractable => Instance.EventSystem.gameObject.activeInHierarchy;

        private int _blockCount = 0;

        void IConstructable.Construct()
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;

            _instantiatedViews = new Dictionary<string, BaseView>();

            _views = new List<BaseView>();

            foreach (Transform child in _viewLayer)
            {
                if (!child.TryGetComponent<BaseView>(out var view))
                {
                    continue;
                }
                var viewName = view.GetType().Name;
                _instantiatedViews.Add(viewName, view);
                view.OnInstantiate();
            }

            _nameToAddressMap = new();
            var locations = AssetManager.GetLocations(new Address(_viewLabel));
            foreach (var location in locations)
            {
                var key = location.PrimaryKey;
                var startIndex = key.LastIndexOf('/') + 1;
                var viewName = key.Substring(startIndex).Replace(".prefab", "");
                var assetRef = new PrefabReference(key);
                _nameToAddressMap.TryAdd(viewName, assetRef);
            }
        }
        private void OnDestroy()
        {
            Instance = null;
        }


#if UNITY_EDITOR
        private List<string> _setInteractableCallers = new();
        [Button]
        private void CheckInteractableCaller()
        {
            Debug.Log(_setInteractableCallers.JoinElements("\n"));
            _setInteractableCallers.Clear();
        }
#endif

        public static void SetInteractable(bool interactable, bool force = false)
        {
#if UNITY_EDITOR
            var collectStackTrace = true;
            if (collectStackTrace)
            {
                try
                {
                    var callerInfo = GeneralUtils.GetMethodCallerInfo();
                    var interactableState = interactable.ToString().Colorize(interactable ? Color.green : Color.red);

                    callerInfo += $", SetInteractable({interactableState}) - [{DateTime.Now.ToString("HH:mm:ss.F2")}]";

                    Instance._setInteractableCallers.Enqueue(callerInfo);
                    if (Instance._setInteractableCallers.Count > 10)
                    {
                        Instance._setInteractableCallers.Pop();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex);
                }
            }
#endif

            if (force)
            {
                Instance._blockCount = 0;
                Instance.EventSystem.gameObject.SetActive(interactable);
                return;
            }

            if (!interactable)
            {
                Instance.EventSystem.gameObject.SetActive(false);
                Instance._blockCount++;
                return;
            }

            Instance._blockCount--;

            if (Instance._blockCount <= 0)
            {
                Instance._blockCount = 0;
                Instance.EventSystem.gameObject.SetActive(true);
            }
        }

        public static void ShakeCamera(float duration, float strength = 12, int vibrato = 15)
        {
            Instance._animation.ShakeCamera(duration, strength, vibrato);
        }

        public static void FadeTransition(FadeSetting fadeSetting)
        {
            Instance._animation.FadeTransition(fadeSetting);
        }

        public static void EnableLoading(bool enable, string text = "")
        {
            Instance._animation.EnableLoading(enable, text);
        }

        public static T Show<T>() where T : BaseView
        {
            return ShowAsync<T>().GetAwaiter().GetResult();
        }

        public async static UniTask<T> ShowAsync<T>() where T : BaseView
        {
            return await Instance.InternalShow<T>();
        }

        private async UniTask<T> InternalShow<T>() where T : BaseView
        {
            var viewName = typeof(T).Name;

            var view = await GetView<T>();
            if (view == null)
            {
                Debug.LogError("View prefab not found " + viewName);
                return null;
            }

            if (view == CurrentView)
            {
                Debug.LogWarning($"You're trying to show the current view again! [{viewName}]");
                return view;
            }

            _views.Add(view);
            view.gameObject.SetActive(true);
            view.transform.SetAsLastSibling();
            view.OnShow();

            if (view is PopupView)
            {
                var popupView = view as PopupView;
                switch (popupView.ShowPopupBehaviour)
                {
                    case ShowPopupBehaviour.HideLowerPopup:
                        if (PreviousView is PopupView)
                        {
                            Hide(PreviousView);
                        }
                        break;
                }

                if (popupView.ShowPopupBehaviour != ShowPopupBehaviour.DoNothing)
                {
                    _animation.ShowPopupBackgroundDim(DefaultTransitionDuration);
                }
            }

            StartCoroutine(TransitionInRoutine());

            return view;


            IEnumerator TransitionInRoutine()
            {
                SetInteractable(false);
                yield return YieldCollection.WaitForSeconds(view.TransitionInDuration);
                SetInteractable(true);
            }
        }

        private async UniTask<T> GetView<T>() where T : BaseView
        {
            var viewName = typeof(T).Name;

            if (_instantiatedViews == null) return null;

            if (_instantiatedViews.TryGetValue(viewName, out var view))
            {
                return view as T;
            }

            try
            {
                var resource = await AssetManager.PrefabLoader.LoadAssetAsync(GetPrefabReference<T>());
                view = Instantiate(resource, _viewLayer).GetComponent<BaseView>();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return null;
            }

            view.OnInstantiate();

            _instantiatedViews.Add(viewName, view);

            return view as T;
        }

        public static void HideTopView(bool release = false)
        {
            Instance.InternalHide(Instance.CurrentView, release);
        }

        public static void Hide(BaseView view, bool release = false)
        {
            Instance.InternalHide(view, release);
        }

        public static void Hide<T>(bool immediate = false, bool release = false) where T : BaseView
        {
            var viewName = typeof(T).Name;

            if (Instance._instantiatedViews.TryGetValue(viewName, out var baseView))
            {
                if (immediate) { baseView.gameObject.SetActive(false); }
                Instance.InternalHide(baseView, release);
            }
        }

        private void InternalHide(BaseView view, bool release = false)
        {
            var viewName = view.GetType().Name;
            if (view == null)
            {
                Debug.LogError($"[ViewManager] {viewName} is null");
                return;
            };
            if (_views.Count < 2)
            {
                Debug.LogError($"[ViewManager] Current view count is less than 2, cannot hide " + viewName);
                return;
            }

            _views.Remove(view);

            var noMorePopups = !_views.Exists(match => match is PopupView);
            if (noMorePopups) { _animation.HidePopupBackgroundDim(DefaultTransitionDuration); }

            StartCoroutine(TransitionOutRoutine());

            ///
            IEnumerator TransitionOutRoutine()
            {
                view.OnHide();

                SetInteractable(false);
                yield return YieldCollection.WaitForSeconds(view.TransitionOutDuration);
                SetInteractable(true);

                if (view.DestroyOnHide)
                {
                    Destroy(view.gameObject);
                    _instantiatedViews.Remove(viewName);

                    if (release) { AssetManager.PrefabLoader.UnloadAsset(_nameToAddressMap[viewName]); }
                }
                else
                {
                    view.gameObject.SetActive(false);
                }

            }
        }

        public static async UniTask Preload<T>() where T : BaseView
        {
            await AssetManager.PrefabLoader.LoadAssetAsync(GetPrefabReference<T>());
        }

        public static PrefabReference GetPrefabReference<T>() where T : BaseView
        {
            return Instance._nameToAddressMap[typeof(T).Name];
        }

    }
}