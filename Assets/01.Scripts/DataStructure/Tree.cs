using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    public GameObject nodePrefab;

    private TreeNode<int> _root = null;

    [SerializeField]
    private InputField _txtInput;

    [SerializeField]
    private int _treeDepth = 0;

    public void AddNode(int number = 0)
    {
        try
        {
            if(number == 0)
                number = int.Parse(_txtInput.text);


            TreeNode<int> newNode = new TreeNode<int>();
            newNode.Data = number;
            
            Vector3 pos = transform.position;
            if (_root == null)
            {
                _root = newNode;
                newNode.Parent = null;
                _treeDepth++;
            }
            else
            {
                pos = Insert(newNode);
                //여기까지 오면 newNode에 셋팅 끝나있음
                if(pos == Vector3.zero)
                {
                    throw new Exception("중복값입니다.");
                }
            }
            GameObject nodeObj = Instantiate(nodePrefab, transform);
            newNode.Prefab = nodeObj;
            newNode.SetPosition(pos);

            AdjustPosition(); 
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
   

    private void Start()
    {
        AddNode(50);
        AddNode(75);
        AddNode(25);
        AddNode(10);
        AddNode(35);
        AddNode(65);
        AddNode(90);
        AddNode(100);
        AddNode(110);
        AddNode(60);
        AddNode(73);
        AddNode(30);
        AddNode(38);
        AddNode(5);
        AddNode(20);
        AddNode(2);
        AddNode(8);
        AddNode(15);
        AddNode(23);
        AddNode(27);
        AddNode(34);
        AddNode(36);
        AddNode(40);
        AddNode(55);
        AddNode(64);
        AddNode(70);
        AddNode(74);
        AddNode(95);
        AddNode(85);
        AddNode(88);
        AddNode(78);
        AdjustPosition();
    }

    //클릭시 삭제로 변경하는 과제 
    public void DeleteNode(int number = 0)
    {
        try
        {
            if (number == 0)
                number = int.Parse(_txtInput.text);
            TreeNode<int> node = Find(number);
            if(node == null)
            {
                throw new Exception("존재하지 않는 수입니다.");
            }
            //자식이 없는 노드
            if(node.Left == null && node.Right == null)
            {
                //노드 부모에게서 자신의 정보삭제
                bool isLeft = IsLeftChild(node);
                if (isLeft) node.Parent.Left = null;
                else node.Parent.Right = null;
                node.Delete();
            }else if(node.Left != null && node.Right == null)
            {
                //왼쪽자식만 있는 경우 자신의 왼쪽에서 가장 오른쪽 손자를 찾아서 가져와야 함.
                TreeNode<int> mostRightChild = node.Left;
                while(mostRightChild.Right != null)
                {
                    mostRightChild = mostRightChild.Right;
                }
                node.Data = mostRightChild.Data;  //데이터 복사 후

                bool isLeft = IsLeftChild(mostRightChild); //부모의 왼쪽자식인지 체크
                if(isLeft)
                {
                    mostRightChild.Parent.Left = mostRightChild.Left; //왼쪽을 연결
                }
                else
                {
                    mostRightChild.Parent.Right = mostRightChild.Left; //왼쪽을 연결
                }
                if(mostRightChild.Left != null)
                    mostRightChild.Left.Parent = mostRightChild.Parent;
                mostRightChild.Delete();
            }else if(node.Right != null)
            {
                //오른쪽 자식만 있는 경우 가장 왼쪽 손자를 찾아서 가져와야 함.
                TreeNode<int> mostLeftChild = node.Right;
                while (mostLeftChild.Left != null)
                {
                    mostLeftChild = mostLeftChild.Left;
                }
                node.Data = mostLeftChild.Data;
                bool isLeft = IsLeftChild(mostLeftChild); //부모의 왼쪽인지 체크
                if (isLeft)
                {
                    mostLeftChild.Parent.Left = mostLeftChild.Right;
                }else
                {
                    mostLeftChild.Parent.Right = mostLeftChild.Right;
                }
                if (mostLeftChild.Right != null)
                    mostLeftChild.Right.Parent = mostLeftChild.Parent;
                mostLeftChild.Delete();
            }
            //else if(node.Left != null && node.Right != null)
            //{
            //    //둘다 있는 경우는 둘중 하나만 선택해서 연결하면 됨. 따라서 위의 코드를 그대로 가져오면 된다.
            //    TreeNode<int> mostLeftChild = node.Right;
            //    while (mostLeftChild.Left != null)
            //    {
            //        mostLeftChild = mostLeftChild.Left;
            //    }
            //    Debug.Log(mostLeftChild.Data);
            //    node.Data = mostLeftChild.Data;
            //    bool isLeft = IsLeftChild(mostLeftChild); //부모의 왼쪽인지 체크
            //    if (isLeft)
            //    {
            //        mostLeftChild.Parent.Left = mostLeftChild.Right;
            //    }
            //    else
            //    {
            //        mostLeftChild.Parent.Right = mostLeftChild.Right;
            //    }
            //    if(mostLeftChild.Right != null)
            //        mostLeftChild.Right.Parent = mostLeftChild.Parent;
            //    mostLeftChild.Delete();
            //}

            AdjustPosition(); //위치 조정
        }catch (Exception e)
        {
            Debug.LogError(e.Message);
            Debug.Log(e.StackTrace);
        }
    }

    private TreeNode<int> Find(int number)
    {
        TreeNode<int> node = _root;
        if (node == null) return null;
        while(node.Data != number)
        {
            if (node.Data < number)
            {
                if (node.Right == null) return null;
                node = node.Right;
            }
            else if (node.Data > number)
            {
                if (node.Left == null) return null;
                node = node.Left;
            }
        }
        return node;
    }

    
    //왼쪽 자식일 경우 true반환
    private bool IsLeftChild(TreeNode<int> node)
    {
        return node.Parent != null && node.Parent.Left == node;
    }


    private Vector3 Insert(TreeNode<int> node)
    {
        TreeNode<int> pointer = _root;
        int depth = 1;
        while(true)
        {
            if(node.Data < pointer.Data) //중복 허용 안할시에는 등호제거하고 에러낼것
            {//왼쪽으로 
                if (pointer.Left == null )
                {
                    node.Parent = pointer;
                    pointer.Left = node;
                    if (depth + 1 > _treeDepth)
                    {
                        _treeDepth = depth + 1;
                    }
                    return pointer.GetPosition() + new Vector3(-2f, -2f, 0);
                }else
                {
                    pointer = pointer.Left;
                }
            }else if(node.Data > pointer.Data)
            {
                //오른쪽으로
                if (pointer.Right == null)
                {
                    node.Parent = pointer;
                    pointer.Right = node;
                    if (depth + 1 > _treeDepth)
                    {
                        _treeDepth = depth + 1;
                    }
                    return pointer.GetPosition() + new Vector3(2f, -2f, 0);
                }
                else
                {
                    pointer = pointer.Right;
                }
            }else
            {
                //중복노드시 입력 안함.
                return Vector3.zero;
            }
            depth++;
        }
    }

    //트리의 뎁스에 따라서 포지션을 수정하도록 정정 => 이것도 과제로
    private void AdjustPosition()
    {        
        //여기서는 트리의 너비를 우선으로 탐색을 해나가야 하기 때문에 너비우선 탐색을 사용한다.
        Queue <TreeNode<int>> bfs = new Queue<TreeNode<int>>();

        bfs.Enqueue(_root); //루트 넣고 시작
        int depth = 1;
        while(bfs.Count > 0)
        {
            Queue<TreeNode<int>> nextNodes = new Queue<TreeNode<int>>();
            while(bfs.Count > 0)
            {
                TreeNode<int> node = bfs.Dequeue();

                SetNodePosition(node, depth);
                if (node.Left != null)
                    nextNodes.Enqueue(node.Left);
                if (node.Right != null)
                    nextNodes.Enqueue(node.Right);
            }
            depth++;
            bfs = nextNodes;
        }
    }

    private void SetNodePosition(TreeNode<int> node, int depth)
    {
        
        if (depth == 1)
        {
            node.SetPosition(new Vector3(0, 0, 0)); //원점으로 변경
        }
        else
        {
            Vector3 parentPos = node.Parent.GetPosition();
            bool isLeft = node.Parent.Left != null && node.Parent.Left == node;


            float distance = Mathf.Pow(2, _treeDepth - depth + 1) / 2;

            if (isLeft) //왼쪽일때
            {
                node.SetPosition(parentPos + new Vector3(-distance, -2f, 0));
            }else //오른쪽일 때
            {
                node.SetPosition(parentPos + new Vector3(distance, -2f, 0));
            }
        }
        SetNodeLine(node);
    }

    private void SetNodeLine(TreeNode<int> node)
    {
        node.Line.SetPosition(0, node.GetPosition());
        if (node.Parent != null)
            node.Line.SetPosition(1, node.Parent.GetPosition());
        else
            node.Line.SetPosition(1, node.GetPosition());
    }
}

