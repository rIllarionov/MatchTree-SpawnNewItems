
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private Sprite _sprite;
    private int _type;
    public int Type => _type;
    
    public void Initialize(ScriptableItemSettings itemSettings)
    {
        var sprite = GetComponent<Image>();
        sprite.sprite = itemSettings.Sprite;
        _type = itemSettings.Type;
        PlayTileAnimation();
    }

    private void PlayTileAnimation()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.
            DOScale(new Vector3(1, 1, 0), 0.5f).
            SetEase(Ease.InSine);
    }
}
