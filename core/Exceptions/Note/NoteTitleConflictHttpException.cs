using core.Exceptions.Base;

namespace core.Exceptions.Note;

public class NoteTitleConflictHttpException(string title) 
    : ConflictHttpExceptionBase($"Note with the title \"{title}\" already exists");