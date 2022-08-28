using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Data.ValueObject;
using DG.Tweening;
using Data.UnityObject;

public class IdleCarManager : MonoBehaviour
{
    #region Self Variables
    #region Public Variables
    public float scaleValue;
    public bool IsActive = true;

    #endregion
    #region SerializeField Variables

    #endregion
    #region Private Variables
    private IdleNavigationEnum _lastEnum;
    private IdleNavigationEnum _newEnum;
    private Vector3 _currentTarget;
    private Sequence _mySequence;
    private IdleTargetData _lastData;

    private IdleCarData _data;

    #endregion
    #endregion

    private void Awake()
    {
        //scaleValue = transform.localScale.x / 0.3f;
        _data = GetData();

        _mySequence = DOTween.Sequence();
        scaleValue = 19.5f;
    }
    private IdleCarData GetData() => Resources.Load<CD_IdleCar>("Data/CD_IdleCar").Data;
    private void Start()
    {
        SetCurrentTarget();
    }

    public void SelectRandomDirection(IdleTargetData targetData)
    {
        _lastData = targetData;
        _newEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        while ((int)_lastEnum % 2 == (int)_newEnum % 2)
        {
            _newEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        }
        _lastEnum = _newEnum;
        StartCoroutine(WaitToReach());
    }

    IEnumerator WaitToReach()
    {
        yield return new WaitForSeconds(0.4f);
        SetCurrentTarget();
    }

    public void SetCurrentTarget()
    {

        if (_newEnum == IdleNavigationEnum.Up)
        {
            _currentTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z + scaleValue);

        }
        else if (_newEnum == IdleNavigationEnum.Down)
        {
            _currentTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z - scaleValue);

        }
        else if (_newEnum == IdleNavigationEnum.Right)
        {
            _currentTarget = new Vector3(transform.position.x + scaleValue, transform.position.y, transform.position.z);

        }
        else if (_newEnum == IdleNavigationEnum.Left)
        {
            _currentTarget = new Vector3(transform.position.x - scaleValue, transform.position.y, transform.position.z);

        }

        Move();
    }

    public void Move()
    {
        _mySequence = DOTween.Sequence();
        _mySequence.Append(transform.DOMove(_currentTarget, _data.ReachingTime).SetEase(_data.ReachingEase));
        transform.DOLookAt(_currentTarget, _data.RotationTime).SetEase(_data.RotationEase);
    }

    public void MoveAfterPlayer(bool _isOnTargetTrigger)
    {
        Move();

        if (_isOnTargetTrigger)
        {
            SelectRandomDirection(_lastData);
        }
    }


    public void PlayerOnRoad()
    {
        Debug.Log(transform.localPosition);
        Vector3 currentPos = transform.localPosition;
        StopAllCoroutines();
        _mySequence.Kill();
        transform.localPosition = currentPos;
        //DOTween.Kill(_mySequence);
    }
}
