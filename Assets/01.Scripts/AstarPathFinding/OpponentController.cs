using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OpponentController : MonoBehaviour
{
    public Transform DestinationPosition;

    private Vector3Int _currentPos;
    private Vector3Int _destination;

    private List<Vector3Int> _routePath = new List<Vector3Int>();

    private Camera _mainCam;

    private bool _isMove;
    private int _idx;
    private Vector3 _nextPos;
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private bool _cornerCheck = true;

    private void Start()
    {
        _currentPos = TilemapInfo.Instance.GetStartCellPos();
        transform.position = TilemapInfo.Instance.GetWorldPos(_currentPos);
        _mainCam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mPos = DestinationPosition.position;
            mPos.z = 0;
            Vector3Int cellPos = TilemapInfo.Instance.GetCellPos(mPos);

            Debug.Log(cellPos);

            if (TilemapInfo.Instance.CanMove(cellPos))
            {
                _destination = cellPos;
                if (CalcRoute())
                {
                    PrintRoute();
                    _idx = 0;
                    SetNextTarget();
                    _isMove = true;
                }
            }
        }

        if (_isMove)
        {
            Vector3 dir = _nextPos - transform.position;
            if (dir.magnitude <= 0.05f)
            {
                SetNextTarget();
            }

            transform.position += dir.normalized * Time.deltaTime * _speed;
        }
    }

    private void SetNextTarget()
    {
        if (_idx >= _routePath.Count)
        {
            _isMove = false;
            return;
        }
        _currentPos = _routePath[_idx];
        _nextPos = TilemapInfo.Instance.GetWorldPos(_currentPos);
        _idx++;
    }

    public void PrintRoute()
    {
        for (int i = 0; i < _routePath.Count; i++)
        {
            Debug.Log(_routePath[i]);
        }
    }

    #region A��Ÿ
    //F = G + H
    // G�� ������� �̵��� �� ���, ���⼭���� ������������ ���� ���(��ֹ� ���ٰ� �����ϰ�)
    private PriorityQueue<Node> _openList;
    private List<Node> _closeList;


    private bool CalcRoute()
    {
        _openList = new PriorityQueue<Node>();
        _closeList = new List<Node>();

        //�� ó�� ���������� openList�� �ִ´�.
        _openList.Push(new Node { pos = _currentPos, _parent = null, G = 0, F = CaclH(_currentPos) });

        bool result = false;
        int cnt = 0;
        while (_openList.Count > 0)
        {
            Node n = _openList.Pop();
            FindOpenList(n);
            _closeList.Add(n);
            Debug.Log(n.pos);
            if (n.pos == _destination)
            {
                result = true;
                break;
            }

            cnt++;
            if (cnt >= 1000)
            {
                Debug.Log("While�� 1000ȸ!!");
                break;
            }

        }


        if (result)
        {
            _routePath.Clear();
            Node last = _closeList[_closeList.Count - 1];
            //_routePath.Add(_destination);
            while (last._parent != null)
            {
                _routePath.Add(last.pos);
                last = last._parent;
            }
            _routePath.Reverse();
        }

        return result;
    }

    private void FindOpenList(Node currentNode)
    {
        // Only check orthogonal directions (up, down, left, right)
        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(0, 1, 0),  // Up
            new Vector3Int(0, -1, 0), // Down
            new Vector3Int(1, 0, 0),  // Right
            new Vector3Int(-1, 0, 0)  // Left
        };

        foreach (var dir in directions)
        {
            Vector3Int next = currentNode.pos + dir;

            //�ش� ������ �̹� �湮�ߴ�.
            Node n = _closeList.Find(x => x.pos == next);
            if (n != null) continue;

            if (TilemapInfo.Instance.CanMove(next))
            {
                //������忡�� ������� ���� ��� + ���������� �Դ� ����� ���ؼ� G�� ������ְ�
                float g = (currentNode.pos - next).magnitude + currentNode.G;

                Node nextOpenNode = new Node { pos = next, _parent = currentNode, G = g, F = g + CaclH(next) };
                Node exists = _openList.Contains(nextOpenNode);

                //Debug.Log(nextOpenNode.pos + ", " + nextOpenNode.F);

                if (exists != null)
                {
                    //�̹� openList�� �����Ѵٸ� ���� �� ª������ ����ؼ� �־��ش�. �ƴϸ� �ƹ��ϵ� �����൵ �ȴ�.
                    if (nextOpenNode.G < exists.G)
                    {
                        exists.G = nextOpenNode.G;
                        exists.F = nextOpenNode.F;
                        exists._parent = nextOpenNode._parent;
                    }
                }
                else
                {
                    //�������� ������ �־��ְ�
                    _openList.Push(nextOpenNode);
                }
            }
        }
    }

    //��ġ�κ��� F���� ���ϴ� �Լ�
    private float CaclH(Vector3Int pos)
    {
        Vector3Int distance = _destination - pos;
        return distance.magnitude;
    }

    #endregion
}
