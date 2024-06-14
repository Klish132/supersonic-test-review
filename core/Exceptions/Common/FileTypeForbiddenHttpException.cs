using core.Exceptions.Base;

namespace core.Exceptions.Common;

public class FileTypeForbiddenHttpException() : ForbiddenHttpExceptionBase("This file type is not allowed");