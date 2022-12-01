# UI Manager for Unity

Created to help me, and maybe you, to navigate beetwen Screens, load Screen Prefabs, update list of available Screens in Editor etc.

## Prerequisites

### Unity 2022.1
This package is in development.

## Getting Started
- Import from Samples a *UILoadResources* sample
- create a Folder **UIScreens** in **Resources** folder
- now you can create a *ScriptableObject* from Asset Menu in created **UIScreens** folder
	- Montana Games/DB/Screens List
- after, you can create your first *Screen* inherited from **UIScreen** class
	- see *UITestScreen* as a sample
- before UIManager can work initialize it with followed code 
```csharp  //pickup a ScriptableObject of type ScreensList 
[SerializeField] ScreensList screensList;

void Awake(){
	//setup a Loader and
	new UI(new UILoaderResources(screensList))
}
```
- now, you can call `UI.OpenScreen<UITestScreen>()` or `UI.OpenNested<UITestScreen>()`
	- `OpenNested` open Screens with save previous Screen in Stack, to open it back again, if you close current. (also save history)