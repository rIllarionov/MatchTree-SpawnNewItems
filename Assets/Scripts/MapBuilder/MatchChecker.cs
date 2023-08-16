
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{

    public List<Item> MatchItems => _matchItems;
    
    private List <Item> _matchItems;

    private void Awake()
    {
        _matchItems = new List<Item>();
    }
    
    public bool CheckAllMap(Item[,] items)
    {
        _matchItems.Clear();
        
        var width = items.GetLength(0);
        var height = items.GetLength(1);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                FindAllMatches(items, x, y);
            }
        }
        
        if(_matchItems.Count > 0)
        {
            // Удаляем одинаковые элементы из списка
            _matchItems = _matchItems.Distinct().ToList();
            return true;
        }

        return false;
    }
    
    private void FindAllMatches(Item[,] items, int x, int y)
    {
        var currentItem = items[x, y];
        
        // Временная проверка (так как сейчас еще не реализована механика заполнения освободившихся клеток)
        if (currentItem == null)
        {
            return;
        }

        // Проверка по горизонтали
        if (x > 0 && x < items.GetLength(0) - 1)
        {
            var leftItem = items[x - 1, y];
            var rightItem = items[x + 1, y];
            
            // Если есть совпадения типов с правым и левым элементом - добавляем их в список
            if (HasTypeMatches(currentItem, leftItem, rightItem))
            {
                AddMatchesToList(currentItem, leftItem, rightItem);
            }
        }

        // Проверка по вертикали
        if (y > 0 && y < items.GetLength(1) - 1)
        {
            var aboveItem = items[x, y + 1];
            var belowItem = items[x, y - 1];

            // Если есть вопадения типов с верхним и нижним элементом - добавляем их в список
            if (HasTypeMatches(currentItem, aboveItem, belowItem))
            {
                AddMatchesToList(currentItem, aboveItem, belowItem);
            }
        }
    }
    
    private bool HasTypeMatches(Item currentTile, Item previousTile, Item nextTile)
    {
        // Временно проверяем на null, так как еще нет заполнения пустых клеток
        if (previousTile == null || nextTile == null)
        {
            return false;
        }
        
        return previousTile.Type == currentTile.Type && nextTile.Type == currentTile.Type;
    }

    private void AddMatchesToList(Item currentTile, Item previousTile, Item nextTile)
    {
        _matchItems.Add(currentTile);
        _matchItems.Add(previousTile);
        _matchItems.Add(nextTile);
    }
    
    
}
