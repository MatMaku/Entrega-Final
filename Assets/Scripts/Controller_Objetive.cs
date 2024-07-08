using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Objetive : MonoBehaviour
{
    public GameObject objectivePanel;
    public TextMeshProUGUI objectiveText;

    private bool isObjectiveShown = false;

    private AudioSource audioSource;
    public AudioClip Effect;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isObjectiveShown && Input.anyKeyDown)
        {
            HideObjective();
        }
    }

    public void ShowObjective(string objective)
    {
        objectiveText.text = objective;
        objectivePanel.SetActive(true);
        Time.timeScale = 0f;
        isObjectiveShown = true;
        audioSource.PlayOneShot(Effect);
    }

    private void HideObjective()
    {
        objectivePanel.SetActive(false);
        Time.timeScale = 1f;
        isObjectiveShown = false;
    }
}
