using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] GameObject infoPanel = null;
    List<GameObject> panels;
    List<string> panelTags;
    public bool panelOpen = false;
    bool settingsOpen = false;
    float timeToOpenSettings = .3f;
    Animator anim = null;

    private void Start()
    {
        anim = GetComponent<Animator>();  
        panels = new List<GameObject>();
        panelTags = new List<string>();

        if(anim != null)
        {
            anim.speed = 1 / timeToOpenSettings;
        }
        

        foreach (string str in UnityEditorInternal.InternalEditorUtility.tags)
        {
            if (str.Substring(0, 5) == "Panel")
            {
                panelTags.Add(str);
            }
        }

        for (int i = 0; i < panelTags.Count; i++)
        {
            var arr = FindObjectsOfType<SettingsPanel>();
            for (int y = 0; y < arr.Length; y++)
            {
                if (arr[y].tag == panelTags[i])
                {
                    panels.Add(arr[y].gameObject);
                    break;
                }
            }
        }
    }

    public void MovePanel()
    {
        if (!panelOpen)
        {
            foreach (SettingsPanel panel in FindObjectsOfType<SettingsPanel>())
            {
                if (panel.panelOpen)
                {
                    panel.ClosePanel();
                }
            }

            infoPanel.transform.position = new Vector2(infoPanel.GetComponent<RectTransform>().sizeDelta.x / 2, infoPanel.transform.position.y - infoPanel.GetComponent<RectTransform>().sizeDelta.y);

            for (int i = panels.IndexOf(gameObject) + 1; i < panels.Count; i++)
            {
                panels[i].transform.position = new Vector2(panels[i].GetComponent<RectTransform>().sizeDelta.x / 2, panels[i].transform.position.y - infoPanel.GetComponent<RectTransform>().sizeDelta.y);
            }

            panelOpen = true;
        }
        else
        {
            ClosePanel();
        }
    }

    void ClosePanel()
    {
        infoPanel.transform.position = new Vector2(infoPanel.transform.position.x, infoPanel.GetComponent<RectTransform>().sizeDelta.y / 2 + transform.position.y - GetComponent<RectTransform>().sizeDelta.y / 2);

        for (int i = panels.IndexOf(gameObject) + 1; i < panels.Count; i++)
        {
            panels[i].transform.position = new Vector2(panels[i].transform.position.x, panels[i].transform.position.y + infoPanel.GetComponent<RectTransform>().sizeDelta.y);
        }

        panelOpen = false;
    }

    public void OpenSettings()
    {
        if (!settingsOpen)
        {
            Transform parentTransform = transform.parent.transform;
            settingsOpen = true;
            
            anim.SetTrigger("RotatePos");
            StartCoroutine(MoveSettings(parentTransform, -parentTransform.GetComponent<RectTransform>().sizeDelta.x / 2, parentTransform.GetComponent<RectTransform>().sizeDelta.x / 2, timeToOpenSettings, true));      
        }
        else
        {
            CloseSettings();
        }
    }

    void CloseSettings()
    {
        Transform parentTransform = transform.parent.transform;
        settingsOpen = false;
        anim.SetTrigger("RotateNeg");    
        StartCoroutine(MoveSettings(parentTransform, parentTransform.GetComponent<RectTransform>().sizeDelta.x / 2, -parentTransform.GetComponent<RectTransform>().sizeDelta.x / 2, timeToOpenSettings, false));
    }

    IEnumerator MoveSettings(Transform parent, float a, float b, float timeToMove, bool opening)
    {
        float incrementation = 1 / (timeToMove * 60);
        float fracOfWhole = 0;

        while (fracOfWhole <= 1)
        {
            fracOfWhole += incrementation;
            parent.position = new Vector2(Mathf.Lerp(a, b, fracOfWhole), parent.position.y);
            yield return new WaitForSeconds(0);
        }

        if(!opening)
        {
            foreach (GameObject panel in panels)
            {
                if (panel.GetComponent<SettingsPanel>().panelOpen)
                {
                    panel.GetComponent<SettingsPanel>().ClosePanel();
                    break;
                }
            }
        }

        yield break;
    }
}
