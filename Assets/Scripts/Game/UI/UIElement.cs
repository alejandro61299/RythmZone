using System.Collections.Generic;
using UnityEngine;
using Utils.ListExtension;

namespace Game.UI
{
    public class UIElement : MonoBehaviour
    {
        protected bool Show
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
    }
}