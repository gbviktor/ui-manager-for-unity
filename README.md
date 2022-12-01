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
		- this Scriptable Object looking for **Prefabs** inside this Folder (Root only) of type *UIScreen* and update list of available *Screens*
		- just put your *Prefab* in this Folder to make it visible for *UI Manager*
- after, you can create your first *Screen* inherited from **UIScreen** class
	- see *UITestScreen* as a sample
- before *UI Manager* can work, initialize it with followed code (do it once, this script use static fields)
```csharp  //pickup a ScriptableObject of type ScreensList 
[SerializeField] ScreensList screensList;

void Awake(){
	//setup a Loader and
	new UI(new UILoaderResources(screensList))
}
```
- now, you can call `UI.OpenScreen<UITestScreen>()` or `UI.OpenNested<UITestScreen>()`
	- `OpenNested` open Screens with save previous Screen in Stack, to open it back again, if you close current. (also save history)


## API
- `UI.Open<UIYourScreenType>()`
	- open new Screen and close automaticaly Previous (disabled/removed)
- `UI.OpenNested<UIYourNextScreen>()` 
	- open new Screen, and save previous in History (stack) open it, if you close current
- `UI.OpenAndCloseAllPrevious<UIMyScreen>()` 
	- close all Screen (clear history/stack), and open requested Screen
- `UI.OnBackButtonPressed()` - use it to handle users inputs, 
	- this method will be close current Screen and open previouse if History is not empty.
- `UI.ForceCloseAll()` - force close all opened Screens or saved in History (Stack)
	- use this, for Scenes transitions, to clear history of opened screens.
- `UI.CloseCurrentFocusPrevious()` - close current screen, focus previous
- `UI.Get<UIMyScreen>()` - instantiate a Screen, but not Open it (don't call OnOpen event)
	- you can manipulate or execute with this Screen before open
		- but i will be not recomend this method, smell like a bad practice
- `UI.DestroyScreen<UIMyScreen>()` - remove/destroy this screen from Scene
	- use it careful, only if you manuly want close screen and can't use `UI.CloseCurrentFocusPrevious()`

## Create your custom Screen
- create a new Canvas in Scene
- add you script/component inherited from *UIScreen* to this Canvas
- rename it, i recomend to name it like `UI(some your name)Screen`
- save this Canvas as Prefab in root of Folder where a ScriptableObject of type *ScreensList* already exist, or create it inside
	- this SO help you, it's make a updatable list of available Screens inside folder where SO persistant
- after this Steps, you can call in your code `UI.OpenNested<UISomeMyNewScreen>()`
	- if you got error, check this step again.

## Feedback is awaited
- under development...