﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using HighlightingSystem;
namespace InteractSystem.Binding
{
    public class EventHightLightRegister : MonoBehaviour
    {
        [SerializeField]
        private string key;
        [SerializeField]
        private Color color = Color.green;
        [SerializeField]
        private List<GameObject> m_Objs;

        private string highLight { get { return "HighLightObjects"; } }
        private string unhighLight { get { return "UnHighLightObjects"; } }

        private bool activeHighLight { get { return Config.Instence.highLightNotice; } }
        private List<Highlighter> highlighters = new List<Highlighter>();
        private EventController eventCtrl { get { return GetComponentInParent<ActionGroup>().EventCtrl; } }
        private void Start()
        {
            eventCtrl.AddDelegate<string>(highLight, HighLightGameObjects);
            eventCtrl.AddDelegate<string>(unhighLight, UnHighLightGameObjects);
            RegistItems();
        }

        private void OnDestroy()
        {
            eventCtrl.RemoveDelegate<string>(highLight, HighLightGameObjects);
            eventCtrl.RemoveDelegate<string>(unhighLight, UnHighLightGameObjects);
        }
        private void RegistItems()
        {
            if (m_Objs.Count == 0) m_Objs.Add(gameObject);
            foreach (var item in m_Objs)
            {
                var high = item.GetComponent<Highlighter>();
                if (high == null)
                {
                    high = item.AddComponent<Highlighter>();
                }

                if (!highlighters.Contains(high))
                {
                    highlighters.Add(high);
                    high.On();
                }
            }
        }

        public void HighLightGameObjects(string key)
        {
            if (!activeHighLight) return;

            if (this.key == key)
            {
                for (int i = 0; i < highlighters.Count; i++)
                {
                    var item = highlighters[i];
                    if (item)
                    {
                        item.FlashingOn(Color.white, color);
                    }
                }
            }

        }
        public void UnHighLightGameObjects(string key)
        {
            if (!activeHighLight) return;

            if (this.key == key)
            {
                for (int i = 0; i < highlighters.Count; i++)
                {
                    var item = highlighters[i];
                    if (item)
                    {
                        item.FlashingOff();
                        item.Off();
                    }
                }
            }
        }

    }
}