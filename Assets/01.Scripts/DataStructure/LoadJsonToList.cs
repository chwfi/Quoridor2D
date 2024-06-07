using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LoadJsonToList : MonoBehaviour
{
    private List<string> _nameList = new List<string>();
    [SerializeField]
    private InputField _txtSearchInput;
    [SerializeField]
    private Text _txtContent;

    void Start()
    {
        TextAsset nameJson = Resources.Load("name") as TextAsset;
        JArray arr = JArray.Parse(nameJson.ToString());
        
        foreach(JToken token in arr)
        {
            _nameList.Add(token["name"].ToString());
        }
        
    }

    public void SearchName()
    {
        //이부분은 직접 구현하도록 하자.
        string searchWord = _txtSearchInput.text;

        List<string> searchList = new List<string>();

        for(int i = 0; i < _nameList.Count; i++)
        {
            if(_nameList[i].Contains(searchWord))
            {
                searchList.Add(_nameList[i]);
            }
        }

        StringBuilder sBuilder = new StringBuilder();
        for(int i = 0; i < searchList.Count; i++)
        {
            sBuilder.Append(searchList[i]);
            if(i < searchList.Count - 1)
            {
                sBuilder.Append("\n");
            }
        }      
        
        _txtContent.text = sBuilder.ToString();

    }
}
