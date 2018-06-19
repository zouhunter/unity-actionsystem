﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace InteractSystem
{
    [System.Serializable]
    public class Config
    {
        private static Config _defult;
        public static Config Instence
        {
            get
            {
                if (_defult == null)
                {
                    SetConfig();
                }
                return _defult;
            }
            set
            {
                _defult = value;
            }
        }

        public int autoExecuteTime = 3;
        public int hitDistence = 100;
        public int elementFoward = 1;
        public bool highLightNotice = true;//高亮提示
        public bool useOperateCamera = true;//使用专用相机
        public bool angleNotice = true;//箭头提示
        public bool previewNotice = true;//实例提示
        public bool quickMoveElement = false;//元素快速移动
        public bool ignoreController = false;//忽略控制器
        public Material lineMaterial = null;
        public float lineWidth = 0.2f;
        public Color highLightColor = Color.green;
        public GameObject angleObj = null;
        public Material previewMat = null;//预览材质
        public  float previewAlpha = 0.1f;
        public List<Binding.OperaterBinding> operateBindings = new List<Binding.OperaterBinding>();
        public List<Binding.ActionItemBinding> actionItemBindings = new List<Binding.ActionItemBinding>();
        public List<Binding.CommandBinding> commandBindings = new List<Binding.CommandBinding>();
        public MatchType linkMatchType = MatchType.WindowPosition;
        public List<OperateNodeFeature> operateNodeFeatures = new List<OperateNodeFeature>();
        public List<ActionItemFeature> actionItemFeatures = new List<ActionItemFeature>();

        public static void SetConfig(Config config = null)
        {
            if (config == null)
            {
                _defult = new Config();
            }
            else
            {
                _defult = config;
            }
        }
    }
}

