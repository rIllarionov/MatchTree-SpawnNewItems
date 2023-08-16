using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private MapBuilder _mapBuilder;
    [SerializeField] private IndexProvider _indexProvider;
    [SerializeField] private ItemMover _itemMover;
    [SerializeField] private FreeTilesFinder _freeTilesFinder;

    private void Awake()
    {
        _mapBuilder.Initialize(_indexProvider);
        _itemMover.OnItemClear += _freeTilesFinder.FindFreeTiles;
        _itemMover.OnItemClear += _mapBuilder.SetItems;
        _freeTilesFinder.OnHasFreeBottomTile += _itemMover.MoveElementDown;
    }

    private void Start()
    {
        _mapBuilder.InitializeMap();
        _mapBuilder.SetItems();
    }

    private void OnDestroy()
    {
        _itemMover.OnItemClear -= _freeTilesFinder.FindFreeTiles;
        _itemMover.OnItemClear -= _mapBuilder.SetItems;
        _freeTilesFinder.OnHasFreeBottomTile -= _itemMover.MoveElementDown;
    }
}
