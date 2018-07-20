﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace InteractSystem {

    public class AngleCtroller
    {
        private Queue<GameObject> objectQueue = new Queue<GameObject>();
        private Dictionary<Transform, GameObject> actived = new Dictionary<Transform, GameObject>();
        private ActionSystem actionSystem;
        private static AngleCtroller _instence;
        public static AngleCtroller Instence
        {
            get
            {
                if (_instence == null)
                {
                    _instence = new AngleCtroller(ActionSystem.Instence);
                }
                return _instence;
            }
        }
        public GameObject this[Transform target]
        {
            get
            {
                if (actived.ContainsKey(target))
                {
                    return actived[target];
                }
                return null;
            }
        }

        private AngleCtroller(ActionSystem system)
        {
            actionSystem = system;
        }


        public void Notice(Transform target,GameObject angle,bool update = false)
        {
            if (!Config.Instence.actionItemNotice) return;

            if (!actived.ContainsKey(target))
            {
                actived.Add(target, GetAngleInstence(target, angle));
            }
            else
            {
                if(update){
                    CopyTranform(actived[target].transform, target);
                }
            }
        }
        public void UnNotice(Transform target)
        {
            if (!Config.Instence.actionItemNotice) return;

            if (actived.ContainsKey(target))
            {
                HideAnAngle(actived[target]);
                actived.Remove(target);
            }
        }

        private GameObject GetAngleInstence(Transform target,GameObject anglePrefab)
        {
            GameObject angle = null;
           
            if (objectQueue.Count > 0)
            {
                angle = objectQueue.Dequeue();
            }
            else
            {
                angle = Object.Instantiate(anglePrefab);
                angle.transform.SetParent(actionSystem.transform);
            }
            CopyTranform(angle.transform, target);
            angle.SetActive(true);
            HighLighter(angle);
            return angle;
        }

        public static void CopyTranform(Transform obj, Transform target)
        {
            obj.transform.position = target.transform.position;
            obj.transform.rotation = target.transform.rotation;
        }

        private void HideAnAngle(GameObject angle)
        {
            HighLighter(angle);
            angle.gameObject.SetActive(false);
            objectQueue.Enqueue(angle);
        }

        private void HighLighter(GameObject angle)
        {
            //highLightDic[angle].FlashingOn(Color.white, highLightColor);
            //highLightDic[angle].SeeThroughOn();
        }
        private void UnHighLighter(GameObject angle)
        {
            //highLightDic[angle].FlashingOff();
        }
    }
}