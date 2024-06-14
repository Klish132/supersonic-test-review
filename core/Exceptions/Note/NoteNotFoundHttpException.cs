using core.Exceptions.Base;

namespace core.Exceptions.Note;

public class NoteNotFoundHttpException(Guid noteId) 
    : NotFoundHttpExceptionBase($"Note with the id \"{noteId}\" not found");