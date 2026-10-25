// Exceptions/AppExceptions.cs
namespace dotnet_api.Exceptions;
public class NotFoundException(string msg) : Exception(msg);
public class ConflictException(string msg) : Exception(msg);
public class BusinessRuleException(string msg) : Exception(msg);
