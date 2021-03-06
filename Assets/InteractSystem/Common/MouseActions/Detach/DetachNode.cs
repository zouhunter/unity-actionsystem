﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractSystem.Actions
{
    [NodeGraph.CustomNode("Operate/Detach", 14, "InteractSystem")]
    public class DetachNode :Graph.OperaterNode
    {
        [SerializeField]
        protected QueueCollectNodeFeature completeableFeature = new QueueCollectNodeFeature(typeof(DetachItem));

        protected override List<OperateNodeFeature> RegistFeatures()
        {
            var features = base.RegistFeatures();
            completeableFeature.SetTarget(this);
            features.Add(completeableFeature);
            return features;
        }
        public override void OnStartExecute(bool auto = false)
        {
            base.OnStartExecute(auto);
            DetachCtrl.Instence.RegistLock(this);
            if(auto){
                completeableFeature.AutoCompleteItems();
            }
        }
        public override void OnUnDoExecute()
        {
            base.OnUnDoExecute();
            DetachCtrl.Instence.RemoveLock(this);
        }
        public override void OnEndExecute(bool force)
        {
            base.OnEndExecute(force);
            DetachCtrl.Instence.RemoveLock(this);
        }
    }
}
