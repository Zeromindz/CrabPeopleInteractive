using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    private GameManager m_GameManager;

    [SerializeField] private TextMeshProUGUI m_TimeUI;
    [SerializeField] private TextMeshProUGUI m_CurrentSpeedText;
    [SerializeField] private TextMeshProUGUI m_MaxSpeedUI;

    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        string currentSpeedText = "Current: " + Mathf.Round(PlayerMovement.Instance.GetSpeeds().x);
        m_CurrentSpeedText.text = currentSpeedText;

        string maxSpeedText = "Current: " + Mathf.Round(PlayerMovement.Instance.GetSpeeds().x);
        m_MaxSpeedUI.text = maxSpeedText;
    }
}
