using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickIndicator : MonoBehaviour
{
    [SerializeField] float lifeTime = 3f;
    [SerializeField] float fractionUntilFade = .8f;
    float timeAlive = 0;
    float fadeAmount = 0;
    float fadePercentIncrement;
    float startAlpha;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        fadePercentIncrement = lifeTime * fractionUntilFade / (60 * lifeTime * fractionUntilFade);
        image = GetComponent<Image>();
        startAlpha = image.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeAlive >= lifeTime)
        {
            Destroy(gameObject);
        } else if (timeAlive >= lifeTime * (1 - fractionUntilFade))
        {
            fadeAmount += fadePercentIncrement;
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(startAlpha, 0.0f, fadeAmount));
        }

        timeAlive += Time.deltaTime;
    }
}
