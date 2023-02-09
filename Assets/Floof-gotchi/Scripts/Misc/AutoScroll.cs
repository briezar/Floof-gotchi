using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private RawImage _image;
    [SerializeField] Vector2 _direction;

    private void Update()
    {
        _image.uvRect = new Rect(_image.uvRect.position + _direction * Time.deltaTime, _image.uvRect.size);
    }

}
