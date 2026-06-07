using Godot;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace quickRestart2;

/// <summary>
/// Harmony patch that adds a localized retry button to the pause menu.
/// The button reloads the game's autosave, effectively restarting the current fight.
/// </summary>
[HarmonyPatch(typeof(NPauseMenu), nameof(NPauseMenu._Ready))]
public static class PauseMenuPatch
{
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    public static void Postfix(NPauseMenu __instance)
    {
        // Only show in singleplayer
        if (RunManager.Instance.NetService.Type != NetGameType.Singleplayer)
            return;

        try
        {
            Control buttonContainer = __instance._buttonContainer;
            NPauseMenuButton saveAndQuitButton = __instance._saveAndQuitButton;

            // Duplicate an existing button as our localized retry button
            NPauseMenuButton retryButton = (NPauseMenuButton)saveAndQuitButton.Duplicate();
            retryButton.Name = "Retry";
            MakeButtonVisualsUnique(retryButton);
            retryButton.GetNode<MegaLabel>("Label")
                .SetTextAutoSize(GetRetryButtonText());

            // Insert before GiveUp
            NPauseMenuButton giveUpButton = __instance._giveUpButton;
            int giveUpIndex = giveUpButton.GetIndex();
            buttonContainer.AddChild(retryButton);
            buttonContainer.MoveChild(retryButton, giveUpIndex);

            // Connect signal
            retryButton.Connect(
                NClickableControl.SignalName.Released,
                Callable.From<NButton>(btn => OnRetryPressed(btn, buttonContainer))
            );

            // Disable if no autosave exists
            if (!SaveManager.Instance.HasRunSave)
                retryButton.Disable();

            // Rebuild focus neighbors for controller/keyboard nav
            RebuildFocusNeighbors(buttonContainer);

            MainFile.Logger.Info("Retry button added to pause menu.");
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error($"Failed to add Retry button: {ex}");
        }
    }

    private static void OnRetryPressed(NButton btn, Control buttonContainer)
    {
        // Immediately disable the retry button to prevent rapid clicks
        btn.Disable();
        QuickSaveLoad.QuickLoad();
    }

    private static void MakeButtonVisualsUnique(NPauseMenuButton retryButton)
    {
        TextureRect buttonImage = retryButton.GetNode<TextureRect>("ButtonImage");
        if (buttonImage.Material is not ShaderMaterial material)
            return;

        // Godot duplicates nodes but can still share resource instances like materials.
        // Pause menu hover state is driven by shader params, so give the retry button
        // its own material to avoid mirroring SaveAndQuit's highlight.
        buttonImage.Material = (ShaderMaterial)material.Duplicate();
    }

    private static string GetRetryButtonText()
    {
        string locale = TranslationServer.GetLocale();
        return locale.StartsWith("zh", StringComparison.OrdinalIgnoreCase) ? "重试" : "Retry";
    }

    private static void RebuildFocusNeighbors(Control buttonContainer)
    {
        List<NPauseMenuButton> buttons = [];
        foreach (Node child in buttonContainer.GetChildren())
        {
            if (child is NPauseMenuButton { Visible: true } btn)
                buttons.Add(btn);
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            NPauseMenuButton btn = buttons[i];
            btn.FocusNeighborLeft = btn.GetPath();
            btn.FocusNeighborRight = btn.GetPath();
            btn.FocusNeighborTop = i > 0 ? buttons[i - 1].GetPath() : btn.GetPath();
            btn.FocusNeighborBottom = i < buttons.Count - 1 ? buttons[i + 1].GetPath() : btn.GetPath();
        }
    }
}

/// <summary>
/// Harmony prefix that disables all pause menu buttons (including the Retry button)
/// before Save &amp; Quit executes, preventing any further button interactions.
/// </summary>
[HarmonyPatch(typeof(NPauseMenu), "OnSaveAndQuitButtonPressed")]
public static class SaveAndQuitDisablePatch
{
    [HarmonyPrefix]
    public static void Prefix(NPauseMenu __instance)
    {
        // Disable all buttons in the container, including our Retry button
        Control buttonContainer = __instance._buttonContainer;
        foreach (Node child in buttonContainer.GetChildren())
        {
            if (child is NPauseMenuButton btn)
                btn.Disable();
        }
    }
}
