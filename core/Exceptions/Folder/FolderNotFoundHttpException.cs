using core.Exceptions.Base;

namespace core.Exceptions.Folder;

public class FolderNotFoundHttpException(Guid folderId) 
    : NotFoundHttpExceptionBase($"Folder with the id \"{folderId}\" not found");