using core.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace core.Extensions;

public static class FormFileExtensions
{
    private static readonly HashSet<string> AllowedExtensions = new() { ".jpg", ".jpg", ".png", ".gif" };
    public static async Task<string> SaveFormImage(this IFormFile file, string savePath)
    {
        var ext = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(ext))
            throw new FileTypeForbiddenHttpException();
        var filename = Path.GetRandomFileName() + ext;
        var path = Path.Combine(savePath, filename);
        await using var fileStream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return path;
    }
}