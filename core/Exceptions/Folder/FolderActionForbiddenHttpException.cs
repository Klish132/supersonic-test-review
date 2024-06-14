using core.Exceptions.Base;

namespace core.Exceptions.Folder;

public class FolderActionForbiddenHttpException(Guid userId, Guid folderId) 
    : ForbiddenHttpExceptionBase($"User with the id \"{userId}\" isn't authorized to perform this action to folder with the id \"{folderId}\"");