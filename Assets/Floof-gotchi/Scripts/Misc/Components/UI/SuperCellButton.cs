using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Floof
{
    public class SuperCellButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private const float DURATION = 0.05f;
        private const float SCALE_DOWN = 0.9f;
        private Vector3 _originalScale;

        private bool _pressed = false;
        private Button _button;


        private void Awake()
        {
            _button = GetComponent<Button>();
            _originalScale = transform.localScale;
        }

        private void EnablePadding(bool enable)
        {
            var graphic = _button.targetGraphic;
            if (graphic == null)
            {
                Debug.LogWarning(gameObject.name + " does not have graphic!");
                return;
            }
            if (!enable)
            {
                graphic.raycastPadding = Vector4.zero;
                return;
            }

            var widthPadding = graphic.rectTransform.rect.width * (1 - SCALE_DOWN);
            var heightPadding = graphic.rectTransform.rect.height * (1 - SCALE_DOWN);

            var padding = graphic.raycastPadding;
            padding.x -= widthPadding;
            padding.z -= widthPadding;
            padding.y -= heightPadding;
            padding.w -= heightPadding;

            graphic.raycastPadding = padding;
        }

        public void UpdateOriginalScale()
        {
            _originalScale = transform.localScale;
        }

        private void OnEnable()
        {
            transform.localScale = _originalScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable) { return; }

            _pressed = true;
            EnablePadding(true);
            Scale(false);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerExit(eventData);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_button.interactable) { return; }

            _pressed = false;
            Scale(true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (_pressed) { Scale(true); }

            _pressed = false;
            EnablePadding(false);
        }

        private void Scale(bool up)
        {
            var scaleTo = _originalScale * (up ? 1 : SCALE_DOWN);

            transform.DOKill();
            transform.DOScale(scaleTo, DURATION).SetLink(gameObject);
        }
    }
}