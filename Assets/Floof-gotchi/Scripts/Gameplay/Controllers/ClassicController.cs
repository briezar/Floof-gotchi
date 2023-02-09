using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClassicController
{
    private PlayUI _gameView;
    public ClassicController()
    {
        _gameView = UIManager.ShowUI<PlayUI>();
    }

}
