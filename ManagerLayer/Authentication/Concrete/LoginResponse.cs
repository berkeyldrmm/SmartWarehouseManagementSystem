namespace ManagerLayer.Authentication.Concrete;

public record LoginResponse(Guid userId, string token);
