﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
namespace WorldActionSystem
{
    public partial class StapPanel
    {
        int backNumInput = 0;
        string jumpStapInput;
        int forwardNumInput = 0;

        public void OnbackNumInputEndEdit(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                backNumInput = -int.Parse(value);
            }
            else
            {
                backNumInput = default(int);
            }
        }
        public void OnjumpStapInputEndEdit(string value)
        {
            jumpStapInput = value;
        }
        public void OnforwardNumInputEndEdit(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                forwardNumInput = int.Parse(value);
            }
            else
            {
                forwardNumInput = default(int);
            }
        }

        public Button accept;
        public Button start;
        public Button backAstep;
        public Button backMutiStap;
        public Button toTargetStap;
        public Button skipAStap;
        public Button skipMutiStap;
        public Button toEnd;
        public Button insert;

        public Toggle autoNext;
        public Toggle autoPlay;

        /// <summary>
        /// 注册按扭事件
        /// </summary>
        void Awake()
        {
            accept.onClick.AddListener(OnAcceptButtonCilcked);
            start.onClick.AddListener(OnToStartButtonClicked);
            backAstep.onClick.AddListener(OnBackAStapButtonClicked);
            backMutiStap.onClick.AddListener(OnBackMutiButtonClicked);
            toTargetStap.onClick.AddListener(OnToGargetButtonClicked);
            skipAStap.onClick.AddListener(OnSkipAstepButtonClicekd);
            skipMutiStap.onClick.AddListener(OnSkipMutiButtonClicked);
            toEnd.onClick.AddListener(ToEndButtonClicked);
            insert.onClick.AddListener(OnInsertScript);

            //accept.onClick.AddListener(OnSelected);
            start.onClick.AddListener(OnStapChange);
            backAstep.onClick.AddListener(OnStapChange);
            backMutiStap.onClick.AddListener(OnStapChange);
            toTargetStap.onClick.AddListener(OnStapChange);
            skipAStap.onClick.AddListener(OnStapChange);
            skipMutiStap.onClick.AddListener(OnStapChange);
            toEnd.onClick.AddListener(OnStapChange);
        }

        void OnAcceptButtonCilcked()
        {
            if (remoteController.CurrCommand != null)
            {
                remoteController.StartExecuteCommand(OnEndExecute, autoPlay.isOn);
                textShow.text = remoteController.CurrCommand.StepName;
            }
            else
            {
                textShow.text = "结束";
            }
        }
        void OnToStartButtonClicked()
        {
            remoteController.ToAllCommandStart();
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }
        }
        void OnBackAStapButtonClicked()
        {
            remoteController.UnDoCommand();
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }
        }

        void OnBackMutiButtonClicked()
        {
            remoteController.ExecuteMutliCommand(backNumInput);
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }
        }
        void OnToGargetButtonClicked()
        {
            remoteController.ToTargetCommand(jumpStapInput);
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }
        }
        void OnSkipAstepButtonClicekd()
        {
            remoteController.ExecuteMutliCommand(1);
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }
        }
        void OnSkipMutiButtonClicked()
        {
            remoteController.ExecuteMutliCommand(forwardNumInput);
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }
        }
        void ToEndButtonClicked()
        {
            remoteController.ToAllCommandEnd();
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }
        }
        void OnStapChange()
        {
            if (autoNext.isOn)
            {
                textShow.text = remoteController.CurrCommand != null ? remoteController.CurrCommand.StepName : "结束";
            }
            else
            {
                textShow.text = "点击接收任务";
            }
        }
        void OnEndExecute()
        {
            if (autoNext.isOn)
            {
                OnAcceptButtonCilcked();
            }

        }
        void OnInsertScript()
        {
            ActionSystem.Instance.InsertScript<InstallItem, InfoTextShow>(true);
        }
        public Text textShow;
    }
    public partial class StapPanel : MonoBehaviour
    {
        public GameObject panel;
        IRemoteController remoteController;
        public Step[] steps;
        IEnumerator Start()
        {
            panel.SetActive(false);
            yield return ActionSystem.LunchActionSystem(steps);
            remoteController = ActionSystem.Instance.RemoteController;
            panel.SetActive(true);

            ActionSystem.Instance.onUserError += (x, y) => { Debug.Log(string.Format("{0}：{1}", x, y)); };
        }
    }

    [Serializable]
    public class Step : IActionStap
    {
        public string step;
        public string StapName
        {
            get
            {
                return step;
            }
        }

    }
}