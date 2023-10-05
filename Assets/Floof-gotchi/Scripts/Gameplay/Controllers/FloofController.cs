using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class FloofController
    {
        private float _happiness, _hunger, _hygiene, _sleep;
        public FloofView Floof { get; private set; }

        public FloofController(FloofView floofView)
        {
            Floof = floofView;
            // Floof.StartWandering();
        }

        public void ResetFloof()
        {
            Floof.StopMoving();
            Floof.StartWandering();
        }

    }
}