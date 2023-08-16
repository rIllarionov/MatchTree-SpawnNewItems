using System;
using UnityEngine;

public class FreeTilesFinder : MonoBehaviour
{
    public Action<Item> OnHasFreeBottomTile;
    
    [SerializeField] private MapBuilder _mapBuilder;
    
    public void FindFreeTiles()

    {
        
        var hasFreeTiles = true;

        while (hasFreeTiles)
        {
            var freeTilesCount = 0;
            
            for (int i = 0; i < _mapBuilder.TileMap.GetLength(0); i++)
            {
                for (int j = 1; j < (_mapBuilder.TileMap.GetLength(1)); j++)
                {
                    var currentTile = _mapBuilder.TileMap[j, i];
                    var bottomTile = _mapBuilder.TileMap[j - 1, i];
                    
                    if (HasChild(currentTile) && !HasChild(bottomTile))
                    {
                        freeTilesCount++;
                        
                        var currentItem = currentTile.GetComponentInChildren<Item>();
                        OnHasFreeBottomTile?.Invoke(currentItem);
                    }
                }
            }

            if (freeTilesCount == 0)
            {
                hasFreeTiles = false;
            }
        }
    }

    private bool HasChild(Tile tile)
    {
        return tile.transform.childCount > 0;
    }
}
