using UnityEngine;

namespace Game.UI
{
    public class ElementUI : MonoBehaviour
    {
        protected bool Show
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
    }
}