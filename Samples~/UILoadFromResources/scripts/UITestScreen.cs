using com.gbviktor.UIManager;

using TMPro;

using UnityEngine;

namespace com.gbviktor.samples
{
    public class UITestScreen : UIScreen
    {
        [SerializeField] TextMeshProUGUI demoText;
        //event called if UI.OpenScreen<UITestScreen>(somevariable)
        protected override void OnOpen(params object[] param)
        {
            base.OnOpen(param);
            demoText.text = param[0].ToString();
        }

        //event called if previous screen will be closed, and this go in focus
        public override void OnFocus()
        {
            base.OnFocus();
        }
        //event called if this Screen is open, and antoher Screen will be open next
        public override void OnFocusLost()
        {
            base.OnFocusLost();
        }
        //event called if you open widget inside this screen
        protected override void OnWidgetOpen(UIBaseWidget widget)
        {
            base.OnWidgetOpen(widget);
        }

        //event will be called if backbutton pressed
        public override bool OnBackButtonEventUsed()
        {
            return base.OnBackButtonEventUsed();
        }
    }
}