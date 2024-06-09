using core.Exceptions.Base;

namespace core.Exceptions.Folder;

public class FolderTitleConflictHttpException(string title) 
    : ConflictHttpExceptionBase($"Folder with the title \"{title}\" already exists");