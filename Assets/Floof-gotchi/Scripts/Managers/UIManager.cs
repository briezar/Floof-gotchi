using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum UILayer
{
    Main,
    Popup,
    Overlay,
}
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private UnityEngine.AddressableAssets.AssetLabelReference _uiLabelRef;
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private Transform _mainCanvasGameRect;


    public Dictionary<UILayer, Transform> UILayers { get; private set; } = new();
    public BaseUI TopUI { get; private set; }
    public Camera UICamera => _uiCamera;

    private List<BaseUI> _currentUIs = new();
    private EventSystem _eventSystem = EventSystem.current;
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

        // StartCoroutine(PreloadUI());

        InputManager.SubscribeInput(KeyCode.Q, () => { ShowUI<MainMenuUI>((ui) => { HideLowerUIs(); }); });
        InputManager.SubscribeInput(KeyCode.W, () => { ShowUI<LoadUI>((ui) => { HideLowerUIs(); }); });

        InputManager.SubscribeInput(KeyCode.A, () => { HideUI<MainMenuUI>(); });
        InputManager.SubscribeInput(KeyCode.S, () => { HideUI<LoadUI>(); });

        InputManager.SubscribeInput(KeyCode.Z, () => { ReleaseUI<MainMenuUI>(); });
        InputManager.SubscribeInput(KeyCode.X, () => { ReleaseUI<LoadUI>(); });

    }

    //     AssetManager.PreloadAssetLabelRef<BaseUI>(_uiLabelRef, (Dictionary<string, BaseUI> value) => { _preloadedUIs = value; });

    private static string GetAddressUI<T>() => "UI/" + typeof(T).Name + ".prefab";

    public static void PreloadUI<T>(Action<T> onComplete) where T : BaseUI
    {
        AssetManager.LoadAsync<T>(GetAddressUI<T>(), onComplete);
    }

    public static void ShowUI<T>(Action<T> onComplete = null) where T : BaseUI
    {
        Instance.ShowUI<T>(UILayer.Main, onComplete, true);
    }

    public static void ShowPopup<T>(Action<T> onComplete = null, bool singleInstance = true) where T : BaseUI
    {
        Instance.ShowUI<T>(UILayer.Popup, onComplete, singleInstance);
    }

    public void ShowUI<T>(UILayer uiLayer, Action<T> onComplete, bool singleInstance = true) where T : BaseUI
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
            ShowUI(targetUI);

            if (singleInstance)
            {
                if (!_currentUIs.Contains(targetUI)) { _currentUIs.Add(targetUI); }
            }
            else
            {
                _currentUIs.Add(targetUI);
            } 

            onComplete?.Invoke(targetUI);
        }
    }

    public BaseUI ShowUI(BaseUI ui)
    {
        ui.transform.SetAsLastSibling();
        ui.gameObject.SetActive(true);

        TopUI = ui;

        InputManager.SubscribeInput(KeyCode.Escape, TopUI.OnBack);

        return ui;
    }



    public static void HideLowerUIs()
    {
        foreach (var ui in Instance._currentUIs)
        {
            if (ui != Instance.TopUI)
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    public static void HideUI<T>() where T : BaseUI
    {
        var ui = GetUI<T>();
        if (ui == null) { return; }

        HideUI(ui);
    }
    public static void HideUI(BaseUI ui)
    {
        ui.gameObject.SetActive(false);
    }

    public static void ShowNextUI()
    {

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
