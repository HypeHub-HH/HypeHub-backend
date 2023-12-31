﻿using HypeHubLogic.CQRS.Item.Commands.Delete;
using HypeHubLogic.CQRS.Item.Commands.Post;
using HypeHubLogic.CQRS.Item.Commands.Update;
using HypeHubLogic.CQRS.Item.Queries;
using HypeHubLogic.DTOs.Exception;
using HypeHubLogic.DTOs.Item;
using HypeHubLogic.DTOs.ItemImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HypeHubAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region Endpoint Description
    /// <summary>
    /// Gets a item with given ID.
    /// </summary>
    /// <param name="id">A GUID representing the unique identifier of the item.</param>
    /// <returns>An item</returns>
    /// <remarks>
    ///   Sample request:
    ///
    ///     GET /api/Item/GetItem
    ///     
    /// </remarks>
    /// <response code="200">The item was successfully found and returned.</response>
    /// <response code="404">The item with the specified ID was not found.</response>
    [ProducesResponseType(typeof(ItemGenerallReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(Guid id)
    {
        var result = await _mediator.Send(new GetItemQuery(id));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    ///  Creates a new item.
    /// </summary>
    /// <param name="item">The data for the item to be created.</param>
    /// <returns>
    ///   Newly created item.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to create a new item. You must provide the item data in the request body
    ///   using the JSON format. The request should also include a valid authorization token as this
    ///   endpoint is secured with authentication using the "Authorize" attribute.
    /// </remarks>
    /// <response code="201">The item was successfully created, and its data is returned.</response>
    /// <response code="400">The request was invalid or the item data is incomplete.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(ItemGenerallReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateItem([FromBody] ItemCreateDTO item)
    {
        var result = await _mediator.Send(new CreateItemCommand(item, HttpContext.User.Claims));
        return CreatedAtAction(nameof(GetItem), new { id = result.Id }, result);
    }

    #region Endpoint Description
    /// <summary>
    /// Updates an existing item.
    /// </summary>
    /// <param name="item">The data for the item to be updated.</param>
    /// <returns>
    ///   Updated item.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to update an existing item with the provided data. You should provide
    ///   the item data in the request body using the JSON format. To use this endpoint, ensure that you
    ///   are authenticated with a valid authorization token, as it is secured with the "Authorize" attribute.
    /// </remarks>
    /// <response code="201">The item was successfully updated, and its updated data is returned.</response>
    /// <response code="400">The request was invalid, or the item data is incomplete.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    [ProducesResponseType(typeof(ItemGenerallReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    #endregion
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateItem(Guid id, [FromBody] ItemUpdateDTO item)
    {
        var result = await _mediator.Send(new UpdateItemCommand(id, item, HttpContext.User.Claims));
        return CreatedAtAction(nameof(GetItem), new { id = result.Id }, result);
    }

    #region Endpoint Description
    /// <summary>
    /// Deletes an item with the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the item to be deleted.</param>
    /// <returns>
    ///   Returns an HTTP 204 (No Content) response upon successful deletion of the item.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to delete an item by providing its unique identifier as part of the URL route.
    ///   To use this endpoint, you need to be authenticated with a valid authorization token, as it is secured
    ///   with the "Authorize" attribute. The item will be deleted, and a response with an HTTP 204 (No Content)
    ///   status code will be returned upon successful deletion.
    /// </remarks>
    /// <response code="204">The item was successfully deleted, and no content is returned.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    /// <response code="404">The item with the specified ID was not found and could not be deleted.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteItem(Guid id)
    {
        await _mediator.Send(new DeleteItemCommand(id, HttpContext.User.Claims));
        return NoContent();
    }

    #region Endpoint Description
    /// <summary>
    /// Retrieves a specific image associated with an item.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to be retrieved.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response with the specific image associated with the provided "imageId."
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to retrieve a specific image associated with an item. You should provide
    ///   the "imageId" as part of the URL route to indicate which image you want to retrieve. The response
    ///   will contain the image associated with the specified "imageId."
    /// </remarks>
    /// <response code="200">The specific image associated with the provided "imageId" was successfully retrieved.</response>
    /// <response code="404">The image with the specified "imageId" was not found.</response>
    [ProducesResponseType(typeof(ItemImageReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpGet("Images/{imageId}")]
    public async Task<IActionResult> GetItemImage(Guid imageId)
    {
        var result = await _mediator.Send(new GetItemImageQuery(imageId));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    /// Uploads and associates a new image with a specific item.
    /// </summary>
    /// <param name="item">The data for the image to be uploaded and associated with the item.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response upon successful creation and association of the image with the item.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to upload and associate a new image with a specific item. To use this
    ///   endpoint, provide the "itemId" as part of the URL route, and provide the image data in the request body
    ///   using the JSON format. Ensure you are authenticated with a valid authorization token, as this endpoint
    ///   is secured with the "Authorize" attribute.
    /// </remarks>
    /// <response code="200">The image was successfully created and associated with the specified item.</response>
    /// <response code="400">The request was invalid or the image data is incomplete.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    /// <response code="500">The error occurred on the server side.</response>
    [ProducesResponseType(typeof(ItemImageReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status500InternalServerError)]
    #endregion
    [HttpPost("Images")]
    [Authorize]
    public async Task<IActionResult> CreateImage([FromBody] ItemImageCreateDTO item)
    {
        var result = await _mediator.Send(new CreateItemImageCommand(item, HttpContext.User.Claims));
        return CreatedAtAction(nameof(GetItemImage), new { imageId = result.ItemId, id = result.Id }, result);
    }

    #region Endpoint Description
    /// <summary>
    /// Deletes a specific image associated with an item.
    /// </summary>
    /// <param name="imageId">The unique identifier of the image to be deleted.</param>
    /// <returns>
    ///   Returns an HTTP 204 (No Content) response upon successful deletion of the image.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to delete a specific image associated with an item. To use this endpoint,
    ///   provide the "imageId" as part of the URL route. Ensure you are authenticated with a valid authorization
    ///   token, as this endpoint is secured with the "Authorize" attribute. The image with the specified "imageId"
    ///   will be deleted, and a response with an HTTP 204 (No Content) status code will be returned upon
    ///   successful deletion.
    /// </remarks>
    /// <response code="204">The image with the specified "imageId" was successfully deleted, and no content is returned.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    /// <response code="404">The image with the specified "imageId" was not found.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpDelete("Images/{imageId}")]
    [Authorize]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        await _mediator.Send(new DeleteItemImageCommand(imageId, HttpContext.User.Claims));
        return NoContent();
    }

    #region Endpoint Description
    /// <summary>
    /// Likes or unlikes an item with the specified ID.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to like or unlike.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response upon successful liking or unliking of the item with current item likes.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to like or unlike an item with the specified "id." To use this endpoint, provide
    ///   the "id" as part of the URL route and ensure you are authenticated with a valid authorization token, as this
    ///   endpoint is secured with the "Authorize" attribute. Liking or unliking an item will affect the item's status,
    ///   and a response with an HTTP 200 (OK) status code will be returned upon successful liking or unliking.
    /// </remarks>
    /// <response code="200">The item with the specified "id" was successfully liked or unliked.</response>
    /// <response code="401">User was unauthorized or JWT was invalid</response>
    /// <response code="404">The item with the specified "id" was not found.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionOccuredReadDTO), StatusCodes.Status404NotFound)]
    #endregion
    [HttpPut("{itemId}/like")]
    [Authorize]
    public async Task<IActionResult> LikeOrUnlikeItem(Guid itemId)
    {
        var result = await _mediator.Send(new LikeOrUnlikeItemCommand(itemId, HttpContext.User.Claims));
        return Ok(result);
    }

    #region Endpoint Description
    /// <summary>
    /// Checks if an item is present in any outfit.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to be checked.</param>
    /// <returns>
    ///   Returns an HTTP 200 (OK) response indicating whether the item is present in any outfit.
    /// </returns>
    /// <remarks>
    ///   This endpoint allows you to check if a specific item (identified by "itemId") is present in any outfit.
    ///   After processing the request, a response with an HTTP 200 (OK) status code will be returned, indicating
    ///   whether the item is present in any outfit.
    /// </remarks>
    /// <response code="200">Indicates whether the item is present in any outfit.</response>
    [ProducesResponseType(typeof(bool),StatusCodes.Status200OK)]
    #endregion
    [HttpGet("{itemId}/isInOutfit")]
    public async Task<IActionResult> CheckIfItemIsInAnyOutfit(Guid itemId)
    {
        var result = await _mediator.Send(new GetCheckIfItemIsInAnyOutfitCommand(itemId));
        return Ok(result);
    }
}
