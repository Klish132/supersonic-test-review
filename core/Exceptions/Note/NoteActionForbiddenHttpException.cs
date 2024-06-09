using core.Exceptions.Base;

namespace core.Exceptions.Note;

public class NoteActionForbiddenHttpException(Guid userId, Guid noteId)
    : ForbiddenHttpExceptionBase($"User with the id \"{userId}\" isn't authorized to perform this action to note with the id \"{noteId}\"");