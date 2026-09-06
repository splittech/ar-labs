using Game.Core;
using UnityEngine;

namespace Game.Lab1
{
    public class PudgeSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _pudgePrefab;

        public PudgeView CreatePudgeObject(Vector3 position, Quaternion rotation)
        {
            GameObject pudgeObject = Instantiate(_pudgePrefab, position, rotation);
            PudgeView pudgeView = pudgeObject.GetComponent<PudgeView>();
            return pudgeView;
        }
    }
}
