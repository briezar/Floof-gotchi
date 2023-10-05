using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class GameObjectLifeCycleDelegate : MonoBehaviour
    {
        public Action Enabled;
        public Action Disabled;
        public Action Started;
        public Action Destroyed;
        public bool DestroyOnCall = true;

        public GameObjectLifeCycleDelegate DontDestroyOnCall()
        {
            DestroyOnCall = false;
            return this;
        }

        private void OnEnable()
        {
            if (Enabled == null) { return; }

            Enabled();
            if (DestroyOnCall) { Destroy(this); }
        }

        private void OnDisable()
        {
            if (Disabled == null) { return; }

            Disabled();
            if (DestroyOnCall) { Destroy(this); }
        }

        private void Start()
        {
            if (Started == null) { return; }

            Started?.Invoke();
            if (DestroyOnCall) { Destroy(this); }
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}