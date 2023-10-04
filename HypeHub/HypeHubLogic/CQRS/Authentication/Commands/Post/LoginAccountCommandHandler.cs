﻿using HypeHubDAL.Exeptions;
using HypeHubDAL.Models;
using HypeHubLogic.DTOs.Logging;
using HypeHubLogic.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace HypeHubLogic.CQRS.Authentication.Commands.Post;

public class LoginAccountCommandHandler : IRequestHandler<LoginAccountCommand, LoggingReadDTO>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public LoginAccountCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService, IConfiguration configuration)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<LoggingReadDTO> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
    {
        var emailOrUserName = request.LoggingCreateDTO.EmailOrUsername;
        var password = request.LoggingCreateDTO.Password;
        var managedUser = await _userManager.FindByEmailAsync(emailOrUserName);
        managedUser ??= await _userManager.FindByNameAsync(emailOrUserName) ?? throw new WrongCredentialsException($"There is no account with the given credentials: {emailOrUserName}.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid) throw new WrongCredentialsException($"Wrong password.");

        var roles = await _userManager.GetRolesAsync(managedUser);

        var accessToken = _tokenService.CreateToken(managedUser, roles);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

        managedUser.RefreshToken = refreshToken;
        managedUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        await _userManager.UpdateAsync(managedUser);

        return new LoggingReadDTO() { Token = accessToken, RefreshToken = refreshToken };
    }
}