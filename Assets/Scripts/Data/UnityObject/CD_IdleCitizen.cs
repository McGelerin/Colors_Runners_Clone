using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_IdleCitizen", menuName = "Game/CD_IdleCitizen", order = 0)]
    public class CD_IdleCitizen : ScriptableObject
    {
        public IdleCitizenData Data;
    }
}