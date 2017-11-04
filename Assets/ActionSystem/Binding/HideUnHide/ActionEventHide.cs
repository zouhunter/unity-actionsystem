﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace WorldActionSystem
{
    /// <summary>
    /// 在一个触发器时间内触发关闭和打开
    /// 或 打开或关闭事件
    /// </summary>
    public class ActionEventHide : ActionObjBinding
    {
        public string key;
        public bool activeOnComplete;
        public bool activeOnStart;
        
        private string resetKey { get { return "HideResetObjects"; } }
        private string hideKey { get { return "HideObjects"; } }
        private string showKey { get { return "UnHideObjects"; } }


        protected override void OnBeforeActive(bool forceAuto)
        {
            if(activeOnStart)
            {
                EventController.NotifyObserver(showKey, key);
            }
            else
            {
                EventController.NotifyObserver(hideKey, key);
            }
        }
        protected override void OnBeforeComplete(bool force)
        {
            if (activeOnComplete)
            {
                EventController.NotifyObserver(showKey, key);
            }
            else
            {
                EventController.NotifyObserver(hideKey, key);
            }
        }
        protected override void OnBeforeUnDo()
        {
            EventController.NotifyObserver(resetKey, key);
        }

    }
}