using Controllers;
using Signals;
using Enums;
using Data.UnityObject;
using Data.ValueObject;
using Sirenix.Serialization;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    #region Self Variables

    #region Public Variables



    #endregion
    #region SerializeField Variables

    #endregion
    #region Private Variables
    

    #endregion

    #endregion

    private void Awake()
    {
    }
    
    
    public void InteractionWithCollectable(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject);
    }

    public void InteractionWithObstacle(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionObstacle?.Invoke(collectableGameObject);
    }
    
}