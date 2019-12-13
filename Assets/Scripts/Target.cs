using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Target : MonoBehaviour
    {
        [SerializeField] float timeBetweenChanges = .1f;
        [SerializeField] float timeWhileLargeMin = .5f;
        [SerializeField] float timeWhileLargeMax = 1f;
        [SerializeField] float timeWhileSmallMin = .3f;
        [SerializeField] float timeWhileSmallMax = .8f;
        [SerializeField] float fractionUntilSlowGrow = .75f;
        [SerializeField] float shrinkingSlowSize = 35f;
        [SerializeField] float shrinkingPauseSize = 30f;
        [SerializeField] float slowdownSpeed = .02f;

        public Vector2 startSize;

        Core core = null;
        Settings settings = null;
        float fractionOfWhole = 0f;
        bool growing = true;
        public bool canRun = true;
        bool pauseOver = false;

        void Awake()
        {       
            core = FindObjectOfType<Core>();
            startSize = core.startSize;
            settings = FindObjectOfType<Settings>();
        }

        public void Remove()
        {
            canRun = false;
            Destroy(gameObject);
        }

        public IEnumerator Grow(float desiredSize)
        {
            fractionOfWhole = startSize.x / desiredSize;
            while (canRun)
            {
                if (growing)
                {
                    fractionOfWhole += Time.deltaTime;
                    gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(1, 1), new Vector2(desiredSize, desiredSize), fractionOfWhole);
                    gameObject.GetComponent<CircleCollider2D>().radius = GetComponent<RectTransform>().sizeDelta.x / 2;

                    if (fractionOfWhole >= 1)
                    {
                        growing = false;
                        yield return new WaitForSeconds(Random.Range(timeWhileLargeMin, timeWhileLargeMax));
                    }
                    else if (fractionOfWhole >= fractionUntilSlowGrow)
                    {
                        yield return new WaitForSeconds(timeBetweenChanges + slowdownSpeed);
                    } else
                    {
                        yield return new WaitForSeconds(timeBetweenChanges);
                    }
                }
                else
                {
                    fractionOfWhole -= Time.deltaTime;
                    gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(new Vector2(1, 1), new Vector2(desiredSize, desiredSize), fractionOfWhole);
                    gameObject.GetComponent<CircleCollider2D>().radius = GetComponent<RectTransform>().sizeDelta.x / 2;

                    if (fractionOfWhole <= 0)
                    {
                        Remove();
                        yield break;
                    }
                    else if (gameObject.GetComponent<RectTransform>().sizeDelta.x <= shrinkingPauseSize && !pauseOver && settings != null && settings.targetSize != Settings.TargetSize.Small)
                    {
                        pauseOver = true;
                        yield return new WaitForSeconds(1);                  
                    }
                    else if (gameObject.GetComponent<RectTransform>().sizeDelta.x <= shrinkingSlowSize)
                    {
                        yield return new WaitForSeconds(timeBetweenChanges + slowdownSpeed);
                    }
                    else
                    {
                        yield return new WaitForSeconds(timeBetweenChanges);
                    }
                }
            }
        }

        public bool ClickedOn(Vector2 position)
        {
            if (GetComponent<CircleCollider2D>().OverlapPoint(position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

