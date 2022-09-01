using System;
using Controllers;
using Enums;
using Data.ValueObject;
using UnityEngine;

namespace Managers
{
    public class IdleCollectableManager : MonoBehaviour
    {
        #region Self Variables
        #region Public Variables
        
        public ColorEnum ColorState
        {
            get => colorState;
            set
            {
                colorState = value;
                collectableMeshController.ColorSet();
            }
        }
    
        #endregion
        #region SerializeField Variables
        [SerializeField] private IdleCollectableMeshController collectableMeshController;
        [SerializeField] private CollectableAnimationController animationController;
        [SerializeField] private ColorEnum colorState;
        [SerializeField] private CollectableAnimStates initialAnimState = CollectableAnimStates.Clap;
        #endregion
        #region Private Variables
        
        private ColorData _colorData;
        private ColorEnum _poolColorEnum;
        #endregion
        #endregion

        // private void OnEnable()
        // {
        //    // initialAnimState = CollectableAnimStates.Clap;
        //     SetReferances();
        // }

        private void OnEnable()
        {
            SetCollectableAnimation(initialAnimState);
        }

        private void Start()
        {
            SetReferances();
        }
        
        private void SetReferances()
        {
            ColorState = colorState;
        }
    
        public void SetCollectableAnimation(CollectableAnimStates newAnimState)
        {
            animationController.SetAnimState(newAnimState);
        }
    }
}