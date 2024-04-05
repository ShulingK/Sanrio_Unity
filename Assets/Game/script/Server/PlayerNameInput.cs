using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.ComponentModel;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] 
    private TMP_InputField m_nameInputField = null;
    [SerializeField]
    private Button m_Connection = null;
    [SerializeField]
    private TMP_Text text = null;

    public static string DisplayName { get; private set; }
    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey (PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString (PlayerPrefsNameKey);

        m_nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        m_Connection.interactable = !string.IsNullOrEmpty(name);
    }

    public void SetPlayerName()
    {
        Debug.Log(text.text);
        m_Connection.interactable = !string.IsNullOrEmpty(text.text);
    }

    public void SavePlayerName()
    {
        DisplayName = m_nameInputField.text;
        PlayerPrefs.SetString (PlayerPrefsNameKey, DisplayName);
    }

}
