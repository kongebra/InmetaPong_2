using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubmitForm : MonoBehaviour
{
    [Header("Input fields")]
    [SerializeField]
    private TMPro.TMP_InputField _nameInputField;
    [SerializeField]
    private TMPro.TMP_InputField _phoneInputField;
    [SerializeField]
    private TMPro.TMP_InputField _emailInputField;

    [Header("Error Texts")]
    [SerializeField]
    private TMPro.TextMeshProUGUI _nameErrorText;
    [SerializeField]
    private TMPro.TextMeshProUGUI _phoneErrorText;
    [SerializeField]
    private TMPro.TextMeshProUGUI _emailErrorText;

    [Header("Buttons")]
    [SerializeField]
    private UnityEngine.UI.Button _submitButton;

    private void Awake()
    {
        _submitButton.onClick.AddListener(HandleSubmitButtonClicked);

        ClearInputs();
        ClearErrors();
    }

    private void HandleSubmitButtonClicked()
    {
        var hasError = false;

        var name = _nameInputField.text;
        if (string.IsNullOrEmpty(name))
        {
            _nameErrorText.text = "Name is required";
            hasError = true;
        }

        var phone = _phoneInputField.text;
        if (string.IsNullOrEmpty(phone))
        {
            // Should this be required?
        }

        var email = _emailInputField.text;
        if (string.IsNullOrEmpty(email))
        {
            _emailErrorText.text = "Email is required";
            hasError = true;
        }

        if (hasError)
        {
            return;
        }

        var playerData = new PlayerScoreData
        {
            playerName = name,
            phoneNumber = phone,
            email = email,
            score = GameManager.Instance.Score,
        };

        HighscoreRowManager.Instance.AddScore(playerData);

        ClearInputs();
        ClearErrors();
    }

    private void ClearInputs()
    {
        _nameInputField.text = "";
        _phoneInputField.text = "";
        _emailInputField.text = "";
    }

    private void ClearErrors()
    {
        _nameErrorText.text = "";
        _phoneErrorText.text = "";
        _emailErrorText.text = "";
    }
}
