using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class FloofController
    {
        public FloofEntity Floof { get; private set; }
        private FloofStateMachine _floofStateMachine;

        public FloofController(FloofEntity floof)
        {
            Floof = floof;
        }

        public void ResetFloof()
        {
            Floof.StopMoving();
            Floof.StartWandering();
        }

    }
}