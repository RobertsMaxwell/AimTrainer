using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game
{
    public class Core : MonoBehaviour
    {
        [SerializeField] GameObject targetButton = null;
        [SerializeField] public Vector2 startSize = new Vector2(1, 1);
        [SerializeField] float targetSize = 50;
        [SerializeField] int amountPerInst = 2;
        [SerializeField] GameObject gameTimerWhole = null;
        [SerializeField] GameObject gameTimerFloat = null;
        [SerializeField] GameObject madeClickObject = null;
        [SerializeField] GameObject missedClickObject = null;

        [Header("Instantiation Frequency")]
        [SerializeField] float easyFrequency = .8f;
        [SerializeField] float mediumFrequency = .6f;
        [SerializeField] float hardFrequency = .4f;
        [SerializeField] float insaneFrequency = .2f;
        [SerializeField] float gameTimeLeftSeconds = 10f;
        [SerializeField] float instantiationTimeLeftSeconds = 9f;
        [SerializeField] float instantiationFreeTime = 1.5f;

        public List<Vector2> posList = new List<Vector2>();
        Settings settings = null;
        SceneChanger sceneChanger = null;

        public float score = 0;
        public float clicks = 0;
        public float targetsSpawned = 0;

        Vector2 canvasOffset;
        Canvas canvas;
        float timeLeftUntilInst = 0;
        float instFrequency = 1f;
        bool mouseHasBeenUp = true;
        bool gameActive = true;

        // Start is called before the first frame update
        void Start()
        {
            if(gameActive)
            {
                settings = FindObjectOfType<Settings>();
                canvas = FindObjectOfType<Canvas>();
                canvasOffset = canvas.GetComponent<RectTransform>().position;
                sceneChanger = FindObjectOfType<SceneChanger>();

                if (settings != null)
                {
                    targetSize = (int)settings.targetSize;
                    gameTimeLeftSeconds = (int)settings.duration;
                    instantiationTimeLeftSeconds = gameTimeLeftSeconds - instantiationFreeTime;

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
                            instFrequency = mediumFrequency;
                            break;
                    }
                }
            }
        }

        void Update()
        {
            if(gameActive)
            {
                if (gameTimeLeftSeconds > 0)
                {
                    if (instantiationTimeLeftSeconds > 0)
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
                        instantiationTimeLeftSeconds -= Time.deltaTime;
                    }
                    
                    gameTimeLeftSeconds -= Time.deltaTime;
                    UpdateGameTimer(gameTimeLeftSeconds);
                }
                else
                {
                    UpdateGameTimer(0);
                    StopGame();
                }

                bool foundTarget = false;
                if (Input.GetKeyDown(KeyCode.Mouse0) && mouseHasBeenUp)
                {
                    foreach (Target target in FindObjectsOfType<Target>())
                    {
                        if (target.ClickedOn(Input.mousePosition))
                        {
                            Instantiate(madeClickObject, Input.mousePosition, Quaternion.identity, canvas.transform);
                            foundTarget = true;
                            target.Remove();
                            UpdateStats(1, 1);
                            break;
                        }
                    }

                    if (!foundTarget)
                    {
                        Instantiate(missedClickObject, Input.mousePosition, Quaternion.identity, canvas.transform);
                        UpdateStats(0, 1);
                    }

                    mouseHasBeenUp = false;
                }
                else if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    mouseHasBeenUp = true;
                }
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

                    targetsSpawned++;
                    posList.Add(position);
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
            gameActive = false;
            sceneChanger.ChangeScene("Stats");
        }

        void UpdateGameTimer(float time)
        {
            if (gameTimerWhole && gameTimerFloat && Mathf.Sign(time) == 1)
            {
                TextMeshProUGUI wholeObject = gameTimerWhole.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI floatObject = gameTimerFloat.GetComponent<TextMeshProUGUI>();

                wholeObject.text = Mathf.Floor(time).ToString();

                if ((time - Mathf.Floor(time)).ToString().Length >= 4) floatObject.text = '.' + (time - Mathf.Floor(time)).ToString().Substring(2, 2);
                else if ((time - Mathf.Floor(time)).ToString().Length == 3) floatObject.text = '.' + (time - Mathf.Floor(time)).ToString().Substring(2, 1);
                else floatObject.text = ".00";
            }
        }

        public void UpdateStats(int deltaScore, int deltaClicks)
        {
            score += deltaScore;
            clicks += deltaClicks;
        }
    }
}