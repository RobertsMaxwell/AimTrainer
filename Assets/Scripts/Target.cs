using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Level1 
{
    public class Target : MonoBehaviour
    {
        [SerializeField] float timeBetweenChanges = .1f;
        [SerializeField] float timeWhileLargeMin = .5f;
        [SerializeField] float timeWhileLargeMax = 1f;

        public Vector2 startSize;

        float fractionOfWhole = 0f;
        bool growing = true;
        public bool canRun = true;

        void Awake()
        {
            startSize = FindObjectOfType<Core>().startSize;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && GetComponent<CircleCollider2D>().OverlapPoint(Input.mousePosition))
            {
                Remove();
            }
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
                    yield return new WaitForSeconds(timeBetweenChanges);

                    if (fractionOfWhole >= 1)
                    {
                        growing = false;
                        yield return new WaitForSeconds(Random.Range(timeWhileLargeMin, timeWhileLargeMax));
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
                    else
                    {
                        yield return new WaitForSeconds(timeBetweenChanges);
                    }
                }
            }
        }
    }
}

