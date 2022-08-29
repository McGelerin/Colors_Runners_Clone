using Controllers;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCitizenManager : MonoBehaviour
{
    #region Self Variables
    #region Public Variables
    public float ScaleValue;

    #endregion
    #region SerializeField Variables
    [SerializeField] private IdleCitizenMeshController meshController;
    [SerializeField] private IdleCitizenAnimController animationController;
    [SerializeField] private IdleCitizenPhysicsController physicsController;
    [SerializeField] private List<Transform> rebornPoints;
    #endregion
    #region Private Variables
    private Sequence _mySequence;
    private IdleNavigationEnum _lastEnum;
    private IdleNavigationEnum _newEnum;
    private Vector3 _currentTarget;
    private IdleCitizenData _data;


    #endregion
    #endregion

    private void Awake()
    {
        _data = GetData();
        SendDataToController();

        _mySequence = DOTween.Sequence();
        //ScaleValue = 20f;
    }
    private void Start()
    { 
        meshController.SetRandomColor();
    }
    private IdleCitizenData GetData() => Resources.Load<CD_IdleCitizen>("Data/CD_IdleCitizen").Data;

    public void SendDataToController()
    {
        physicsController.GetData(_data);
    }
    public void SelectRandomDirection(IdleTargetData targetData)
    {
        _newEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        while ((int)_lastEnum % 2 == (int)_newEnum % 2)
        {
            _newEnum = targetData.axises[Random.Range(0, targetData.axises.Count)];

        }
        _lastEnum = _newEnum;

        SetCurrentTarget();
    }


    public void SetCurrentTarget()
    {

        if (_newEnum == IdleNavigationEnum.Up)
        {
            _currentTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z + ScaleValue);

        }
        else if (_newEnum == IdleNavigationEnum.Down)
        {
            _currentTarget = new Vector3(transform.position.x, transform.position.y, transform.position.z - ScaleValue);

        }
        else if (_newEnum == IdleNavigationEnum.Right)
        {
            _currentTarget = new Vector3(transform.position.x + ScaleValue + 3, transform.position.y, transform.position.z);

        }
        else if (_newEnum == IdleNavigationEnum.Left)
        {
            _currentTarget = new Vector3(transform.position.x - ScaleValue - 3, transform.position.y, transform.position.z);

        }

        Move();
    }

    public void Move()
    {
        SetAnimation(IdleCitizenAnimStates.Walk);
        _mySequence = DOTween.Sequence();
        _mySequence.Append(transform.DOMove(_currentTarget, _data.ReachingTime).SetEase(Ease.Linear));
        transform.DOLookAt(_currentTarget, _data.RotationTime);
    }

    public void CollideWithPlayer()
    {
        _mySequence.Kill();
        int rand = Random.Range(0, rebornPoints.Count);
        transform.position = new Vector3(rebornPoints[rand].position.x, transform.position.y, rebornPoints[rand].position.z);
        //transform.position = rebornPoints[rand].position;
    }

    public void SetAnimation(IdleCitizenAnimStates animState)
    {
        animationController.ResetAnim();
        animationController.SetAnimState(animState);
    }
}
