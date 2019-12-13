using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] List<GameObject> panels;

    public void MovePanel(GameObject panel)
    {
        GameObject parentPanel = panel.transform.parent.gameObject;

        panel.transform.position = new Vector2(panel.transform.position.x, panel.transform.position.y - panel.GetComponent<RectTransform>().sizeDelta.y);

        for (int i = panels.IndexOf(parentPanel) + 1; i < panels.Count; i++)
        {
            panels[i].transform.position = new Vector2(panels[i].transform.position.x, panels[i].transform.position.y - panel.GetComponent<RectTransform>().sizeDelta.y);
        }
    }
}
