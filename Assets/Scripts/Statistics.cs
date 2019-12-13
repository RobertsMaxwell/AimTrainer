using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;

public class Statistics : MonoBehaviour
{
    [Header("Accuracy")]
    [SerializeField] Text accuracyLeftText;
    [SerializeField] Text accuracyRightText;

    [Header("Targets")]
    [SerializeField] Text targetsLeftText;
    [SerializeField] Text targetsRightText;

    [Header("Game")]
    [SerializeField] Text difficultyText;
    [SerializeField] Text durationText;
    [SerializeField] Text sizeText;

    [Header("ClickMap")]
    [SerializeField] GameObject miniCanvas;
    [SerializeField] GameObject miniTarget;

    Settings settings;
    Core core;

    Settings.Difficulty difficulty;
    Settings.Duration duration;
    Settings.TargetSize targetSize;

    void Start()
    {
        settings = FindObjectOfType<Settings>();
        core = FindObjectOfType<Core>();
        difficulty = settings.difficulty;
        duration = settings.duration;
        targetSize = settings.targetSize;

        TextSetup();
    }

    void TextSetup()
    {
        accuracyLeftText.text = $"{Mathf.Round(core.score / core.clicks * 100)}%";
        accuracyRightText.text = $"{core.clicks} Clicks\n{core.score} Landed\n{core.clicks - core.score} Missed";

        targetsLeftText.text = $"{core.targetsSpawned}";
        targetsRightText.text = $"{core.score} Destroyed\n{core.targetsSpawned - core.score} Expired";

        difficultyText.text = difficulty.ToString();
        durationText.text = $"{duration} Seconds";
        sizeText.text = targetSize.ToString();

        foreach (Vector2 pos in core.posList)
        {
            var instTarget = Instantiate(miniTarget, miniCanvas.transform);
            instTarget.transform.position = pos / 4 + (Vector2)miniCanvas.transform.parent.transform.position - new Vector2(miniCanvas.GetComponent<RectTransform>().sizeDelta.x / 2, miniCanvas.GetComponent<RectTransform>().sizeDelta.y / 2); 
        }
    }
}
