using System;
using Core;
using Data;
using Managers.Interfaces;
using Settings;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainApp : MonoBehaviour
{
    public event Action LateUpdateEvent;
    public event Action FixedUpdateEvent;
    public event Action UpdateEvent;

    private static MainApp _instance;

    public static MainApp Instance
    {
        get => _instance;
        private set => _instance = value;
    }
    
    [SerializeField] private GameObject canvas;
    public GameObject Canvas => canvas;

    [SerializeField] private PrefabsConfig prefabsConfig;
    public PrefabsConfig PrefabsConfig => prefabsConfig;
    
    private GameObject _eventSystem;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        _eventSystem = EventSystem.current.gameObject;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        if (Instance != this) return;

        ResourceLoading();
    }

    private void ResourceLoading()
    {
        ResourceLoader.Init();

        GameClient.Instance.InitServices();
        GameClient.Get<IGameplayManager>().ChangeAppState(Enumerators.AppState.MainMenu);
    }

    private void Update()
    {
        if (Instance == this)
        {
            UpdateEvent?.Invoke();
        }
    }
    
    public void LockClick(bool isActive)
    {
        _eventSystem.SetActive(isActive);
    }

    private void LateUpdate()
    {
        if (Instance == this)
        {
            LateUpdateEvent?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (Instance == this)
        {
            FixedUpdateEvent?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            GameClient.Instance.Dispose();
        }
    }

    private void OnApplicationQuit()
    {
        if (Instance == this)
        {
            GameClient.Instance.GetService<IGameDataManager>().SaveDataClients();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (Instance != this) return;

        if (Application.platform == RuntimePlatform.Android)
            GameClient.Instance.GetService<IGameDataManager>().SaveDataClients();
    }
}