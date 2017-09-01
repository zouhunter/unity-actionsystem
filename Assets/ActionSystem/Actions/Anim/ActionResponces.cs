﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace WorldActionSystem
{
    public class ActionResponces : MonoBehaviour
    {
        public UnityAction<List<ActionResponce>> onAllElementInit;
        private List<ActionResponce> responceList = new List<ActionResponce>();

        private void Start()
        {
            responceList.AddRange(GetComponentsInChildren<ActionResponce>(true));
            if (onAllElementInit != null) onAllElementInit.Invoke(responceList);
        }
    }

}