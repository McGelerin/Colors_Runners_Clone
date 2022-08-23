using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Drone", menuName = "Game/CD_Drone", order = 0)]
    public class CD_Drone : ScriptableObject
    {
        public DronePoolData Data;
    }
}