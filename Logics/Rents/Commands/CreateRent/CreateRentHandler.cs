using System.ComponentModel.DataAnnotations;
using System.Xml;
using Cinema9.DataContracts.Rents.Commands.CreateRent;
using Cinema9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cinema9.Logics.Rents.Commands.CreateRent;

public record CreateRentCommand : CreateRentInput, IRequest<CreateRentOutput>
{
}

// public class CreateRentHandler(MyDatabase database) : IRequestHandler<CreateRentCommand, CreateRentOutput>
// {
//     public async Task<CreateRentOutput> Handle(CreateRentCommand request, CancellationToken cancellationToken)
//     {
//         // First, get the movie to access its budget
//         var movie = await database.Movies
//             .Where(m => m.Id == request.MovieId)
//             .FirstOrDefaultAsync(cancellationToken)
//             ?? throw new NotFoundException($"Movie with ID {request.MovieId} not found");
        
//         var rent = new Rent
//         {
//             Qty = request.Qty,
//             Amount = movie.Budget * request.Qty,
//             MovieId = request.MovieId
//         };

//         await database.Rents.AddAsync(rent, cancellationToken);
//         await database.SaveChangesAsync(cancellationToken);

//         var output = new CreateRentOutput
//         {
//             RentId = rent.Id
//         };

//         return output;
//     }
// }

public class CreateRentHandler(
    MyDatabase database,
    ILogger<CreateRentHandler> logger,
    CalculatorService calculatorService) 
    : IRequestHandler<CreateRentCommand, CreateRentOutput>
{
    public async Task<CreateRentOutput> Handle(
        CreateRentCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Get movie data
        logger.LogInformation("Fetching movie with ID {MovieId}", request.MovieId);
        var movie = await database.Movies
            .FirstOrDefaultAsync(m => m.Id == request.MovieId, cancellationToken)
            ?? throw new NotFoundException($"Movie with ID {request.MovieId} not found");

        // 2. Validate input
        if (request.Qty <= 0)
        {
            logger.LogWarning("Invalid quantity: {Quantity}", request.Qty);
            throw new ValidationException("Quantity must be greater than zero");
        }

        if (movie.Budget <= 0)
        {
            logger.LogWarning("Invalid movie budget: {Budget}", movie.Budget);
            throw new ValidationException("Movie budget must be positive");
        }

        // 3. Prepare values for calculation (convert to cents)
        int budgetInCents = (int)movie.Budget;
        int qty = request.Qty;
        logger.LogDebug("Calculation parameters - Budget: {BudgetCents} cents, Qty: {Quantity}", 
            budgetInCents, qty);
        Console.WriteLine($"Konten: {budgetInCents} x {qty}");

        // 4. Call calculator service
        int calculatedAmount;
        try
        {
            logger.LogInformation("Calling SOAP calculator for {BudgetCents} * {Quantity}", 
                budgetInCents, qty);
            
            calculatedAmount = await calculatorService.MultiplyAsync(budgetInCents, qty);
            
            logger.LogDebug("Raw result from SOAP service: {CalculatedAmount}", calculatedAmount);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "SOAP service request failed");
            throw new ApplicationException("Calculator service unavailable");
        }
        catch (XmlException ex)
        {
            logger.LogError(ex, "Failed to parse SOAP response");
            throw new ApplicationException("Invalid response from calculator service");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in calculator service");
            throw;
        }

        // 5. Convert back to dollars
        decimal finalAmount = calculatedAmount;
        logger.LogInformation("Final calculated amount: {FinalAmount}", finalAmount);

        // 6. Create rent record
        var rent = new Rent
        {
            Qty = qty,
            Amount = finalAmount,
            MovieId = request.MovieId
        };

        // 7. Save to database
        try
        {
            await database.Rents.AddAsync(rent, cancellationToken);
            await database.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Created new rent with ID {RentId}", rent.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save rent record");
            throw;
        }

        return new CreateRentOutput { RentId = rent.Id };
    }
}
