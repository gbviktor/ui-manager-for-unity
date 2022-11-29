using Core;

public static class UIScreenExtentions
{
    internal static T OpenScreenNested<T>(this UIScreen self, params object[] param) where T : UIScreen => UI.OpenNested<T>(param);
    internal static T OpenNested<T>(this UIScreen self, params object[] param) where T : UIScreen
    {
        return UI.OpenNested<T>(param);
    }
}
