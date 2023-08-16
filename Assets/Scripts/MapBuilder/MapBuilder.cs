using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private RectTransform _map;
    [SerializeField] private ItemRandomiser _itemRandomiser;

    [SerializeField] private int _countItemsRow;
    [SerializeField] private int _countItemsColumn;

    [SerializeField] private Tile _tilePrefab;

    private IndexProvider _indexProvider;
    public Tile[,] TileMap => _tileMap;
    private Tile[,] _tileMap;
    public Item[,] Items => _items;
    private Item[,] _items;

    public void Initialize(IndexProvider indexProvider)
    {
        _indexProvider = indexProvider;

        _tileMap = new Tile [_countItemsRow, _countItemsColumn];
        _items = new Item [_countItemsRow, _countItemsColumn];

        _indexProvider.Initialize(_countItemsColumn, _countItemsRow);
        _itemRandomiser.Initialize(_tileMap);
    }

    public void InitializeMap()
    {
        for (int i = 0; i < _countItemsRow; i++)
        {
            for (int j = 0; j < _countItemsColumn; j++)
            {
                _tileMap[i, j] = Instantiate(_tilePrefab, _map, false);
                _tileMap[i, j].transform.localPosition = _indexProvider.GetCoordinates(j, i);
            }
        }
    }

    public void SetItems()
    {
        for (int i = 0; i < _countItemsRow; i++)
        {
            for (int j = 0; j < _countItemsColumn; j++)
            {
                if (_tileMap[i, j].transform.childCount == 0)
                {
                    //запускаем поиск рандомного элемента и когда находим инсталим его в текущую клетку
                    _items[i, j] = _tileMap[i, j].Initialize(_itemRandomiser.GetRandomElement(true, i, j));
                }
            }
        }
    }
}