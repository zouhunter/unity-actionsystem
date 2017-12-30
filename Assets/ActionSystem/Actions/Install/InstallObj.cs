﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Internal;
#if !NoFunction
using DG.Tweening;
#endif
namespace WorldActionSystem
{
    /// <summary>
    /// 模拟安装坐标功能
    /// </summary>
    public class InstallObj : PlaceObj
    {
        public override ControllerType CtrlType
        {
            get
            {
                return ControllerType.Install;
            }
        }

        public override int layer
        {
            get
            {
               return Layers.installPosLayer;
            }
        }
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnBeforeComplete(bool force)
        {
            base.OnBeforeComplete(force);

            if (!AlreadyPlaced)
            {
                PickUpAbleElement obj = elementCtrl.GetUnInstalledObj(Name);
                Attach(obj);
                obj.QuickInstall(this);
                obj.StepComplete();
            }
        }
        protected override void OnBeforeUnDo()
        {
            Debug.Log("OnBeforeUnDo:" + Name);

            base.OnBeforeUnDo();

            if (AlreadyPlaced)
            {
                var obj = Detach();
                obj.QuickUnInstall();
                obj.StepUnDo();
            }
        }

        protected override void OnInstallComplete()
        {
            if (!Complete)
            {
                OnEndExecute(false);
            }
        }

        protected override void OnUnInstallComplete()
        {
            if(Started)
            {
                if (AlreadyPlaced)
                {
                    var obj = Detach();
                    obj.QuickUnInstall();
                    obj.StepUnDo();
                }
                this.obj = null;
            }
        }

        protected override void OnAutoInstall()
        {
            PickUpAbleElement obj = elementCtrl.GetUnInstalledObj(Name);
            Attach(obj);
            obj.StepActive();
            if (Config.quickMoveElement && !ignorePass)
            {
                obj.QuickInstall(this);
            }
            else
            {
                obj.NormalInstall(this);
            }
        }
    }

}