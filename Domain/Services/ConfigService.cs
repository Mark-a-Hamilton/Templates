using Microsoft.Extensions.Configuration;

namespace Domain.Services;

/// <summary>
/// CssService does all the actions relating to the configuration of CSS files
/// </summary>
public class ConfigService
{
    private readonly IWebHostEnvironment _env;

    public ConfigService(IWebHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    /// GetCssFilenames returns a list of filenames from the Domain project wwwroot/css folder which are minified.
    /// </summary>
    /// <returns>A list of filenames without the .min.css extenion so "spacelab.min.css" would be returned as "spacelab"</returns>
    /// <exception cref="DirectoryNotFoundException">Is thrown in the event the Domain/wwwroot/css Directory is not found</exception>
    public IEnumerable<string> GetCssFileNames()
    {
        // Adjust the path to point to the Domain project's wwwroot/css folder
        var cssDirectory = Path.Combine(_env.ContentRootPath, "..", "Domain", "wwwroot", "css");
        if (!Directory.Exists(cssDirectory))
        {
            throw new DirectoryNotFoundException($"The directory '{cssDirectory}' does not exist.");
        }
        return Directory.GetFiles(cssDirectory, "*.min.css")
                        .Select(file => Path.GetFileNameWithoutExtension(file)
                        .Replace(".min", ""));
    }

    public string GetSiteCss()
    {
        // Adjust the path to point to the Domain project's wwwroot/css folder
        var cssDir = Path.Combine(_env.ContentRootPath, "..", "Domain", "wwwroot", "css");
        if (!Directory.Exists(cssDir))
        {
            throw new DirectoryNotFoundException($"The directory '{cssDir}' does not exist.");
        }

        // Get the full path of the site.css file
        var siteCssPath = Directory.GetFiles(cssDir, "site.css").FirstOrDefault();

        if (siteCssPath == null)
        {
            throw new FileNotFoundException($"The file 'site.css' does not exist in the specified directory : {cssDir}.");
        }

        return siteCssPath;
    }

    public string GetSiteJs()
    {
        // Adjust the path to point to the Domain project's wwwroot/js folder
        var jsDir = Path.Combine(_env.ContentRootPath, "..", "Domain", "wwwroot", "js");
        if (!Directory.Exists(jsDir))
        {
            throw new DirectoryNotFoundException($"The directory '{jsDir}' does not exist.");
        }

        // Get the full path of the site.js file
        var siteJsPath = Directory.GetFiles(jsDir, "site.js").FirstOrDefault();

        if (siteJsPath == null)
        {
            throw new FileNotFoundException($"The file 'site.js' does not exist in the specified directory : {jsDir}.");
        }

        return siteJsPath;
    }

    /// <summary>
    /// Selects the CSS File if it exists in the Domain/wwwroot/css directory otherwise will be spacelab which is the default
    /// </summary>
    /// <param name="appsettings">The configuration settings from the appsettings.json file</param>
    /// <returns>The full path of the CSS file to use</returns>
    public string VerifyCssFilename(IConfiguration appsettings)
    {
        var skin = appsettings.GetValue<string>("Layout:Skin")?.ToLower();  // Read the skin value from appsettings.json 
        skin = GetCssFileNames().Contains(skin) ? skin : "spacelab";        // Verifies the selected skin Default is spacelab
        return $"~/wwwroot/css/{skin}.min.css";                             // Return the full CSS string
    }

    /// <summary>
    /// Selects the Style if valid else returns the default Style of 4
    /// </summary>
    /// <param name="appsettings">The configuration settings from the appsettings.json file</param>
    /// <returns>Ethier the selecred style if valid or "1" as defeault style.</returns>
    public string VerifyCssStyle(IConfiguration appsettings)
    {
        var style = appsettings.GetValue<string>("Layout:Style");           // Read the Style value from appsettings.json
        return (new[] { "1", "2", "3" }.Contains(style)) ? style : "4";     // Verify selected style Default is 1
    }

    /// <summary>
    /// Defines the Class used based on the Style and cssClass 
    /// </summary>
    /// <param name="cssClass"></param>
    /// <param name="style"></param>
    /// <returns>the full class path</returns>
    public string SetCssClass(string cssClass, string style)
    {
        if (style == "1") return $"{cssClass} bg-primary";      //  Style = 1 config
        else if (style == "2") return $"{cssClass} bg-dark";    //  Style = 2 config
        else if (style == "3") return $"{cssClass} bg-light";   //  Style = 3 config
        else return $"{cssClass} bg-body-tertiary";             //  Style = 4 config
    }
}