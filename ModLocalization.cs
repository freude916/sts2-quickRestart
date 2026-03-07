using System.Reflection;
using System.Text.Json;
using Godot;
using MegaCrit.Sts2.Core.Localization;
using FileAccess = Godot.FileAccess;

namespace quickRestart2;

public static class ModLocalization
{
    private const string DefaultLanguage = "en";
    private static readonly object SyncRoot = new();
    private static Dictionary<string, string> _translations = new(StringComparer.OrdinalIgnoreCase);
    private static string? _loadedLanguage;

    public static string Get(string key, string fallback)
    {
        EnsureLoaded();
        return _translations.GetValueOrDefault(key) ?? fallback;
    }

    private static void EnsureLoaded()
    {
        string language = ResolveLanguage();

        lock (SyncRoot)
        {
            if (string.Equals(_loadedLanguage, language, StringComparison.OrdinalIgnoreCase))
                return;

            _translations = LoadTranslations(language);
            _loadedLanguage = language;
        }
    }

    private static Dictionary<string, string> LoadTranslations(string language)
    {
        foreach (string candidate in GetLanguageCandidates(language))
        {
            Dictionary<string, string>? translations = TryLoadEmbedded(candidate);
            if (translations is { Count: > 0 })
            {
                MainFile.Logger.Info($"Loaded embedded localization for '{candidate}'.");
                return translations;
            }

            string path = $"res://{MainFile.ModId}/localization/{candidate}.json";
            translations = TryLoadFromGodotPath(path);
            if (translations is { Count: > 0 })
            {
                MainFile.Logger.Info($"Loaded localization file: {path}");
                return translations;
            }
        }

        MainFile.Logger.Info($"No localization file found for '{language}', falling back to defaults.");
        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    private static Dictionary<string, string>? TryLoadEmbedded(string language)
    {
        string resourceName = $"{MainFile.ModId}.localization.{language}.json";
        using Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        if (stream == null)
            return null;

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(stream);
        }
        catch (JsonException ex)
        {
            MainFile.Logger.Error($"Failed to parse embedded localization '{resourceName}': {ex.Message}");
            return null;
        }
    }

    private static Dictionary<string, string>? TryLoadFromGodotPath(string path)
    {
        if (!FileAccess.FileExists(path))
            return null;

        using FileAccess? file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            MainFile.Logger.Error($"Failed to open localization file: {path}");
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(file.GetAsText());
        }
        catch (JsonException ex)
        {
            MainFile.Logger.Error($"Failed to parse localization file '{path}': {ex.Message}");
            return null;
        }
    }

    private static string ResolveLanguage()
    {
        string? language = LocManager.Instance.Language;
        if (string.IsNullOrWhiteSpace(language))
            language = TranslationServer.GetLocale();

        return NormalizeLanguageCode(language);
    }

    private static IEnumerable<string> GetLanguageCandidates(string language)
    {
        HashSet<string> candidates = new(StringComparer.OrdinalIgnoreCase);

        foreach (string candidate in ExpandLanguageCandidates(language))
        {
            if (candidates.Add(candidate))
                yield return candidate;
        }

        if (candidates.Add(DefaultLanguage))
            yield return DefaultLanguage;
    }

    private static IEnumerable<string> ExpandLanguageCandidates(string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
            yield break;

        string normalized = NormalizeLanguageCode(language);
        yield return normalized;

        int separatorIndex = normalized.IndexOf('_');
        if (separatorIndex > 0)
            yield return normalized[..separatorIndex];

        if (normalized.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
            yield return "zhs";

        if (normalized.StartsWith("en", StringComparison.OrdinalIgnoreCase))
            yield return "en";
    }

    private static string NormalizeLanguageCode(string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return DefaultLanguage;

        string normalized = language.Trim().Replace('-', '_').ToLowerInvariant();
        return normalized switch
        {
            "zh_cn" or "zh_hans" or "zh_sg" or "zh" => "zhs",
            "en_us" or "en_gb" => "en",
            _ => normalized,
        };
    }
}
