using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
    
    [SerializeField]
    [Range(2, 100)]
    private int _pointCount = 20;
    [SerializeField]
    [Range(0.5f, 10f)]
    private float _radius = 4f;

    [SerializeField]
    [Range(0.5f, 10f)]
    private float _lineWidth = 3f;

    [SerializeField]
    private float _fillSpeed = 3f;

    private LineRenderer _outline;
    private LineRenderer _innerFill;
    private bool _fillProgress = false;
    private void Awake()
    {
        _outline = transform.Find("OutLine").GetComponent<LineRenderer>();
        _innerFill = transform.Find("InnerFill").GetComponent<LineRenderer>();
    }

    void Start()
    {
        DrawLine(_outline, 4, 0.2f);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump") && !_fillProgress)
        {

            //채워줄 녀석의 반지름의 절반을 반지름으로 잡고 너비를 반지름과 동일하게 잡는다.
            StartCoroutine(FillSequence(radius:2, width:4));
        }
    }

    IEnumerator FillSequence(float radius, float width)
    {
        _fillProgress = true;
        float currentFill = 0f;

        while ( radius - currentFill >= 0.01f )
        {
            yield return null;
            currentFill += Time.deltaTime * _fillSpeed;
            DrawLine(_innerFill, currentFill, currentFill * 2);
        }

        _fillProgress = false;
        
    }

    //라인렌더러, 반지름, 너비를 받아서 원을 그려준다.
    private void DrawLine(LineRenderer line, float radius, float width)
    {
        line.startWidth = width;
        line.endWidth = width;

        line.positionCount = _pointCount + 1;
        float oneDegree = 360f / _pointCount; 
        for(int i = 0; i < _pointCount; i++)
        {
            float degree = oneDegree * i * Mathf.Deg2Rad;
            Vector2 pos = new Vector2( Mathf.Cos(degree), Mathf.Sin(degree));
            line.SetPosition(i, pos * radius);
        }

        line.SetPosition(_pointCount, Vector2.right * radius);
    }
}
