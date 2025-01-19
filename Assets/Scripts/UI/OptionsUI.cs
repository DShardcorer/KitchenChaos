using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private GameObject soundEffectSlider;
    [SerializeField] private GameObject musicSlider;

    [SerializeField] private Button closeButton;

    private Slider soundEffectSliderComponent;

    private TextMeshProUGUI soundEffectLabel;
    private Slider musicSliderComponent;
    private TextMeshProUGUI musicLabel;

    [SerializeField] private TextMeshProUGUI moveUpButtonText;
    [SerializeField] private TextMeshProUGUI moveDownButtonText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;

    [SerializeField] private TextMeshProUGUI interactButtonText;
    [SerializeField] private TextMeshProUGUI interactAlterateButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private GameObject pressToRebindKeyGameObject;






    private void Awake()
    {
        Instance = this;
        soundEffectSliderComponent = soundEffectSlider.GetComponentInChildren<Slider>();
        soundEffectLabel = soundEffectSlider.GetComponentInChildren<TextMeshProUGUI>();

        musicSliderComponent = musicSlider.GetComponentInChildren<Slider>();
        musicLabel = musicSlider.transform.GetComponentInChildren<TextMeshProUGUI>();

        soundEffectSliderComponent.onValueChanged.AddListener(OnSoundEffectVolumeChanged);
        musicSliderComponent.onValueChanged.AddListener(OnMusicVolumeChanged);

        closeButton.onClick.AddListener(OnCloseButtonClicked);



        moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Up));
        moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Down));
        moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Left));
        moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Right));
        interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
        interactAlternateButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact_Alternate));
        pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause));

    }



    private void Start()
    {
        GameManager.Instance.OnGameResumed += GameManager_OnGameResumed;
        UpdateVisuals();
        Hide();
        HidePressToRebindKey();
    }


    private void UpdateVisuals()
    {
        moveUpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlterateButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void GameManager_OnGameResumed(object sender, EventArgs e)
    {
        Hide();
    }

    private void OnMusicVolumeChanged(float value)
    {
        float maxMusicValue = 9;
        float normalizedValue = value / maxMusicValue;
        MusicManager.Instance.ChangeVolumeNormalized(normalizedValue);
        musicLabel.text = "Music:" + value;
    }

    private void OnSoundEffectVolumeChanged(float value)
    {
        float maxSoundValue = 9;
        float normalizedValue = value / maxSoundValue;
        SoundManager.Instance.ChangeVolumeNormalized(normalizedValue);
        soundEffectLabel.text = "SoundFX:" + value;
    }

    private void OnCloseButtonClicked()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowPressToRebindKey()
    {
        pressToRebindKeyGameObject.SetActive(true);
    }
    public void HidePressToRebindKey()
    {
        pressToRebindKeyGameObject.SetActive(false);
    }

    public void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding,
        () =>
        {
            UpdateVisuals();
            HidePressToRebindKey();
        });
    }
}
