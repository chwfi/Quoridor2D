using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeNode<T>
{ 
    private TextMeshPro _text;
    private GameObject _prefab; //자기를 표현할 프리팹

    private TreeNode<T> _parent = null, _left = null, _right = null;
    private T _data;
    //private int _depth = 1;
    
    public TreeNode<T> Parent
    {
        get => _parent;
        set => _parent = value;
    }

    public TreeNode<T> Left
    {
        get => _left;
        set => _left = value;
    }

    public TreeNode<T> Right
    {
        get => _right;
        set => _right = value;
    }
    public T Data
    {
        get => _data;
        set {
            _data = value;
            if(_text != null)
                _text.text = value.ToString();
        }
    }

    //public int Depth
    //{
    //    get => _depth;
    //    set => _depth = value;
    //}

    public GameObject Prefab
    {
        get => _prefab;
        set
        {
            _prefab = value;
            _text = _prefab.GetComponentInChildren<TextMeshPro>();
            _line = _prefab.GetComponent<LineRenderer>();
            _text.text = _data.ToString();
        }
    }

    private LineRenderer _line;
    public LineRenderer Line
    {
        get => _line;
    }

    public Vector3 GetPosition()
    {
        return _prefab.transform.position;// 프리팹 위치 
    }

    public void SetPosition(Vector3 position)
    {
        _prefab.transform.position = position;
    }

    public void Delete()
    {
        GameObject.Destroy(_prefab.gameObject);
    }
}
