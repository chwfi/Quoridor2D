using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [HideInInspector]
    public Transform canvasTrm;

    public Dictionary<string, PopupUI> popupUIDictionary = new();
    public Stack<PopupUI> currentPopupUI = new Stack<PopupUI>();

    public override void Awake()
    {
        canvasTrm = GameObject.Find("Canvas").transform;

        PopupUI[] popupUIs = canvasTrm.GetComponentsInChildren<PopupUI>();

        foreach (PopupUI popupUI in popupUIs)
        {
            if (!popupUIDictionary.ContainsKey(popupUI.name))
            {
                popupUIDictionary.Add(popupUI.name, popupUI);
            }
            else
            {
                Debug.LogWarning($"�ߺ� Ű : {popupUI.name}");
            }
        }
    }

    public void ShowPanel(string uiName, bool isOverlap = false)
    {
        popupUIDictionary.TryGetValue(uiName, out PopupUI popupUI);

        if (popupUI != null)
        {
            popupUI.ShowPanel();
        }
    }

    public void HidePanel(string uiName)
    {
        popupUIDictionary.TryGetValue(uiName, out PopupUI popupUI);

        popupUI.HidePanel();
    }

    public void HideAllPanel()
    {
        if (currentPopupUI.Count <= 0) return;

        var panelStackCopy = new Stack<PopupUI>(currentPopupUI);

        foreach (var panel in panelStackCopy)
        {
            currentPopupUI.TryPop(out _);
            panel.HidePanel();
        }
    }

    public void MovePanel(string uiName, float x, float y, float fadeTime)
    {
        popupUIDictionary.TryGetValue(uiName, out PopupUI popupUI);
        popupUI.MovePanel(x, y, fadeTime);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentPopupUI.Count > 0) //���� Stack�� �ϳ� �̻��� PopupUI�� ����ִٸ� ����
            {
                string name = currentPopupUI.Peek().name;
                bool isNotBattleResult = name != "DefeatUI" && name != "VictoryUI";

                if (isNotBattleResult) //�¸� �� UI�� �й� �� UI�� ���� �� ���� ����
                {
                    currentPopupUI.Peek().HidePanel(); //Stack�� ���� �ֱٿ� ���� PopupUI�� �ݾ���
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ShowPanel("Panel01");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowPanel("Panel02");
        }
    }
}