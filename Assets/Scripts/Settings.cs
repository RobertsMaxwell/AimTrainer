using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Quick = 15,
            Normal = 30,
            Long = 90
        }

        [SerializeField] GameObject difficultyButton = null;
        [SerializeField] GameObject sizeButton = null;
        [SerializeField] GameObject durationButton = null;

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
            difficultyButton.GetComponentInChildren<Text>().text = difficulty.ToString();

            targetSize = TargetSize.Medium;
            sizeButton.GetComponentInChildren<Text>().text = targetSize.ToString();

            duration = Duration.Normal;
            durationButton.GetComponentInChildren<Text>().text = duration.ToString();
        }

        public void ChangeDifficulty()
        {
            if (difficultyButton != null)
            {
                List<Difficulty> difficultyList = new List<Difficulty>();
                difficultyList = Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>().ToList();

                difficulty = difficultyList[(difficultyList.IndexOf(difficulty) + 1) % difficultyList.Count];
                difficultyButton.GetComponentInChildren<Text>().text = difficulty.ToString();
            }
        }

        public void ChangeSize()
        {
            if (sizeButton != null)
            {
                List<TargetSize> sizeList = new List<TargetSize>();
                sizeList = Enum.GetValues(typeof(TargetSize)).Cast<TargetSize>().ToList();

                targetSize = sizeList[(sizeList.IndexOf(targetSize) + 1) % sizeList.Count];
                sizeButton.GetComponentInChildren<Text>().text = targetSize.ToString();
            }
        }

        public void ChangeDuration()
        {
            if (difficultyButton != null)
            {
                List<Duration> durationList = new List<Duration>();
                durationList = Enum.GetValues(typeof(Duration)).Cast<Duration>().ToList();

                duration = durationList[(durationList.IndexOf(duration) + 1) % durationList.Count];
                durationButton.GetComponentInChildren<Text>().text = duration.ToString();
            }
        }
    }
}
