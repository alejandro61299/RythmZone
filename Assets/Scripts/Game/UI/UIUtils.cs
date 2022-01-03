using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public static class UIUtils
    {
        public static void UpdateLayoutHierarchy(RectTransform root)
        {
            foreach (RectTransform child in root)
                UpdateLayoutHierarchy(child);
            if (root.GetComponent<LayoutGroup>())
                RebuildLayout(root);
        }
        
        private static void RebuildLayout(RectTransform transform) =>
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
    }
}