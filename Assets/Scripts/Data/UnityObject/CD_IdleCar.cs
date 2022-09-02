using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_IdleCar", menuName = "Game/CD_IdleCar", order = 0)]
    public class CD_IdleCar : ScriptableObject
    {
        public IdleCarData Data;
    }
}