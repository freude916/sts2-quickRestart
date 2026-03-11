This mod no longer maintain since English community have a activier one:

这个 Mod 不再维护，薄荷尖塔的维护者已经手搓了一个，安全性应该比我这个 Vibe 的强

<https://github.com/erasels/StS2-Quick-Restart>

---

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
3. 启动游戏

> ⚠️ 开启 Mod 后游戏内会弹出一次 Waring，确认**会导致存档丢失**。
> 
> 按 **`` ` ``** 键打开控制台，输入 `unlock` 即可解锁全部内容继续爽玩。
> 
> 或者开启过 Mod 之后通过下面方法恢复您的存档：
> 
> 
> 
> 游戏的 Modded 模式会创建另一个 modded 存档文件夹
> 
> 您可以在 `{Steam 目录}/userdata/<SteamID>/2868840/` (Windows / Linux / macOS 均适用) 下看到 `profile1 2 3` 和 `modded` 一个文件夹
> 
> 将 `profile1` 复制到 `modded` 中覆盖原文件夹，即可恢复存档。
> 
> 这其实是 Steam 云同步目录。因此在您回到 Steam 之后，Steam 会提示 `云冲突`，选择 `保留本地文件` 即可。


---

Slay the Spire 2 officially supports modding with extensive hooks and Harmony integration.

1. Create a `mods` folder in the game's root directory
2. Drop the `.pck` and `.dll` files into it
3. Launch the game and enable the mod

> ⚠️ Enabling mods will trigger an in-game warning and confirm **will wipe your save**.
> 
> Press **`` ` ``** to open the console and type `unlock` to restore all unlocks.
> 
> 
> Alternatively, to recover your save after enabling mods:
> 
> The modded mode creates a separate save folder.
>
> You can find `profile1 2 3` and `modded` folders under `{Steam Directory}/userdata/<SteamID>/2868840/` (same for Windows / Linux / macOS).
> 
> Copy `profile1` into `modded`, overwriting the existing folder, to restore your save.
> 
> This is actually the Steam Cloud sync directory, so when you return to Steam, it will prompt a "Cloud Conflict" — choose "Keep Local Files" to finalize the restore.

---


# License

Vibe Coding 参考的原始 Code Base 是 [erasels 的 QuickRestart](https://github.com/erasels/QuickRestart)

原始 repo 是 Unlicense 的，因此本 repo 不添加 License。

The original code base that Vibe Coding referenced is [erasels' QuickRestart](https://github.com/erasels/QuickRestart)

The original repo is Unlicensed, so no license is also applied to this repo.

---


## Modder 指南 / For Modders

想自己做 Mod？推荐参考 Alchyr 的样板，一键构建：

**[ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2)**

一个开箱即用的尖塔2 C# Mod 工程模板，内置构建脚本和基础目录结构，克隆后改改就能上手。

---

Want to make your own mod? Check out the community template:

**[ModTemplate-StS2](https://github.com/Alchyr/ModTemplate-StS2)**

A ready-to-use C# mod project template for Slay the Spire 2. Includes build scripts, and a standard folder layout — clone it and start hacking.
