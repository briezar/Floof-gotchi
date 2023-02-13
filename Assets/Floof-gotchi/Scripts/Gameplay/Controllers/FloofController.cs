using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloofController
{
    private float _happiness, _hunger, _hygiene, _sleep;
    private FloofView _view;

    public FloofController(FloofView floofView)
    {
        _view = floofView;
    }

}
