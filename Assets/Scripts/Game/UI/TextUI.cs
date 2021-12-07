using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class TextUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _mesh;
        public void SetText(string text) => _mesh.text = text;
        public void Clean() => _mesh.text = string.Empty;
    }
}
