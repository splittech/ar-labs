using System;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeSpawnerView : MonoBehaviour, IPudgeSpawnerView
    {
        [Header("Parameters")]
        [SerializeField] private Transform _pudgeRootTransform;
        [SerializeField] private float _initialScale = 1f;

        [Header("Prefabs")]
        [SerializeField] private GameObject _normalPudgePrefab;
        [SerializeField] private GameObject _happyPudgePrefab;
        [SerializeField] private GameObject _sadPudgePrefab;

        public float InitialScale => _initialScale;

        public IPudgeView CreatePudgeObject(Pudge.State pudgeState)
        {
            GameObject pudgePrefab = pudgeState switch
            {
                Pudge.State.Normal => _normalPudgePrefab,
                Pudge.State.Happy => _happyPudgePrefab,
                Pudge.State.Sad => _sadPudgePrefab,
                _ => throw new IndexOutOfRangeException()
            };

            GameObject pudgeObject = Instantiate(pudgePrefab, _pudgeRootTransform);
            IPudgeView pudgeView = pudgeObject.GetComponent<IPudgeView>();
            return pudgeView;
        }
    }
}
