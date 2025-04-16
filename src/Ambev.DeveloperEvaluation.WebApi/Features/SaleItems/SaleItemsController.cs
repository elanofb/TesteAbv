using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItemsBySaleId;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.GetSaleItemsBySaleId;
using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.CreateSaleItems;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sale operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SaleItemController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public SaleItemController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }    

    [HttpGet("{saleId}/items")]
    [ProducesResponseType(typeof(ApiResponseWithData<List<GetSaleItemsBySaleIdResult>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSaleItemsBySaleId(int saleId, CancellationToken ct)
    {
        var request = new GetSaleItemsBySaleIdRequest { SaleId = saleId };
        var command = _mapper.Map<GetSaleItemsBySaleIdCommand>(request);
        var result = await _mediator.Send(command, ct);

        return Ok(new ApiResponseWithData<List<GetSaleItemsBySaleIdResult>>
        {
            Success = true,
            Message = "Items retrieved",
            Data = result
        });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleItemResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Post(CreateSaleItemRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateSaleItemCommand>(request);
        var result = await _mediator.Send(command, ct);
        var response = _mapper.Map<CreateSaleItemResponse>(result);

        return Ok(new ApiResponseWithData<CreateSaleItemResponse>
        {
            Success = true,
            Message = "Item created successfully",
            Data = response
        });
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeleteSaleItemCommand(id); // Use o construtor aqui
        await _mediator.Send(command, ct);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Item deleted successfully"
        });
    }

}
