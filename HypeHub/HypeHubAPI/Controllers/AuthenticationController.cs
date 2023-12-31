﻿using HypeHubDAL.Models;
using HypeHubLogic.CQRS.Authentication.Commands.Post;
using HypeHubLogic.CQRS.Authentication.Queries;
using HypeHubLogic.DTOs.Exception;
using HypeHubLogic.DTOs.Logging;
using HypeHubLogic.DTOs.Registration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HypeHubAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    #region Endpoint Description
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="registrationCreateDTO">Data for creating a new user account.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response upon successful registration of a new user account.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to register a new user account by providing the necessary registration data
    ///   in the request body using the JSON format. After successful registration, a response with an HTTP 200
    ///   (OK) status code will be returned, and the newly registered user's data may be included in the response.
    /// </remarks>
    /// <response code="200">The user account was successfully registered.</response>
    /// <response code="400">The request was invalid or the registration data is incomplete.</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(RegistrationReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegistrationCreateDTO registrationCreateDTO)
    {
        var result = await _mediator.Send(new RegisterAccountCommand(registrationCreateDTO));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    /// Logs in a user to their account.
    /// </summary>
    /// <param name="loggingCreateDTO">Data for user login.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response upon successful user login, along with user authentication data.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows a registered user to log in to their account by providing their login credentials
    ///   in the request body using the JSON format. After successful login, a response with an HTTP 200 (OK)
    ///   status code will be returned with user authentication data, such as tokens or user information.
    /// </remarks>
    /// <response code="200">The user was successfully logged in, and user authentication data is returned with JWT and refresh token.</response>
    /// <response code="400">The login request was invalid or the login data is incorrect.</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(RegistrationReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoggingCreateDTO loggingCreateDTO)
    {
        var result = await _mediator.Send(new LoginAccountCommand(loggingCreateDTO));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    /// Refreshes a user's authentication token using a refresh token.
    /// </summary>
    /// <param name="token">A refresh token for refreshing the user's authentication token.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response upon successful token refresh, along with the new access token.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows a user to refresh their authentication token by providing a valid refresh token
    ///   in the request body using the JSON format. After a successful token refresh, a response with an HTTP 200 (OK)
    ///   status code will be returned, and it will include the new access token for the user.
    /// </remarks>
    /// <response code="200">The user's authentication token was successfully refreshed, and new token is returned.</response>
    /// <response code="400">The token refresh request was invalid, or the refresh token is expired or incorrect.</response>
    [ProducesResponseType(typeof(Token), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    #endregion
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] Token token)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(token));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    /// Revokes a user's authentication token, effectively logging the user out.
    /// </summary>
    /// <param name="username">The username of the user whose token should be revoked.</param>
    /// <returns>
    ///   Returns an HTTP 204 (No Content) response upon successful token revocation.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to revoke a user's authentication token, effectively logging the user out. To use this
    ///   endpoint, provide the "username" as part of the URL route and ensure that you are authenticated with a valid
    ///   authorization token, as this endpoint is secured with the "Authorize" attribute. After successful token
    ///   revocation, a response with an HTTP 204 (No Content) status code will be returned.
    /// </remarks>
    /// <response code="204">The user's authentication token was successfully revoked, and no content is returned.</response>
    /// <response code="400">Wrong user credentials.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    #endregion
    [HttpPost("RevokeToken/{username}")]
    [Authorize]
    public async Task<IActionResult> RevokeToken(string username)
    {
        await _mediator.Send(new RevokeTokenCommand(username));
        return NoContent();
    }

    #region Endpoint Description
    /// <summary>
    /// Gets current account.
    /// </summary>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response after successful user identification with user data.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows to get information about the current user based on the JWT token. After successful identification, a response with an HTTP 200 (OK)
    ///   status code will be returned with user data.
    /// </remarks>
    /// <response code="200">The user was successfully identified, and user data is returned.</response>
    /// <response code="400">User or JWT was invalid.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(RegistrationReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpGet("GetCurrentAccount")]
    [Authorize]
    public async Task<IActionResult> GetCurrentAccount()
    {
        var token = Request.Headers.Authorization;
        var result = await _mediator.Send(new GetCurrentAccountQuery(token));
        return Ok(result);
    }
}