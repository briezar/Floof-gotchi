using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicController
{
    private PlayUI _gameView;
    public ClassicController()
    {
        UIManager.ShowUI<PlayUI>((playUI) => _gameView = playUI);
    }

}
