namespace Domain.Services;

/// <summary>
/// ConfigService handles the configuration of CSS and JS files.
/// </summary>
public class ConfigService(IWebHostEnvironment env)
{
    private readonly IWebHostEnvironment _env = env;
    private static readonly string[] sourceArray = { "1", "2", "3" };

    /// <summary>
    /// Returns a list of filenames from the Domain project's wwwroot/css folder that are minified.
    /// </summary>
    /// <returns>A list of filenames without the .min.css extension.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown if the Domain/wwwroot/css directory is not found.</exception>
    public IEnumerable<string> GetFileNames()
    {
        var cssDirectory = Path.Combine(_env.ContentRootPath, "../Domain/wwwroot/css");
        if (!Directory.Exists(cssDirectory))
        {
            throw new DirectoryNotFoundException($"The directory '{cssDirectory}' does not exist.");
        }
        return Directory.GetFiles(cssDirectory, "*.min.css")
                        .Select(file => Path.GetFileNameWithoutExtension(file).Replace(".min", ""));
    }

    /// <summary>
    /// Gets the jQuery file from the Domain project's wwwroot/js directory.
    /// </summary>
    /// <returns>The full path including the jquery.min.js file.</returns>
    /// <exception cref="DirectoryNotFoundException">Thrown if the js directory is not found in Domain.</exception>
    /// <exception cref="FileNotFoundException">Thrown if the jquery.min.js file is not found in the js directory.</exception>
    public string GetJQuery()
    {
        var jDir = Path.Combine(_env.ContentRootPath, "../Domain/wwwroot/lib/jquery/dist");
        if (!Directory.Exists(jDir))
        {
            throw new DirectoryNotFoundException($"The directory '{jDir}' does not exist.");
        }

        return Directory.GetFiles(jDir, "jquery.min.js").FirstOrDefault()
            ?? throw new FileNotFoundException($"The file 'jquery.min.js' does not exist in the specified directory: {jDir}.");
    }

    /// <summary>
    /// Selects the CSS file if it exists in the Domain/wwwroot/css directory, otherwise defaults to spacelab.
    /// </summary>
    /// <param name="appsettings">The configuration settings from the appsettings.json file.</param>
    /// <returns>The full path of the CSS file to use.</returns>
    public string VerifyFilename(IConfiguration appsettings)
    {
        var skin = appsettings.GetValue<string>("Layout:Skin")?.ToLower();
        skin = GetFileNames().Contains(skin) ? skin : "spacelab";
        return $"~/css/{skin}.min.css";
    }

    /// <summary>
    /// Selects the style if valid, else returns the default style of 4.
    /// </summary>
    /// <param name="appsettings">The configuration settings from the appsettings.json file.</param>
    /// <returns>Either the selected style if valid or "4" as the default style.</returns>
    public static string? VerifyStyle(IConfiguration appsettings)
    {
        var style = appsettings.GetValue<string>("Layout:Style");
        return sourceArray.Contains(style) ? style : "4";
    }

    /// <summary>
    /// Defines the class used based on the style and cssClass.
    /// </summary>
    /// <param name="cssClass"></param>
    /// <param name="style"></param>
    /// <returns>The full class path.</returns>
    public static string SetClass(string cssClass, string style)
    {
        return style switch
        {
            "1" => $"{cssClass} bg-primary",
            "2" => $"{cssClass} bg-dark",
            "3" => $"{cssClass} bg-light",
            _ => $"{cssClass} bg-body-tertiary"
        };
    }
}