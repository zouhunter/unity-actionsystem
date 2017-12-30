﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace WorldActionSystem
{
    public class ActionSystem : MonoBehaviour
    {
        private static bool isQuit = false;
        private static ActionSystem _instence;
        public static ActionSystem Instence
        {
            get
            {
                if (_instence == null && !isQuit)
                {
                    _instence = new GameObject("ActionSystem").AddComponent<ActionSystem>();
                }
                return _instence;
            }
        }
        public ActionCtroller actionCtrl { get; private set; }
        public CameraController cameraCtrl { get; private set; }
        public AngleCtroller angleCtrl { get; private set; }

        private List<ActionGroup> groupList = new List<ActionGroup>();
        private Dictionary<string, List<UnityAction<ActionGroup>>> waitDic = new Dictionary<string, List<UnityAction<ActionGroup>>>();
        private void Awake()
        {
            if (_instence == null){
                _instence = this;
            }
            InitControllers();
        }
        private void InitControllers()
        {
            if (cameraCtrl == null)
            {
                cameraCtrl = new CameraController(this);
            }
            if (actionCtrl == null)
            {
                actionCtrl = new ActionCtroller(this);
            }
            if (angleCtrl == null)
            {
                angleCtrl = new AngleCtroller(this);
            }
        }
        private void OnApplicationQuit()
        {
            isQuit = true;
        }

        public void RetriveAsync(string groupKey, UnityAction<ActionGroup> onRetrive)
        {
            if (onRetrive == null) return;
            var item = groupList.Find(x => x.groupKey == groupKey);
            if (item)
            {
                onRetrive.Invoke(item);
            }
            else
            {
                if (!waitDic.ContainsKey(groupKey))
                {
                    waitDic[groupKey] = new List<UnityAction<ActionGroup>>();
                }
                waitDic[groupKey].Add(onRetrive);
            }
        }

        internal void RegistGroup(ActionGroup actionGroup)
        {
            if (!groupList.Contains(actionGroup))
            {
                groupList.Add(actionGroup);
            }
            if (waitDic.ContainsKey(actionGroup.groupKey))
            {
                var actions = waitDic[actionGroup.groupKey];
                waitDic.Remove(actionGroup.groupKey);
                foreach (var item in actions)
                {
                    item.Invoke(actionGroup);
                }
            }
        }

        internal void RemoveGroup(ActionGroup actionGroup)
        {
            if (groupList.Contains(actionGroup))
            {
                groupList.Remove(actionGroup);
            }
        }
    }

}