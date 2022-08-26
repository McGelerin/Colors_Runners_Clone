using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Data.ValueObject;
using DG.Tweening;
public class IdleCarManager : MonoBehaviour
{
    public IdleNavigationEnum LastEnum;
    public IdleNavigationEnum NewEnum;
    public Vector3 _currentTarget;
    public float scaleValue;
    public bool IsActive = true;
    Sequence mySequence;
    Coroutine myCoroutine;

    private IdleTargetData _lastData;

    private void Awake()
    {
        //scaleValue = transform.localScale.x / 0.3f;
        mySequence = DOTween.Sequence();
        scaleValue = 19.5f;
    }
    private void Start()
    {

        SetCurrentTarget();
    }

    public void SelectRandomDirection(IdleTargetData targetData)
    {
        _lastData = targetData;
        NewEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        while ((int)LastEnum % 2 == (int)NewEnum % 2)
        {
            NewEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        }
        LastEnum = NewEnum;
        myCoroutine = StartCoroutine(WaitToReach());
    }

    IEnumerator WaitToReach()
    {
        yield return new WaitForSeconds(0.5f);
        SetCurrentTarget();

    }

    public void SetCurrentTarget()
    {

        if (NewEnum == IdleNavigationEnum.Up)
        {
            _currentTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z + scaleValue);

        }
        else if (NewEnum == IdleNavigationEnum.Down)
        {
            _currentTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z - scaleValue);

        }
        else if (NewEnum == IdleNavigationEnum.Right)
        {
            _currentTarget = new Vector3(transform.position.x + scaleValue, transform.position.y, transform.position.z);

        }
        else if (NewEnum == IdleNavigationEnum.Left)
        {
            _currentTarget = new Vector3(transform.position.x - scaleValue, transform.position.y, transform.position.z);

        }

        Move();
    }

    public void Move()
    {
        mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(_currentTarget, 2f).SetEase(Ease.Linear));
        transform.DOLookAt(_currentTarget, 1f);
    }

    public void MoveAfterPlayer()
    {
        mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMove(_currentTarget, 2f).SetEase(Ease.Linear));
        transform.DOLookAt(_currentTarget, 1f);

        if (myCoroutine.Equals(null))
        {
            SelectRandomDirection(_lastData);
        }
    }


    public void PlayerOnRoad()
    {
        StopAllCoroutines();
        mySequence.Kill();
    }

    

    
}
