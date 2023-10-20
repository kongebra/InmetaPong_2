using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private void Start()
    {
        // focus on name input field
        EventSystem.current.SetSelectedGameObject(_nameInputField.gameObject, null);
        _nameInputField.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_nameInputField.isFocused)
            {
                _emailInputField.Select();
            }
            else if (_emailInputField.isFocused)
            {
                _phoneInputField.Select();
            }
            else if (_phoneInputField.isFocused)
            {
                _nameInputField.Select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            HandleSubmitButtonClicked();
        }
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

        var email = _emailInputField.text.ToLower();
        if (string.IsNullOrEmpty(email))
        {
            _emailErrorText.text = "Email is required";
            hasError = true;
        }

        if (!IsValidEmail(email))
        {
            _emailErrorText.text = "Email is invalid";
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

    private bool IsValidEmail(string email)
    {
        // Regular expression pattern for a valid email
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Use the Regex.IsMatch method to check if the email matches the pattern
        return Regex.IsMatch(email, pattern);
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
