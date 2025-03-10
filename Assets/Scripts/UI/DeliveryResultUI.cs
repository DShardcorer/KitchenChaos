using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POP_UP = "PopUp";
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;

    [SerializeField] private Color successColor;
    [SerializeField] private Color failedColor;

    [SerializeField] private Sprite successIcon;
    [SerializeField] private Sprite failedIcon;

    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {

        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        Hide();
    }
    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {

        backgroundImage.color = successColor;
        iconImage.sprite = successIcon;
        messageText.text = "DELIVERY\nSUCCESS";
        animator.SetTrigger(POP_UP);
        Show();
    }
    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        backgroundImage.color = failedColor;
        iconImage.sprite = failedIcon;
        messageText.text = "DELIVERY\nFAILED";
        animator.SetTrigger(POP_UP);
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
