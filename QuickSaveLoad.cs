using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;

namespace quickRestart2;

/// <summary>
/// Quick Load (重打): reads the game's own autosave (current_run.save) and reloads the run.
/// The game autosaves when entering a room / combat, so this effectively restarts the current fight.
/// </summary>
public static class QuickSaveLoad
{
    public static void QuickLoad()
    {
        TaskHelper.RunSafely(QuickLoadAsync());
    }

    private static async Task QuickLoadAsync()
    {
        try
        {
            // 1. Read the game's own autosave
            ReadSaveResult<SerializableRun> result = SaveManager.Instance.LoadRunSave();
            if (!result.Success || result.SaveData == null)
            {
                MainFile.Logger.Error($"Quick Load failed: could not read autosave. Status={result.Status}");
                return;
            }

            SerializableRun serializableRun = result.SaveData;
            RunState runState = RunState.FromSerializable(serializableRun);

            // 2. Tear down the current run
            RunManager.Instance.ActionQueueSet.Reset();
            NRunMusicController.Instance?.StopMusic();

            await NGame.Instance!.Transition.FadeOut();

            RunManager.Instance.CleanUp();

            // 3. Reload from save (same flow as NMainMenu.OnContinueButtonPressedAsync)
            RunManager.Instance.SetUpSavedSinglePlayer(runState, serializableRun);
            NGame.Instance.ReactionContainer.InitializeNetworking(new NetSingleplayerGameService());
            await NGame.Instance.LoadRun(runState, serializableRun.PreFinishedRoom);
            await NGame.Instance.Transition.FadeIn();

            MainFile.Logger.Info("Quick Load completed successfully.");
        }
        catch (Exception ex)
        {
            MainFile.Logger.Error($"Quick Load failed: {ex}");
        }
    }
}

