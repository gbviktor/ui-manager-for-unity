using com.gbviktor.samples;
using com.gbviktor.UIManager;

using UnityEngine;

public class UIStarterDemo : MonoBehaviour
{
    [SerializeField] ScreensListScriptable screensList;

    private void Awake()
    {
        //Initialize UI Manager
        new UI(new UILoaderResources(screensList));

        //Open your first screen
        UI.OpenNested<UITestScreen>("First opened Screen, with some params");
    }

    public void OpenOtherScreen()
    {
        UI.OpenAndCloseAllPrevious<UITestScreen>("Opened with other params");
    }
}
