using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    [SerializeField] GameObject targetButton;
    [SerializeField] float startSize = 1;
    [SerializeField] float targetSize = 50;
    [SerializeField] float instFrequency = 2;
    [SerializeField] int amountPerInst = 2;

    Vector2 canvasOffset;

    Canvas canvas;
    float timeLeftUntilInst = 0;


    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        canvasOffset = canvas.GetComponent<RectTransform>().position;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) ;

        if (timeLeftUntilInst - Time.deltaTime <= 0)
        {
            timeLeftUntilInst = instFrequency;
            InstantiateCircle(startSize, targetSize, amountPerInst);
        }
        else
        {
            timeLeftUntilInst -= Time.deltaTime;
        }

    }

    public void InstantiateCircle(float startSize, float desiredSize, int amount)
    {
        for(int i = 0; i < amount; i++)
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

                instTarget.GetComponent<RectTransform>().sizeDelta = new Vector2(startSize, startSize);
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
}