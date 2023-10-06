using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Floof
{
    public class ClassicGameController
    {
        private PlayView _playView;
        private FloofController _floofController;

        public ClassicGameController(PlayView playView)
        {
            _playView = playView;
            playView.GoToScene(GameScene.LivingRoom);

            var floof = playView.Floof;
            floof.Setup(playView.CurrentScene.MoveSpace);
            floof.StartWandering();

            _floofController = new FloofController(floof);
        }
    }
}
