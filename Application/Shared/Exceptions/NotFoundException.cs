namespace Application.Shared;

public class NotFoundException(string msg) : Exception(msg);