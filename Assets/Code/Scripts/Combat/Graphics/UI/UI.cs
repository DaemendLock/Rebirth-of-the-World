using Projectiles;
using System;
using UnityEngine;

public class UI : MonoBehaviour {

    public static UI Instance { get; private set; } = null;

    public PartyFrame partyFrame;
    public GameObject projectileContainer;
    public TipPanel tipPanel;
    [SerializeField] private PauseMenu _pauseMenu;

    [NonSerialized]
    public bool paused = true;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        ProjectileManager.projectileContainer = projectileContainer;
        Loader.Instance.LoadBufferedScenario();
    }

    private void Update() {
        if (paused)
            return;
        ProjectileManager.ProccesParticles(Time.deltaTime);
    }

    public void TogglePauseMenu() {
        _pauseMenu.gameObject.SetActive(!_pauseMenu.gameObject.activeSelf);
    }




}