using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Item _prefab;
    public Item Item { get; private set; }
    
    public Item Initialize(ScriptableItemSettings itemSettings)
    {
        Item = Instantiate(_prefab, transform, false);
        Item.Initialize(itemSettings);
        
        return Item;
    }
}
