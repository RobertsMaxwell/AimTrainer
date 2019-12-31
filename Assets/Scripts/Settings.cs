using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class Settings : MonoBehaviour
    {
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard,
            Insane
        }

        public enum TargetSize
        {
            Small = 50,
            Medium = 100,
            Large = 150
        }

        public enum Duration
        { 
            Quick = 8,
            Normal = 15,
            Long = 30
        }

        [SerializeField] GameObject difficultyButtonPos = null;
        [SerializeField] GameObject difficultyButtonNeg = null;
        [SerializeField] GameObject difficultyText = null;
        [SerializeField] GameObject sizeButtonPos = null;
        [SerializeField] GameObject sizeButtonNeg = null;
        [SerializeField] GameObject sizeText = null;
        [SerializeField] GameObject durationButtonPos = null;
        [SerializeField] GameObject durationButtonNeg = null;
        [SerializeField] GameObject durationText = null;

        public Difficulty difficulty;
        public TargetSize targetSize;
        public Duration duration;

        void Start()
        {
            ButtonSetup();
        }

        private void ButtonSetup()
        {
            difficulty = Difficulty.Medium;
            difficultyText.GetComponent<TextMeshProUGUI>().text = difficulty.ToString();

            targetSize = TargetSize.Medium;
            sizeText.GetComponent<TextMeshProUGUI>().text = targetSize.ToString();

            duration = Duration.Normal;
            durationText.GetComponent<TextMeshProUGUI>().text = duration.ToString();
        }

        public void ChangeDifficulty(int change)
        {
            if (difficultyButtonPos && difficultyButtonNeg)
            {
                List<Difficulty> difficultyList;
                difficultyList = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().ToList();

                difficulty = difficultyList[(int)Mathf.Repeat(difficultyList.IndexOf(difficulty) + change, difficultyList.Count)];
                difficultyText.GetComponent<TextMeshProUGUI>().text = difficulty.ToString();
            }
        }

        public void ChangeSize(int change)
        {
            if (sizeButtonPos && sizeButtonNeg)
            {
                List<TargetSize> sizeList;
                sizeList = Enum.GetValues(typeof(TargetSize)).Cast<TargetSize>().ToList();

                targetSize = sizeList[(int)Mathf.Repeat(sizeList.IndexOf(targetSize) + change, sizeList.Count)];
                sizeText.GetComponent<TextMeshProUGUI>().text = targetSize.ToString();
            }
        }

        public void ChangeDuration(int change)
        {
            if (durationButtonPos && durationButtonNeg)
            {
                List<Duration> durationList;
                durationList = Enum.GetValues(typeof(Duration)).Cast<Duration>().ToList();

                duration = durationList[(int)Mathf.Repeat(durationList.IndexOf(duration) + change, durationList.Count)];
                durationText.GetComponent<TextMeshProUGUI>().text = duration.ToString();
            }
        }
    }
}
