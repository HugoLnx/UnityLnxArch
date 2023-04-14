[![npm package](https://img.shields.io/npm/v/com.sensengames.unity-lnx-arch)](https://www.npmjs.com/package/com.sensengames.unity-lnx-arch)
[![openupm](https://img.shields.io/npm/v/com.sensengames.unity-lnx-arch?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.sensengames.unity-lnx-arch/)
![Tests](https://github.com/sensengames/unity-lnx-arch/workflows/Tests/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

# Lnx Arch

Make your Unity project cleaner and more modular with those features inspired by Dependency Injection solutions and Scriptable Objects Architecture.

- [How to use](#how-to-use)
- [Install](#install)
  - [via npm](#via-npm)
  - [via OpenUPM](#via-openupm)
  - [via Git URL](#via-git-url)
  - [Tests](#tests)
- [Configuration](#configuration)

<!-- toc -->

## Overview
This library is composed by the following 4 features:

### LnxEntities - Solid Concept References
It serves as a reference point for a object that is root of a solid concept in your game, like a Player, a Gun, an Item, etc. With that reference point, you'll be able to autofetch the Components and Behaviours related to that concept automatically, without having to use GetComponent/GetComponentChildren, etc.

### Autofetch Methods - Cleaner Component Fetching
Add the attribute `[AutoFetch]` to a method so its parameters will be automatically fetched if they are on the same Entity. AutoFetch methods are called before Awake and it'll be called in a order so the dependencies are initialized first. If the Entity locator doesn't fit, you can use the attributes `[FromLocal]`, `[FromLocalChild]`, `[FromLocalAncestor]`, `[FromParentEntity]` to specify how to locate a specific parameter.

### Components and Events - Simple Behaviour Dependencies
Components are single-valued behaviours that serves as contracts to more complex behaviours, it can be the hero Health, Mana, it's FacingDirection.
Events are behaviours with a single event declaration, so more complex behaviours can act when the player dies, touches an item, or other events like that.
Having behaviours that depend on those simple components makes them extremelly reusable.

### Channels - Systems Communication via Scriptable Objects
Channels are Scriptable Objects that can be linked to Components and Events so their changes be automatically broadcasted to other Behaviours that are also linked to the Channel. It's a clean way of having different parts of your architecture communicating without having hard dependency to it.


## How to use

### DISCLAIMER
This library is in extremelly early stages of development, I'll start using in some prototypes now, so it'll probably change a lot, don't use in production.
It also doesn't have tests yet, I've done everything I could in a week so I can test the concept before investing too much time in it. I hope it works as I think it'll, but who knows, I'll only know in practice.

### LnxEntity
Put the LnxEntity at the root of the object that represent the desired concept you want to reference (Player, Item, Enemy, etc).

### Autofetch Method
In a MonoBehaviour, add the attribute `[Autofetch]` at the top of the methods you want to be called before Awake with the dependencies solved (you can have multiple if you want). Autofetch methods can only be used in MonoBehaviours that are children of an `LnxEntity` (in the GameObjects hierarchy). By default, the Autofetcher will get any Component that's in a child of the `LnxEntity`.


**Attributes for Fetching**  
You can also use the following attributes on the parameters to specify a different type of lookup:

- `[FromEntity]`(default): Fetch any component in the entity that owns the current behaviour.
- `[FromParentEntity]`: Fetch any component in the parent entity.
- `[FromLocal]`: Fetch a sibling component from the current game object.
- `[FromLocalChild]`: Fetch a component from the current game object's children.
- `[FromLocalAncestor]`: Fetch a component from the current game object's ancestors.


**Fetch Multiple Components**  
To fetch multiple components, just declare the parameter as an `array[]` or `List<>`. It'll automatically detect that and get multiple components instead of only one.


```C#
// Sample
public class GunBehaviour : MonoBehaviour
{
    [Autofetch]
    private void Prepare(
        [FromParentEntity]
        PlayerBehaviour player,

        [FromParentEntity]
        Transform parentEntityTransform,

        [FromLocal]
        Transform localTransform,

        [FromLocalChild]
        Collider childCollider,

        [FromLocalAncestor]
        Collider ancestorCollider,

        Collider[] colliders,

        List<Collider> collidersList,

        [FromParentEntity]
        Component[] allComponents
    )
    {
        Debug.Log($"Player: {player.gameObject.name}");
        Debug.Log($"PlayerTransform: {parentEntityTransform.gameObject.name}");
        Debug.Log($"LocalTransform: {localTransform.gameObject.name}");
        Debug.Log($"childCollider: {childCollider.gameObject.name}");
        Debug.Log($"ancestorCollider: {ancestorCollider.gameObject.name}");
        Debug.Log($"colliders: {string.Join(", ", colliders.Select(c => c.gameObject.name))}");
        Debug.Log($"collidersList: {string.Join(", ", collidersList.Select(c => c.gameObject.name))}");
        Debug.Log($"allComponents: {string.Join(", ", allComponents.Select(c => $"({c.GetType().Name}|{c.gameObject.name})"))}");
    }
}
```

**Create Your Own Fetching Attribute**  
You just need to create an attribute that implements the interface `IFetchAttribute`.

### Components and Events
Differently from the usual Scriptable Object Architecture solution. In LnxArch, Components and Events are MonoBehaviours that can be linked to a Channel (Scriptable Object). This gives semantical meaning to components in code, which allows us to make Behaviours to automatically locate the components and events.

**Components**  
Creating a component done by simply creating a class that extends from `LnxComponent<T>`:
```C#
public class HealthComponent : LnxComponent<int>
{}

// Some sample calls
HealthComponent health = // Fetch HealthComponent somehow
health.OnWrite += (value, _) => Debug.Log($"Health was set to {value}");
health.OnChange += (oldValue, newValue, _) => Debug.Log($"Health has change from {oldValue} to {newValue}");

health.Value = 10;
// Health was set to 10
// Health has change from 0 to 10

health.Value = 10;
// Health was set to 10

health.Value = 30
// Health was set to 30
// Health has change from 10 to 30
```

LnxComponents has:  
-  `property Value` property for reading and writing the value.
-  `event OnWrite (T value, LnxComponentSource<T> source)`: Raised everytime the value is set.
-  `event OnChange (T oldValue, T newValue, LnxComponentSource<T> source)`: Raised when the value changes.

[TODO: Explain LnxComponentSynchronizer and LnxComponentLightweight]

**Events**  
In LnxArch events extends from `LnxEvent<T>`:
```C#
public class PlayerDied : LnxVoidEvent
{}

PlayerDied died = // ... Fetch PlayerDied somehow
died.OnTrigger += (_, _) => Debug.Log("Player has died.")
died.Emit()

public class PlayerLevelUp : LnxEvent<Player>
{}

PlayerLevelUp levelUp = // ... Fetch PlayerLevelUp somehow
levelUp.OnTrigger += (player, _) => Debug.Log($"Player has leveled up to {player.Level}.")
levelUp.Emit(player)
```

LnxEvents has:  
-  `event OnTrigger (T args = default, LnxEventTriggerSource<T> source = default)`: Raised when the event is emitted
-  `Trigger(T args = default, LnxEventTriggerSource<T> source = default)`: To trigger the event


### Channels
Every Component and Event can be linked to a channel, when that happens, any other Component/Event that's linked to the same channel will have the same value and events triggered also will be triggered on all of the linked Components/Events.

A primitive component channel (with one of the primitive types) can be done by instantiating the default Scriptable Objects in the Right-Click menu (LnxArch > Component Channel > Float, Vector2, etc).
But you can also extend `LnxComponentChannel<T>` to create channel types for your custom types.
```C#
    [CreateAssetMenu(menuName = "LnxArchCustom/ComponentChannel/Player", fileName = "PlayerChannel")]
    public class PlayerComponentChannel : LnxComponentChannel<Player>
    {
    }
```

The same goes for events, but use the class `LnxEventChannel<T>` instead.


[TODO: Explain about `LnxPrefabAutofetcher`]

## Install

### via npm

Open `Packages/manifest.json` with your favorite text editor. Add a [scoped registry](https://docs.unity3d.com/Manual/upm-scoped.html) and following line to dependencies block:
```json
{
  "scopedRegistries": [
    {
      "name": "npmjs",
      "url": "https://registry.npmjs.org/",
      "scopes": [
        "com.sensengames"
      ]
    }
  ],
  "dependencies": {
    "com.sensengames.unity-lnx-arch": "1.0.0"
  }
}
```
Package should now appear in package manager.

### via OpenUPM

The package is also available on the [openupm registry](https://openupm.com/packages/com.sensengames.unity-lnx-arch). You can install it eg. via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.sensengames.unity-lnx-arch
```

### via Git URL

Open `Packages/manifest.json` with your favorite text editor. Add following line to the dependencies block:
```json
{
  "dependencies": {
    "com.sensengames.unity-lnx-arch": "https://github.com/sensengames/unity-lnx-arch.git"
  }
}
```

### Tests

*Work in progress*

The package can optionally be set as *testable*.
In practice this means that tests in the package will be visible in the [Unity Test Runner](https://docs.unity3d.com/2017.4/Documentation/Manual/testing-editortestsrunner.html).

Open `Packages/manifest.json` with your favorite text editor. Add following line **after** the dependencies block:
```json
{
  "dependencies": {
  },
  "testables": [ "com.sensengames.unity-lnx-arch" ]
}
```

## Configuration

Nothing to configure for now.

## License

MIT License

Copyright © 2023 Sensen Games
