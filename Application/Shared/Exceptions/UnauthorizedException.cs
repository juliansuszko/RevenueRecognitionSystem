namespace Application.Shared;

public class UnauthorizedException(string msg) : Exception(msg);