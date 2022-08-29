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
    private Sequence _moveSequence;
    private Sequence _rotateSequence;
    private IdleTargetData _lastData;
    private Transform _lastTrigger;

    private IdleCarData _data;


    #endregion
    #endregion

    private void Awake()
    {
        _data = GetData();
        Init();
        
    }
    private void Init()
    {
        _lastTrigger = transform;

        _moveSequence = DOTween.Sequence();
        scaleValue = 19.5f;
    }
    private IdleCarData GetData() => Resources.Load<CD_IdleCar>("Data/CD_IdleCar").Data;
    private void Start()
    {
        SetCurrentTarget(_lastTrigger);
    }

    public void SelectRandomDirection(IdleTargetData targetData, Transform trigger)
    {
        _lastData = targetData;
        _newEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        while ((int)_lastEnum % 2 == (int)_newEnum % 2)
        {
            _newEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        }
        _lastEnum = _newEnum;
        StartCoroutine(WaitToReach(trigger));
    }

    IEnumerator WaitToReach(Transform trigger)
    {
        yield return new WaitForSeconds(0.4f);
        SetCurrentTarget(trigger);
    }

    public void SetCurrentTarget(Transform lastTrigger)
    {

        if (_newEnum == IdleNavigationEnum.Up)
        {
            _currentTarget = new Vector3(lastTrigger.position.x, transform.position.y, lastTrigger.position.z + scaleValue);

        }
        else if (_newEnum == IdleNavigationEnum.Down)
        {
            _currentTarget = new Vector3(lastTrigger.position.x, transform.position.y, lastTrigger.position.z - scaleValue);

        }
        else if (_newEnum == IdleNavigationEnum.Right)
        {
            _currentTarget = new Vector3(lastTrigger.position.x + scaleValue, transform.position.y, lastTrigger.position.z);

        }
        else if (_newEnum == IdleNavigationEnum.Left)
        {
            _currentTarget = new Vector3(lastTrigger.position.x - scaleValue, transform.position.y, lastTrigger.position.z);

        }
        _lastTrigger = lastTrigger;

        Move();
    }



    public void Move()
    {
        _moveSequence = DOTween.Sequence();
        _rotateSequence = DOTween.Sequence();
        _moveSequence.Append(transform.DOMove(_currentTarget, _data.ReachingTime).SetEase(_data.ReachingEase));
        _rotateSequence.Append(transform.DOLookAt(_currentTarget, _data.RotationTime).SetEase(_data.RotationEase));
    }

    public void MoveAfterPlayer(bool _isOnTargetTrigger)
    {
        Move();

        if (_isOnTargetTrigger)
        {
            SelectRandomDirection(_lastData, _lastTrigger);
        }
    }


    public void PlayerOnRoad()
    {
        Vector3 currentPos = transform.localPosition;
        StopAllCoroutines();
        _moveSequence.Kill();
        _rotateSequence.Kill();
        transform.localPosition = currentPos;
    }

}
