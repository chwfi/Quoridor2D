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
                Debug.LogWarning($"중복 키 : {popupUI.name}");
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
            if (currentPopupUI.Count > 0) //만약 Stack에 하나 이상의 PopupUI가 들어있다면 실행
            {
                string name = currentPopupUI.Peek().name;
                bool isNotBattleResult = name != "DefeatUI" && name != "VictoryUI";

                if (isNotBattleResult) //승리 시 UI와 패배 시 UI는 닫을 수 없게 설정
                {
                    currentPopupUI.Peek().HidePanel(); //Stack의 가장 최근에 들어온 PopupUI를 닫아줌
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