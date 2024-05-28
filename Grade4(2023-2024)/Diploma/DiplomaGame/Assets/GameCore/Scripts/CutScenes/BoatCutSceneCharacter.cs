using System.Collections.Generic;
using GameCore.ShipScene.Extentions;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore.CutScenes
{
    public class BoatCutSceneCharacter : MonoBehaviour
    {
        [SerializeField] private GameObject _paddle;
        [SerializeField] private GameObject _model;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _startPoint;
        public Animator animator => _animator;
        public GameObject paddle => _paddle;
        public GameObject model => _model;
        public Transform startPoint => _startPoint;
        
        
    }
}