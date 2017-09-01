﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace WorldActionSystem
{
    public class ResponceObj : MonoBehaviour, IActionCommand
    {
        [SerializeField]
        private string _stepName;
        public bool startActive;
        public bool endActive;
        [SerializeField]
        protected StepEvent onBeforeActive;
        [SerializeField]
        protected StepEvent onBeforeUnDo;
        [SerializeField]
        protected StepEvent onBeforePlayEnd;
        protected bool _complete;
        public bool Complete { get { return _complete; } }
        protected bool _started;
        public bool Started { get { return _started; } }
        public string StepName { get { return _stepName; } }
        protected virtual void Start()
        {
            gameObject.SetActive(startActive);
        }
        public virtual void StartExecute(bool forceAuto = false)
        {
            onBeforeActive.Invoke(StepName);
            _started = true;
            gameObject.SetActive(true);
        }
        public virtual void EndExecute()
        {
            onBeforePlayEnd.Invoke(StepName);
            _complete = true;
            gameObject.SetActive(endActive);
        }
        public virtual void UnDoExecute()
        {
            onBeforeUnDo.Invoke(StepName);
            _started = false;
            _complete = false;
            gameObject.SetActive(startActive);
        }
    }
}