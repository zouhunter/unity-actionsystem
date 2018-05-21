﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace WorldActionSystem
{

    public abstract class AnimPlayer: PickUpAbleElement
    {
        [SerializeField]
        protected bool _reverse;
        [SerializeField]
        protected int playableCount = 1;
        protected float _duration = 1;
        protected List<Actions.AnimObj> targets = new List<Actions.AnimObj>();
        public virtual float duration { get { return _duration; }set { _duration = value; } }
        public virtual bool reverse { get { return _reverse; } set { _reverse = value; } }
        public UnityAction onAutoPlayEnd { get; set; }
        [HideInInspector]
        public UnityEvent onPlayComplete;
      

        public override void StepComplete()
        {
            onPlayComplete.Invoke();
        }
        public virtual void RecordPlayer(Actions.AnimObj target)
        {
            if(!targets.Contains(target))
            {
                this.targets.Add(target);
            }
        }
        public virtual void RemovePlayer(Actions.AnimObj target)
        {
            if (targets.Contains(target))
            {
                this.targets.Remove(target);
            }
        }
        public virtual bool CanPlay()
        {
            if(targets.Count < playableCount)
            {
                return true;
            }
            return false;
        }
        
    }

}
