using System.Text.RegularExpressions;

namespace NameProject.Server.Utils;

/// <summary>
/// Provides utility methods for sanitizing and validating file paths to ensure security.
/// </summary>
public static class PathSanitization
{
    private static readonly string[] SPatterns =
    [
        @"//",
        @"\.\.",
        @"\$",
        @"{.*}",
        @"%[0-9a-fA-F{2}]",
        "^/auth/user$",
        "/thishouldnotexistandhope"
    ];

    /// <summary>
    /// Determines whether the provided path is malicious based on predefined patterns.
    /// </summary>
    /// <param name="path">The path string to be checked for malicious content.</param>
    /// <returns>Returns true if the path matches any predefined malicious patterns, otherwise false.</returns>
    public static bool IsMalicious(string path)
        => SPatterns.Any(pattern => {

            var matchTimeout = TimeSpan.FromMilliseconds(500);
            return Regex.IsMatch(path, pattern, RegexOptions.IgnoreCase, matchTimeout);
        });
}