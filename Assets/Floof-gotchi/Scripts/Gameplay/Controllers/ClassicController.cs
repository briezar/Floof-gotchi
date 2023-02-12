using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClassicController
{
    private PlayUI _gameView;
    private FloofController _floofController;

    public ClassicController()
    {
        _gameView = UIManager.ShowUI<PlayUI>();
        _floofController = new FloofController(_gameView.FloofView);
    }

}
