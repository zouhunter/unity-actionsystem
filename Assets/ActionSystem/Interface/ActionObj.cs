﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using WorldActionSystem;
namespace WorldActionSystem
{
    public abstract class ActionObj:MonoBehaviour
    {
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
        public string StepName { get; set; }
        protected virtual void Start()
        {
            gameObject.SetActive(startActive);
        }
        public virtual void StartExecute()
        {
            onBeforeActive.Invoke(StepName);
            _started = true;
            _complete = false;
            gameObject.SetActive(true);
        }
        public virtual void EndExecute()
        {
            onBeforePlayEnd.Invoke(StepName);
            _started = true;
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