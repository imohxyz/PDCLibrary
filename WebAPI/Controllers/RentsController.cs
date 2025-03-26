using Cinema9.DataContracts.Rents.Queries.GetRents;
using Cinema9.DataContracts.Rents.Queries.GetRent;
using Cinema9.DataContracts.Rents.Commands.CreateRent;
using Cinema9.Logics.Common.Exceptions;
using Cinema9.Logics.Rents.Commands.CreateRent;
using Cinema9.Logics.Rents.Queries.GetRents;
using Cinema9.Logics.Rents.Queries.GetRent;
using Cinema9.Logics.Rents.Queries.GetRentsPdf;
using Cinema9.Logics.Rents.Queries.GetRentsExcel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cinema9.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RentsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RentIndex>>> GetRentsAsync()
    {
        var request = new GetRentsQuery();

        return await sender.Send(request);
    }

    [HttpGet("{rentId:guid}")]
    public async Task<ActionResult<RentDetail>> GetRentAsync(Guid rentId)
    {
        try
        {
            var request = new GetRentQuery
            {
                RentId = rentId
            };

            var rent = await sender.Send(request);

            return rent;
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<CreateRentOutput>> CreateRentAsync([FromBody] CreateRentInput input)
    {
        var request = new CreateRentCommand
        {
            Qty = input.Qty,
            MovieId = input.MovieId
            // Don't set Amount here - it will be calculated in the handler
        };

        var output = await sender.Send(request);

        return output;
    }

    [HttpGet("GeneratePdf")]
    public async Task<ActionResult> GeneratePdf()
    {
        var request = new GetRentsPdfQuery();
        var output = await sender.Send(request);

        return new FileContentResult(output.Content, output.ContentType)
        {
            FileDownloadName = output.FileName
        };
    }

    [HttpGet("GenerateExcel")]
    public async Task<ActionResult> GenerateExcel()
    {
        var request = new GetRentsExcelQuery();
        var output = await sender.Send(request);

        return new FileContentResult(output.Content, output.ContentType)
        {
            FileDownloadName = output.FileName
        };
    }

}
