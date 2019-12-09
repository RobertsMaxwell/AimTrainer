using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game;

namespace Level1
{
    public class Core : MonoBehaviour
    {
        [SerializeField] GameObject targetButton = null;
        [SerializeField] public Vector2 startSize = new Vector2(1, 1);
        [SerializeField] float targetSize = 50;
        [SerializeField] int amountPerInst = 2;
        [SerializeField] GameObject gameTimer;

        [Header("Instantiation Frequency")]
        [SerializeField] float easyFrequency = 1.5f;
        [SerializeField] float mediumFrequency = 1.0f;
        [SerializeField] float hardFrequency = .75f;
        [SerializeField] float insaneFrequency = .25f;
        [SerializeField] float timeLeftSeconds = 10f;

        Settings settings = null;

        Vector2 canvasOffset;
        Canvas canvas;
        float timeLeftUntilInst = 0;
        float instFrequency = 1f;


        // Start is called before the first frame update
        void Start()
        {
            settings = FindObjectOfType<Settings>();
            canvas = FindObjectOfType<Canvas>();
            canvasOffset = canvas.GetComponent<RectTransform>().position;

            if (settings != null)
            {
                targetSize = (int)settings.targetSize;

                switch (settings.difficulty)
                {
                    case Settings.Difficulty.Easy:
                        instFrequency = easyFrequency;
                        break;
                    case Settings.Difficulty.Medium:
                        instFrequency = mediumFrequency;
                        break;
                    case Settings.Difficulty.Hard:
                        instFrequency = hardFrequency;
                        break;
                    case Settings.Difficulty.Insane:
                        instFrequency = insaneFrequency;
                        break;
                    default:
                        instFrequency = 1f;
                        break;
                }
            }
        }

        void Update()
        {
            if (timeLeftSeconds > 0)
            {
                if (timeLeftUntilInst - Time.deltaTime <= 0)
                {
                    timeLeftUntilInst = instFrequency;
                    InstantiateCircle(startSize, targetSize, amountPerInst);
                }
                else
                {
                    timeLeftUntilInst -= Time.deltaTime;
                }

                timeLeftSeconds -= Time.deltaTime;
                UpdateGameTimer(timeLeftSeconds);
            } else
            {
                StopGame();
            }
        }

        public void InstantiateCircle(Vector2 startSize, float desiredSize, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Vector2 position = new Vector2(
                    Random.Range(
                        -(canvas.GetComponent<RectTransform>().rect.width / 2 - desiredSize / 2),
                        canvas.GetComponent<RectTransform>().rect.width / 2 - desiredSize / 2)
                            + canvasOffset.x,
                    Random.Range(
                        -(canvas.GetComponent<RectTransform>().rect.height / 2 - desiredSize / 2),
                        canvas.GetComponent<RectTransform>().rect.height / 2 - desiredSize / 2)
                            + canvasOffset.y
                    );

                if (Physics2D.OverlapCircle(position, desiredSize / 2) == null)
                {
                    var instTarget = Instantiate(targetButton);

                    instTarget.GetComponent<RectTransform>().sizeDelta = startSize;
                    instTarget.GetComponent<CircleCollider2D>().radius = instTarget.GetComponent<RectTransform>().sizeDelta.x / 2;
                    instTarget.GetComponentsInChildren<CircleCollider2D>()[1].radius = desiredSize / 2;
                    instTarget.transform.SetParent(canvas.transform);
                    instTarget.GetComponent<RectTransform>().position = position;
                    StartCoroutine(instTarget.GetComponent<Target>().Grow(desiredSize));
                }
                else
                {
                    InstantiateCircle(startSize, desiredSize, 1);
                }
            }
        }

        void StopGame()
        {
            foreach (var target in FindObjectsOfType<Target>())
            {
                target.canRun = false;
                Destroy(target.gameObject);
            }
        }

        void UpdateGameTimer(float time)
        {
            gameTimer.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(time).ToString();
        }
    }
}