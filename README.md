# FazAppFramework
 
FazApp Framework is a package for Unity that helps with mobile games development for Android (and iOS in the future). It is mainly used for hyper casual, free to play games.

# Features
 - Preconfigured mobile services like:
   * **Admob** - AdManager handles ad loading, displaying, and rewarding player (with Rewarded video ads)
   * **Firebase** - simple data collection with events from Analytics. Configured Cloud Messaging for remote notifications from console and RemoteConfig - for showing own interstitial ads (with own games for example)
   * **Local Notifications** with 24, 48, 72 hour fire times and 96 hours repeatable notifcation
 - Configuration tool with often used options from editor and quick services setup. Everything is saved to JSON file in Assets folder
   ![Configurator](/FAFrameworkConfScreen.png)
 - **Dependency Injection** that uses Autofac. You can simply add types to register as dependencies before Framework initialization and resolve registered objects (for example AdManager for displaying ads)
 - **MVVM** (in progress) - provides good UI architecture. Simply derive from View and ViewModel classes with data binding functionality (or right click in Project window -> Create -> Scripts -> ViewViewModel and use provided tool for creating these files)
 - Own **Event System** for easy tracking and blocking events. Each GameEvent knows sender and can have additional data
 - MonoBehaviour is replaced with **MainBehaviour** - for adding some default functionality (like checking SerializedFields or injecting components)
 - Few additional Attributes, Extensions and small editor tools to speed up developmnent - also in progress
 
# Requirements
This package requires:
 - Autofac
 - Admob
 - Firebase Analytics, Messaging and Remote Config
 - Unity Local Notifications
 
I hope that Admob will be soon published as package in Package Manager so I can simply add it as dependecy in my package.json file (for now Firebase and Local Notifications are added like that). It will make framework installation much easier.
 
# TODO
 - **DOCUMENTATION**
 - Facebook service integration
 - Something for sharing our game (maybe Facebook SDK)
 - UI - my goal is to provide tool for fast UI creation, probably with premade prefabs like Buttons, Texts and few ready to use panels like Rate Us window or Own Interstital
