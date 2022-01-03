using Game.UI.Lock;
using UnityEngine;

namespace Game.UI.ButtonUI
{
    public class ButtonUILocker : UILocker<ButtonUI>
    {
        protected override void Lock()
        {
            Debug.Log(Element);
        }

        protected override void Unlock()
        {
           
        }
    }
}