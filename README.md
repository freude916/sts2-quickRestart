# QuickRestart2 — 快速重打 Mod for 尖塔2

## 简介

在暂停菜单中新增一个**「重打」**按钮。点击后读取游戏自动存档（`current_run.save`），快速重开本场战斗。

- 仅在单人模式下生效
- 无需手动存档，游戏进入房间 / 战斗时已自动存档
- 使用 Harmony 补丁注入，与游戏原生流程保持一致

## Introduction

Adds a **"Retry"** button to the pause menu. Clicking it reads the game's own autosave (`current_run.save`) and reloads the current room — effectively restarting the ongoing fight instantly.

- Works in singleplayer only
- No manual save needed; the game autosaves on room/combat entry
- Injected via Harmony patch, following the same load flow as the main menu "Continue"

---

## Mod 使用指南 / Installation Guide

尖塔2官方已支持 Modding，并提供了大量钩子和 Harmony 支持。

1. 在游戏目录下创建 `mods` 文件夹
2. 将 `.pck` 和 `.dll` 文件放入该文件夹
3. 启动游戏并开启 Mod

> ⚠️ 开启 Mod 后游戏内会弹出提示，并**会导致存档丢失**。
> 按 **`` ` ``** 键打开控制台，输入 `unlock` 即可解锁全部内容继续爽玩。

---

Slay the Spire 2 officially supports modding with extensive hooks and Harmony integration.

1. Create a `mods` folder in the game's root directory
2. Drop the `.pck` and `.dll` files into it
3. Launch the game and enable the mod

> ⚠️ Enabling mods will trigger an in-game warning and **will wipe your save**.
> Press **`` ` ``** to open the console and type `unlock` to restore all unlocks。
