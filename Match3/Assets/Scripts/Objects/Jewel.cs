using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Jewel : MonoBehaviour
{
    private bool _canMoveRight = false;
    private bool _canMoveLeft = false;
    private bool _canMoveTop = false;
    private bool _canMoveBottom = false;
    private int _type;
    private GameManager _gm;
    private Vector3 _screenPoint;
    private Vector3 _offset;
    private Vector3 _startPosition;


    public int type
    {
        get
        {
            return _type;
        }
        private set
        {
            _type = value;
        }
    }

    public void ChangeType(int newType)
    {
        _type = newType;
    }

    public void Init()
    {
        _gm = GetComponentInParent<GameManager>();
	}

    private void OnMouseDown()
    {
        if (_gm.canPlay)
        {
            // проверяем 3 варианта если этот кристал уничтожит те которые рядом
            // потом проверяем уничтожит ли соседний камень с той стороны если передвинуть его на наше текущее место
            // 0 возможное уничтожение кристалов X с кем мы можем поменятся J наш кристалл 
            //    0
            //    0
            //    X
            // 00XJX00 
            //    X
            //    0
            //    0

            _canMoveRight = (_gm.IsSameType(_type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y + 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y + 2)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x + 2, (int)transform.localPosition.y) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x + 3, (int)transform.localPosition.y)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y - 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y - 2));
            // можно было записать в одно сравнение но не всегда у нас есть та ячейка с которой сравнивать
            if (!_canMoveRight)
            {
                try
                {
                    _canMoveRight =
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x + 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y + 1) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x + 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y + 2)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x + 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x + 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x - 2, (int)transform.localPosition.y)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x + 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y - 1) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x + 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y - 2));
                }
                catch { }
            }

            _canMoveLeft = (_gm.IsSameType(_type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y + 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y + 2)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x - 2, (int)transform.localPosition.y) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x - 3, (int)transform.localPosition.y)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y - 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y - 2));

            if (!_canMoveLeft)
            {
                try
                {
                    _canMoveLeft = (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x - 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y + 1) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x - 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y + 2)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x - 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x - 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x + 2, (int)transform.localPosition.y)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x - 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y - 1) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x - 1, (int)transform.localPosition.y].type, (int)transform.localPosition.x, (int)transform.localPosition.y - 2));
                }
                catch { }
            }

            _canMoveTop = (_gm.IsSameType(_type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y + 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x + 2, (int)transform.localPosition.y + 1)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x, (int)transform.localPosition.y + 2) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x, (int)transform.localPosition.y + 3)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y + 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x - 2, (int)transform.localPosition.y + 1));

            if (!_canMoveTop)
            {
                try
                {
                    _canMoveTop = (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y + 1].type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y + 1].type, (int)transform.localPosition.x + 2, (int)transform.localPosition.y)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y + 1].type, (int)transform.localPosition.x, (int)transform.localPosition.y - 1) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y + 1].type, (int)transform.localPosition.x, (int)transform.localPosition.y - 2)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y + 1].type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y + 1].type, (int)transform.localPosition.x - 2, (int)transform.localPosition.y));
                }
                catch { }
            }


            _canMoveBottom = (_gm.IsSameType(_type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y - 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x + 2, (int)transform.localPosition.y - 1)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x, (int)transform.localPosition.y - 2) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x, (int)transform.localPosition.y - 3)) ||
                (_gm.IsSameType(_type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y - 1) &&
                _gm.IsSameType(_type, (int)transform.localPosition.x - 2, (int)transform.localPosition.y - 1));

            if (!_canMoveBottom)
            {
                try
                {
                    _canMoveBottom = (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y - 1].type, (int)transform.localPosition.x + 1, (int)transform.localPosition.y) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y - 1].type, (int)transform.localPosition.x + 2, (int)transform.localPosition.y)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y - 1].type, (int)transform.localPosition.x, (int)transform.localPosition.y + 1) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y - 1].type, (int)transform.localPosition.x, (int)transform.localPosition.y + 2)) ||
                    (_gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y - 1].type, (int)transform.localPosition.x - 1, (int)transform.localPosition.y) &&
                    _gm.IsSameType(_gm.fieldData[(int)transform.localPosition.x, (int)transform.localPosition.y - 1].type, (int)transform.localPosition.x - 2, (int)transform.localPosition.y));
                }
                catch { }
            }
            // не плохо былобы вынести это в отдельную функцию и сделать универсальной

            _screenPoint = Camera.main.WorldToScreenPoint(transform.position);

            _offset = transform.position - Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
            _startPosition = transform.position;
        }
    }

    private void OnMouseDrag()
    {
        if (_gm.canPlay)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
            GameObject anotherJewel;
            // проверки на обнуление если в ту сторону нельзя двигаться
            if (!_canMoveRight && curPosition.x > _startPosition.x)
            {
                curPosition.x = _startPosition.x;
            }
            if (!_canMoveLeft && curPosition.x < _startPosition.x)
            {
                curPosition.x = _startPosition.x;
            }
            if (!_canMoveTop && curPosition.y > _startPosition.y)
            {
                curPosition.y = _startPosition.y;
            }
            if (!_canMoveBottom && curPosition.y < _startPosition.y)
            {
                curPosition.y = _startPosition.y;
            }
            // проверки чтобы нельзя было двигать бесконечно
            if (_canMoveRight && curPosition.x > _startPosition.x + _gm.spaceBetween)
            {
                curPosition.x = _startPosition.x + _gm.spaceBetween;
            }
            if (_canMoveLeft && curPosition.x < _startPosition.x - _gm.spaceBetween)
            {
                curPosition.x = _startPosition.x - _gm.spaceBetween;
            }
            if (_canMoveTop && curPosition.y > _startPosition.y + _gm.spaceBetween)
            {
                curPosition.y = _startPosition.y + _gm.spaceBetween;
            }
            if (_canMoveBottom && curPosition.y < _startPosition.y - _gm.spaceBetween)
            {
                curPosition.y = _startPosition.y - _gm.spaceBetween;
            }

            transform.position = curPosition;
        }
    }
}
