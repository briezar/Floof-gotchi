using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UILayer
{
    Main,
    Popup,
    Overlay,
}
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    // [SerializeField] private UnityEngine.AddressableAssets.AssetLabelReference _uiLabelRef;
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Transform _mainCanvasGameRect;

    public Dictionary<UILayer, Transform> UILayers { get; private set; } = new();
    public BaseUI TopUI => _currentUIs.GetLast();
    public Camera UICamera => _uiCamera;
    public static bool IsInteractable => Instance._blockCount <= 0;

    private List<BaseUI> _currentUIs = new();
    private Dictionary<string, BaseUI> _preloadedUIs = new();

    private int _blockCount = 0;

    private void Awake()
    {
        var sampleLayer = _mainCanvasGameRect.GetChild(0);
        var layerNames = Enum.GetNames(typeof(UILayer));

        for (var i = 0; i < layerNames.Length; i++)
        {
            var newLayer = Transform.Instantiate(sampleLayer, _mainCanvasGameRect);
            newLayer.name = "Layer" + layerNames[i];
            UILayers.Add((UILayer)i, newLayer);
        }
        sampleLayer.DestroyGameObject();

        // AssetManager.PreloadAssetLabelRef<BaseUI>(_uiLabelRef, (value) => { _preloadedUIs = value; });

        InputManager.SubscribeInput(KeyCode.Q, () => { ShowUI<PlayUI>(); });
        InputManager.SubscribeInput(KeyCode.W, () => { ShowUI<LoadUI>(); });

        InputManager.SubscribeInput(KeyCode.A, () => { HideUI<PlayUI>(); });
        InputManager.SubscribeInput(KeyCode.S, () => { HideUI<LoadUI>(); });

        InputManager.SubscribeInput(KeyCode.Z, () => { ReleaseUI<PlayUI>(); });
        InputManager.SubscribeInput(KeyCode.X, () => { ReleaseUI<LoadUI>(); });

        InputManager.SubscribeInput(KeyCode.Escape, () =>
        {
            if (TopUI != null) { TopUI.OnBack(); }
        });

    }

    //     AssetManager.PreloadAssetLabelRef<BaseUI>(_uiLabelRef, (Dictionary<string, BaseUI> value) => { _preloadedUIs = value; });

    private static string GetAddressUI<T>() => "UI/" + typeof(T).Name + ".prefab";

    public static void PreloadUI<T>(Action<T> onComplete = null) where T : BaseUI
    {
        onComplete += (ui) => { ui.gameObject.SetActive(false); };
        ShowUI<T>(onComplete, false);

        // AssetManager.LoadAsync<T>(GetAddressUI<T>(), onComplete);
    }

    public static void ShowUI<T>(Action<T> onComplete = null, bool hideLowerUI = true) where T : BaseUI
    {
        Instance.ShowUI<T>(UILayer.Main, onComplete, hideLowerUI, true);
    }

    public static void ShowPopup<T>(Action<T> onComplete = null, bool singleInstance = false) where T : BaseUI
    {
        Instance.ShowUI<T>(UILayer.Popup, onComplete, false, singleInstance);
    }

    public void ShowUI<T>(UILayer uiLayer, Action<T> onComplete, bool hideLowerUI, bool singleInstance = true) where T : BaseUI
    {
        T ui = null;
        bool instantiate = !singleInstance;

        if (singleInstance)
        {
            ui = GetUI<T>();
            if (ui == null) { instantiate = true; }
        }

        if (instantiate)
        {
            AssetManager.InstantiateAsync<T>(GetAddressUI<T>(), UILayers[uiLayer], OnComplete);
            // ui = GameObject.Instantiate<T>((T)_preloadedUIs[typeof(T).Name], UILayers[uILayer]);
        }
        else
        {
            OnComplete(ui);
        }

        void OnComplete(T targetUI)
        {
            if (singleInstance)
            {
                if (_currentUIs.Contains(targetUI))
                {
                    _currentUIs.Remove(targetUI);
                    _currentUIs.Add(targetUI);
                }
                else
                {
                    _currentUIs.Add(targetUI);
                }
            }
            else
            {
                _currentUIs.Add(targetUI);
            }

            ShowUI(targetUI).SetLayer(uiLayer);

            if (hideLowerUI)
            {
                HideLowerUI();
            }

            onComplete?.Invoke(targetUI);
        }
    }

    public BaseUI ShowUI(BaseUI ui)
    {
        if (!_currentUIs.Contains(ui)) { Debug.LogError($"CurrentUI does not contain {ui.name}!"); }

        ui.transform.SetAsLastSibling();
        ui.gameObject.SetActive(true);

        ui.OnShow();

        return ui;
    }

    public static void HideLowerUI()
    {
        var count = Instance._currentUIs.Count;
        if (count <= 1) { return; }

        var index = count - 2;
        var ui = Instance._currentUIs[index];
        HideUI(ui);
    }

    public static void HideUI<T>() where T : BaseUI
    {
        var ui = GetUI<T>();
        if (ui == null) { return; }

        HideUI(ui);
    }
    public static void HideUI(BaseUI ui)
    {
        if (!ui.gameObject.activeInHierarchy) { return; }

        ui.OnHide();
        ui.gameObject.SetActive(false);
    }

    public void ShowNextUI()
    {
        if (_currentUIs.Count <= 1) { return; }

        var lastTopUI = TopUI;

        _currentUIs.Remove(lastTopUI);
        _currentUIs.Insert(0, lastTopUI);

        HideUI(lastTopUI);
        ShowUI(TopUI);
    }

    public static void ReleaseUI<T>() where T : BaseUI
    {
        var ui = GetUI<T>();
        if (ui == null) { return; }

        ReleaseUI(ui);
    }
    public static void ReleaseUI(BaseUI ui)
    {
        Instance._currentUIs.Remove(ui);
        ui.DestroyGameObject();
    }

    private static T GetUI<T>() where T : BaseUI
    {
        T capturedUI = null;
        foreach (var ui in Instance._currentUIs)
        {
            if (ui as T != null)
            {
                capturedUI = (T)ui;
                break;
            }
        }
        return capturedUI;
    }

    public static void SetInteractable(bool interactable, bool force = false)
    {
        if (force)
        {
            Instance._eventSystem.gameObject.SetActive(interactable);
            return;
        }

        if (interactable)
        {
            Instance._blockCount--;
            if (Instance._blockCount <= 0)
            {
                Instance._blockCount = 0;
                Instance._eventSystem.gameObject.SetActive(true);
            }
            return;
        }

        Instance._eventSystem.gameObject.SetActive(false);
        Instance._blockCount++;

    }


}
