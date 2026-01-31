# GTA-like Unity Android (URP)

Оригинальная GTA-like 3D игра для Android на **Unity 2022 LTS (2022.3.x)** и **URP** с компонентной архитектурой и ScriptableObjects.

## Требования
- Unity Hub + Unity 2022.3 LTS.
- Модуль **Android Build Support** (SDK/NDK + OpenJDK) установлен через Unity Hub.
- Подключение к локальной сети (для тестов LAN-мультиплеера).

## Как открыть проект
1. Откройте Unity Hub.
2. Нажмите **Open** и выберите корневую папку репозитория.
3. Убедитесь, что проект запускается в Unity 2022.3 LTS.

## Настройки Android
В `ProjectSettings/ProjectSettings.asset` уже заданы базовые настройки:
- Min SDK: 24
- Target SDK: 33
- Package Name: `com.example.gtalikeunity`

При необходимости измените их в **Project Settings → Player → Android**.

## Сборка APK
1. Откройте **File → Build Settings**.
2. Выберите **Android** и нажмите **Switch Platform**.
3. Убедитесь, что сцена `Assets/Scenes/SampleScene.unity` добавлена в Build.
4. Нажмите **Build** и укажите путь для APK.

## Что реализовано
- Процедурная генерация города (дороги, тротуары, здания с LOD).
- Персонаж от третьего лица с камерой и мобильным вводом.
- Машины на Rigidbody + WheelCollider и пылевые эффекты.
- Три вида оружия (пистолет/автомат/дробовик) с Raycast-уроном.
- Мини-карта и полноэкранная карта.
- Меню, экран друзей (LAN), настройки качества.
- Локальный мультиплеер (P2P) через Netcode for GameObjects + LAN discovery.

## URP
Проект подключает **Universal Render Pipeline** через `Packages/manifest.json`. При первом открытии Unity может предложить обновить/перегенерировать URP-ресурсы — согласитесь.

## Быстрый старт (Play Mode)
1. Откройте сцену `Assets/Scenes/SampleScene.unity`.
2. Добавьте на сцену `NetworkManager` + `UnityTransport`.
3. Создайте `GameBootstrap` и привяжите `CityGenerator`, `PlayerSpawner` и `LanDiscovery`.
4. Создайте UI: джойстик, кнопки действия/огня, переключатель карты, меню.
5. Нажмите Play и создайте host/client через UI.
