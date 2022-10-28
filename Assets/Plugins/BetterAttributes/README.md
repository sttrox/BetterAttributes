# Better Attributes


![image](https://user-images.githubusercontent.com/22265817/181865901-35fea6f6-0b6e-4246-9df5-99e13cb5ed0f.png)

This package provides following features:

### Select Dropdown

This package contains base class for drawer with better dropdown.<br>
Better dropdown contains:
1. Search
2. Fast navigation
3. Grouping

### Select Implementation

Provides possibility to select interface implementation in Unity Inspector.

Usage:

```c#
[SelectImplementation] [SerializeReference]
private ISomeInterface someInterface;

[SelectImplementation] [SerializeReference]
private SomeAbstractClass someAbstractClass;

[SelectImplementation(typeof(SomeAbstractClass)] [SerializeReference]
private List<SomeAbstractClass> someAbstractClasses;

[SelectImplementation(typeof(ISomeInterface))] [SerializeReference]
private List<ISomeInterface> someInterfaces;
```

### Select Enum

Provides possibility to select enum value with better dropdown.<br>
Also supports **_flag_** enums.

Usage:

```c#
[SelectEnum] [SerializeField]
private KeyCode keyCode;
```

### Preview

Provides possibility to see object preview by clicking into the field in Unity Inspector.<br>
Supports preview for **_scene objects_** and **_prefabs_** object as well as **_textures_** and **_sprites_**.

Usage:

```c#
[Preview] [SerializeField]
private Sprite sprite;

[Preview] [SerializeField]
private SomeMonobehaviour someMonobehaviour;
```

### Read Only Field

Provides possibility to disable modification of fields in Unity Inspector but keep it displayed.

Usage:

```c#
[ReadOnlyField] [SerializeField] 
private SomeClass someClass;

[ReadOnlyField] [SerializeField] 
private float someFloat;

[ReadOnlyField] [TextArea(5, 10)] [SerializeField] 
private string someString;
```

### Rename Field

Provides possibility to rename label in Unity Inspector.

Usage:

```c#
[RenameField("New Name")] [SerializeField]
private string oldName;
```

### Gizmo / Gizmo Local

Provides possibility to set value for Vector3/Vector2/Quaternion/Bounds from scene view by dragging handles.<br>
`[GizmoLocal]` works only into `MonoBehaviour` Unity Inspector.

Usage:

```c#
[Gizmo]
[SerializeField] private Bounds bounds;
        
[Gizmo]
[SerializeField] private Vector3 vector3;
        
[Gizmo]
[SerializeField] private Quaternion quaternion;

[GizmoLocal]
[SerializeField] private Bounds boundsLocal;
        
[GizmoLocal]
[SerializeField] private Vector3 vector3Local;
        
[GizmoLocal]
[SerializeField] private Quaternion quaternionLocal;
```

### Editor Buttons

Provides possibility to display button for method in Unity Inspector.

Usages:

```c#
///Default usage of attribute.
[EditorButton]
private void SomeMethod()
{
//Some code.
}

///This button will call method with predefined parameters. 
///When invokeParams not specified will call with null.
[EditorButton(invokeParams: 10f)]
private void SomeMethod(float floatValue)
{
//Some code.
}

///This button will call method with predefined parameters. 
///When invokeParams not specified will call with null.
[EditorButton(invokeParams: new object[] { 10f, 10 })]
private void SomeMethod(float floatValue, int intValue)
{
//Some code.
}

/// This button will be in the same row with button for SomeMethod2.
/// But will be in the second position.
/// When captureGroup not specified each button placed in separate row.
/// When priority not specified buttons in one row sorted by order in code.
[EditorButton(captureGroup: 1, priority: 2)]
private void SomeMethod1()
{
//Some code.
}

[EditorButton(captureGroup: 1, priority: 1)]
private void SomeMethod2()
{
//Some code.
}

/// This button will have name "Some Cool Button".
/// When displayName not specified or null/empty/whitespace button 
/// will have name same as method.
[EditorButton(displayName: "Some Cool Button")]
private void SomeMethod()
{
//Some code
}
```

You can check constructors for `EditorButtonAttribute` there more specific options.

## Install
Window -> PackageManager -> Add package from GIT url...
```
https://github.com/uurha/BetterAttributes.git#upm
```
or if you want some specific version
```
https://github.com/uurha/BetterAttributes.git#<version>
```
You can check versions [here](https://github.com/uurha/BetterAttributes/releases).

Note: all versions related to tags. Use tag name, **_NOT_** release name.