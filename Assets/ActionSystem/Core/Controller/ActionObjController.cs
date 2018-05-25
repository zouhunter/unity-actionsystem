﻿using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using WorldActionSystem.Graph;

namespace WorldActionSystem
{

    public class ActionObjCtroller
    {
        public ActionCommand trigger { get;private set; }
        protected List<int> queueID = new List<int>();
        protected OperateNode[] actionObjs { get;private set; }
        public OperateNode[] StartedActions { get { return startedActions.ToArray(); } }
        protected bool isForceAuto;
        private Queue<OperateNode> actionQueue = new Queue<OperateNode>();
        private List<OperateNode> startedActions = new List<OperateNode>();
        public static bool log = false;
        public UnityAction<ControllerType> onCtrlStart { get; set; }
        public UnityAction<ControllerType> onCtrlStop { get; set; }
        private ActionGroup _system;
        private CameraController cameraCtrl
        {
            get
            {
                return ActionSystem.Instence.cameraCtrl;
            }
        }

        public ActionObjCtroller(ActionCommand trigger)
        {
            this.trigger = trigger;
            actionObjs = trigger.ActionObjs;
            ChargeQueueIDs();
        }

        public virtual void OnStartExecute(bool forceAuto)
        {
            this.isForceAuto = forceAuto;
            ExecuteAStep();
        }

        internal void OnPickUpObj(PickUpAbleItem obj)
        {
            var prio = startedActions.Find(x => x.Name == obj.Name);
            if (prio != null)
            {
                startedActions.Remove(prio);
                startedActions.Insert(0, prio);
            }
        }

        private void ChargeQueueIDs()
        {
            actionQueue.Clear();
            queueID.Clear();
            Array.Sort(actionObjs);
            foreach (var item in actionObjs)
            {
                if (!queueID.Contains(item.QueueID))
                {
                    queueID.Add(item.QueueID);
                }
            }
            queueID.Sort();
        }
        public virtual void OnEndExecute()
        {
            StopUpdateAction(false);
            CompleteQueues();
            Array.Sort(actionObjs);
            foreach (var item in actionObjs)
            {
                if (!item.Started)
                {
                    item.OnStartExecute(isForceAuto);
                }
                if (!item.Completed)
                {
                    item.OnEndExecute(true);
                }
            }

        }

        public virtual void OnUnDoExecute()
        {
            StopUpdateAction(true);
            UnDoQueues();
            ChargeQueueIDs();
            Array.Sort(actionObjs);
            Array.Reverse(actionObjs);
            foreach (var item in actionObjs)
            {
                if (item.Started)
                {
                    item.OnUnDoExecute();
                }
            }
        }


        private void OnCommandObjComplete(OperateNode obj)
        {
            OnStopAction(obj);
            var notComplete = Array.FindAll<OperateNode>(actionObjs, x => x.QueueID == obj.QueueID && !x.Completed);
            if (notComplete.Length == 0)
            {
                if (!ExecuteAStep())
                {
                    if (!trigger.Completed)
                        trigger.Complete();
                }
            }
            else if (actionQueue.Count > 0)//正在循环执行
            {
                QueueExectueActions();
            }
        }

        public void CompleteOneStarted()
        {
            if (startedActions.Count > 0)
            {
                var action = startedActions[0];
                OnStopAction(action);
                action.OnEndExecute(true);
            }
            else
            {
                if (log) Debug.Log("startedActions.Count == 0");
            }
        }
        private void CompleteQueues()
        {
            while (actionQueue.Count > 0)
            {
                var action = actionQueue.Dequeue();
                if (!action.Completed)
                {
                    action.OnEndExecute(true);
                }
            }
        }
        private void UnDoQueues()
        {
            while (actionQueue.Count > 0)
            {
                var action = actionQueue.Dequeue();
                if (action.Started)
                {
                    action.OnUnDoExecute();
                }
            }
        }
        protected bool ExecuteAStep()
        {
            if (queueID.Count > 0)
            {
                var id = queueID[0];
                queueID.RemoveAt(0);
                var neetActive = Array.FindAll<OperateNode>(actionObjs, x => x.QueueID == id && !x.Started);
                if (isForceAuto)
                {
                    actionQueue.Clear();
                    foreach (var item in neetActive)
                    {
                        if (item.QueueInAuto)
                        {
                            actionQueue.Enqueue(item as OperateNode);
                        }
                        else
                        {
                            TryStartAction(item);
                        }
                    }
                    QueueExectueActions();
                }
                else
                {
                    foreach (var item in neetActive)
                    {
                        var obj = item;
                        TryStartAction(obj);
                    }
                }
                return true;
            }
            return false;
        }

        protected void QueueExectueActions()
        {
            if (actionQueue.Count > 0)
            {
                var actionObj = actionQueue.Dequeue();
                if (log) Debug.Log("QueueExectueActions" + actionObj);
                TryStartAction(actionObj);
            }
        }
        private void TryStartAction(OperateNode obj)
        {
            if (log) Debug.Log("Start A Step:" + obj);
            if (!obj.Started)
            {
                if (cameraCtrl != null)
                {
                    cameraCtrl.SetViewCamera(() =>
                    {
                        StartAction(obj);
                    }, GetCameraID(obj));
                }
                else
                {
                    Debug.Log(cameraCtrl == null);
                    StartAction(obj);
                }
            }
            else
            {
                Debug.LogError(obj + " allready started");
            }

        }

        private void StartAction(OperateNode obj)
        {
            if (!obj.Started)
            {
                obj.onEndExecute = () => OnCommandObjComplete(obj);
                obj.OnStartExecute(isForceAuto);
                OnStartAction(obj);
            }

        }

        /// <summary>
        /// 添加新的触发器
        /// </summary>
        /// <param name="action"></param>
        private void OnStartAction(OperateNode action)
        {
            startedActions.Add(action);
            if (onCtrlStart != null) onCtrlStart.Invoke(action.CtrlType);
        }

        /// <summary>
        /// 移除触发器
        /// </summary>
        /// <param name="action"></param>
        private void OnStopAction(OperateNode action)
        {
            startedActions.Remove(action);
            if (onCtrlStop != null && startedActions.Find(x=>x.CtrlType == action.CtrlType) == null){
                onCtrlStop.Invoke(action.CtrlType);
            }
        }

        private string GetCameraID(OperateNode obj)
        {
            //忽略匹配相机
            if (Config.quickMoveElement /*&& obj is Actions.MatchObj && !(obj as Actions.MatchObj).ignorePass*/)
            {
                return null;
            }
            else if (Config.quickMoveElement /*&& obj is Actions.InstallObj && !(obj as Actions.InstallObj).ignorePass*/)
            {
                return null;
            }
            //除要求使用特殊相机或是动画步骤,都用主摄像机
            else if (Config.useOperateCamera/* || obj is Actions.AnimObj*/)
            {
                if (string.IsNullOrEmpty(obj.CameraID))
                {
                    return trigger.CameraID;
                }
                else
                {
                    return obj.CameraID;
                }
            }
            //默认是主相机
            else
            {
                return CameraController.defultID;
            }
        }

        private void StopUpdateAction(bool force)
        {
            if (cameraCtrl != null)
            {
                cameraCtrl.StopStarted(force);
            }
        }
    }
}