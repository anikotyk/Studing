using System.Collections.Generic;
using GameBasicsCore.Plugins.Tools.GameBasicsTools.Extensions;
using UnityEngine;

#pragma warning disable 0649
namespace GameCore.Common.UI
{
    public class EndlessMove : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _items;
        [SerializeField] private float _speed = 50;
        [SerializeField] private float _moveAtTheBeginningAfter = -1200;

        private int _countFrame;
        
        private void Update()
        {
            // Let's skip some frames while canvas is not finalized
            if (++_countFrame < 10) return;
            
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                item.anchoredPosition = item.anchoredPosition.AddY(-_speed * Time.unscaledDeltaTime);
                if (item.anchoredPosition.y < _moveAtTheBeginningAfter)
                {
                    var prev = GetPrev(item);
                    item.anchoredPosition = item.anchoredPosition.SetY(prev.anchoredPosition.y + item.rect.height);
                }
            }
        }

        private RectTransform GetPrev(RectTransform item)
        {
            var index = _items.IndexOf(item);
            return index == 0 ? _items[^1] : _items[index - 1];
        }
    }
}