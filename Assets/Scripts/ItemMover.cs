using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class ItemMover : MonoBehaviour
{
    public Action OnItemClear;

    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private IndexProvider _indexProvider;
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private MatchChecker _matchChecker;

    private Item _firstElement;
    private Item _secondElement;

    private bool _hasFirstElement;
    private bool _hasSecondElement;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _hasFirstElement == false) //захват первого элемента
        {
            _hasFirstElement = SelectElement(out _firstElement);
        }

        if (Input.GetMouseButtonUp(0) && _hasFirstElement) //захват второго элемента
        {
            _hasSecondElement = SelectElement(out _secondElement);

            if (!_hasSecondElement) //если не оказалось элемента под мышью то сбрасываем так же и первый элемент
            {
                _hasFirstElement = false;
            }
        }

        if (_hasFirstElement && _hasSecondElement) //если оба элемента есть
            //и выполняется условие проверки, то меняем их местами
        {
            if (CanMove(_firstElement,_secondElement))
            {
                //сразу сбросили наличие элементов
                _hasFirstElement = false;
                _hasSecondElement = false;

                SwapElements(_firstElement, _secondElement);
                ReversElementsInArray(_firstElement,_secondElement);

                if (!_matchChecker.CheckAllMap(_mapBuilder
                        .Items))
                {
                    SwapElements(_secondElement,_firstElement);
                    ReversElementsInArray(_secondElement,_firstElement);
                }
                else
                {
                    StartCoroutine(ClearElements(_matchChecker.MatchItems));
                }
            }
            else
            {
                _hasFirstElement = false;
                _hasSecondElement = false;
            }
            
        }
    }
    
    private bool SelectElement(out Item element)
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(mousePosition, Vector2.one);

        if (hit.collider != null)
        {
            element = hit.collider.GetComponent<Item>();
            return true;
        }
        else
        {
            element = null;
            return false;
        }
    }
    
    private bool CanMove(Item firstElement, Item secondElement) //проверяем что объекты рядом и что двигать планируем не диагонально
    {
        var firstElementCoordinates = _indexProvider.GetIndex(firstElement.transform.parent.transform.localPosition);
        var secondElementCoordinates = _indexProvider.GetIndex(secondElement.transform.parent.transform.localPosition);

        // если индекс x отличается на единицу а у не отличается или если у отличается на единицу а х не меняется
        if (firstElementCoordinates.x == secondElementCoordinates.x &&
            Math.Abs(firstElementCoordinates.y - secondElementCoordinates.y) == 1)
        {
            return true;
        }

        else if (firstElementCoordinates.y == secondElementCoordinates.y &&
                 Math.Abs(firstElementCoordinates.x - secondElementCoordinates.x) == 1)
        {
            return true;
        }

        return false;
    }
    
    private void SwapElements(Item firstElement, Item secondElement)
    {
        var firstElementStartPosition = firstElement.transform.position;
        var secondElementStartPosition = secondElement.transform.position;

        _firstElement.transform
            .DOMove(secondElementStartPosition, _moveDuration)
            .SetEase(Ease.OutQuint);

        _secondElement.transform
            .DOMove(firstElementStartPosition, _moveDuration)
            .SetEase(Ease.OutQuint);
    }
    
    private void ReversElementsInArray(Item firstElement,Item secondElement)
    {
        var firstParent = firstElement.transform.parent; //взяли
        var secondParent = secondElement.transform.parent;//два объекта
            
        //получили их координаты в массиве через родителей
        var firstElementCoordinates = _indexProvider.GetIndex(firstParent.transform.localPosition);
        var secondElementCoordinates = _indexProvider.GetIndex(secondParent.transform.localPosition);
        
        //тут меняем элементы местами в массиве с элементами
        _mapBuilder.Items[firstElementCoordinates.y, firstElementCoordinates.x] = secondElement;
        _mapBuilder.Items[secondElementCoordinates.y, secondElementCoordinates.x] = firstElement;

        //установили новых родителей
        _mapBuilder.Items[secondElementCoordinates.y, secondElementCoordinates.x].transform
            .SetParent(secondParent.transform);
        _mapBuilder.Items[firstElementCoordinates.y, firstElementCoordinates.x].transform
            .SetParent(firstParent.transform);
    }

    private IEnumerator ClearElements(List<Item> matchItems)
    {
        for (int i = 0; i < matchItems.Count; i++)
        {
            var currentElement = matchItems[i].transform;
            currentElement
                .DOScale(1.2f, _moveDuration / 4)
                .SetDelay(_moveDuration / 2)
                .OnComplete(() => currentElement
                    .DOScale(0, _moveDuration)
                    .OnComplete(() => Destroy(currentElement.gameObject)));
        }

        yield return new WaitForSeconds(2);
        OnItemClear?.Invoke();
        
        yield return new WaitForSeconds(1);

        if (_matchChecker.CheckAllMap(_mapBuilder
                .Items))
        {
            StartCoroutine(ClearElements(_matchChecker.MatchItems));
        }

    }

    public void MoveElementDown(Item item)
    {
        var firstParent = item.transform.parent; //получили родителя первого элемента
        
        //получили координаты в массиве первого элемента!!!!!!!!!!!!!!!!!!!
        var firstElementCoordinates = _indexProvider.GetIndex(firstParent.transform.localPosition);
        
        //получаем координаты родителя снизу в массиве
        var secondElementCoordinates = new Vector2Int(firstElementCoordinates.x, firstElementCoordinates.y - 1);
        
        //получили нижнего родителя 
        var secondParent = _mapBuilder.TileMap[secondElementCoordinates.y,secondElementCoordinates.x];
        
        var firstElementStartPosition = item.transform;
        var secondElementPosition = secondParent.transform.position;
        
        firstElementStartPosition.transform
            .DOMove(secondElementPosition, _moveDuration)
            .SetEase(Ease.OutQuint);
        
        item.transform.SetParent(secondParent.transform);
        
        _mapBuilder.Items[firstElementCoordinates.y,firstElementCoordinates.x] = null;
        _mapBuilder.Items[secondElementCoordinates.y, secondElementCoordinates.x] = item;
    }
}