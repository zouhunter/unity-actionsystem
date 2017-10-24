﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace WorldActionSystem
{

    public class CameraNode : MonoBehaviour
    {
        [SerializeField,Range(1,10)]
        private float _lerpTime = 1;//缓慢移入
        //[SerializeField]
        private float _field = 60;
        [SerializeField]
        private Transform _target;
        private float _distence = 1;
        private Quaternion _rotate;
        public string ID { get { return name; } }
        public float LerpTime { get { return _lerpTime; } }
        public float Distence { get { return _distence; } }
        public float CameraField { get { return _field; } }
        public Quaternion Rotation { get { return _rotate; } }
        public bool MoveAble { get { return _target != null; } }
        private void Awake()
        {
            if (_target == null && transform.childCount > 0)
            {
                _target = transform.GetChild(0);
            }

            if(_target != null)
            {
                _distence = Vector3.Distance(transform.position, _target.position);
                _rotate = Quaternion.LookRotation(_target.position - transform.position);
            }
            else
            {
                _rotate = transform.rotation;
            }

            CameraController.RegistNode(this);
        }
    }

}