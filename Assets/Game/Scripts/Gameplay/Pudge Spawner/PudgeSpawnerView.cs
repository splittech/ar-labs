using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _pudgePrefab;
        [SerializeField] private float _initialScale = 1f;

        public PudgeView CreatePudgeObject()
        {
            GameObject pudgeObject = Instantiate(_pudgePrefab);
            PudgeView pudgeView = pudgeObject.GetComponent<PudgeView>();
            return pudgeView;
        }
    }
}
