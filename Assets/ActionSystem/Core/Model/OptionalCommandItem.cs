﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace WorldActionSystem
{
    [System.Serializable]
    public class OptionalCommandItem
    {
        public bool active;
        public ActionCommand prefab;
    }
}