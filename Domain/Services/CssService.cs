namespace Domain.Services;

public class CssService
{
    private readonly IWebHostEnvironment _env;

    public CssService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IEnumerable<string> GetCssFileNames()
    {
        // Adjust the path to point to the Domain project's wwwroot/css folder
        var cssDirectory = Path.Combine(_env.ContentRootPath, "..", "Domain", "wwwroot", "css");
        if (!Directory.Exists(cssDirectory))
        {
            throw new DirectoryNotFoundException($"The directory '{cssDirectory}' does not exist.");
        }
        var Dir = Directory.GetFiles(cssDirectory, "*.min.css")
                        .Select(Path.GetFileNameWithoutExtension);
        return Directory.GetFiles(cssDirectory, "*.min.css")
                        .Select(Path.GetFileNameWithoutExtension);
    }

    public string VerifyCssFilename(string skin)
    {
        skin = $"{skin.ToLower()}.min";
        skin = GetCssFileNames().Contains(skin) ? skin : "spacelab.min";    //  Validate selected skin Default is spacelab
        return $"~/wwwroot/css/{skin}.css";                                 //  Return the full CSS string
    }

    public string VerifyCssStyle(string style)
    {
        return (new[] { "2", "3", "4" }.Contains(style)) ? style : "1";     //  Verify selected style Default is 1
    }

    public string SetCssClass(string cssClass, string style)
    {
        if (style == "1") return $"{cssClass} bg-primary";      //  Style = 1 config
        else if (style == "2") return $"{cssClass} bg-dark";    //  Style = 2 config
        else if (style == "3") return $"{cssClass} bg-light";   //  Style = 3 config
        else return $"{cssClass} bg-body-tertiary";             //  Style = 4 config
    }
}