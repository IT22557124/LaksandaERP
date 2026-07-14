using Laksanda.API.DTOs.Auth;
using Laksanda.API.Models.Identity;

namespace Laksanda.API.Services;

public interface ITokenService
{
    Task<AuthResponse> CreateTokenAsync(ApplicationUser user);
}