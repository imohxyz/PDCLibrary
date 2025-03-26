using OfficeOpenXml.Table;
using OfficeOpenXml;

namespace Cinema9.Logics.Rents.Queries.GetRentsExcel;

public record GetRentsExcelQuery : IRequest<GetRentsExcelOutput>
{
}

public record GetRentsExcelOutput
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Content { get; init; } = [];
}

public record RentExcelItem
{
    public required Guid Id { get; set; }
    public required Guid MovieId { get; set; }
    public required int Qty { get; set; }
    public required decimal Amount { get; set; }
    
    // Optional movie details
    public string? MovieTitle { get; set; }
    public float? MovieRating { get; set; }
    public decimal? MoviePrice { get; set; }
}

public class GetRentsExcelHandler(MyDatabase database) : IRequestHandler<GetRentsExcelQuery, GetRentsExcelOutput>
{
    public async Task<GetRentsExcelOutput> Handle(GetRentsExcelQuery request, CancellationToken cancellationToken)
    {
        var rents = await database.Rents
            .AsNoTracking()
            .Include(r => r.Movie) // Eager load Movie data
            .Select(r => new RentExcelItem
            {
                Id = r.Id,
                MovieId = r.MovieId,
                Qty = r.Qty,
                Amount = r.Amount,
                MovieTitle = r.Movie.Title,
                MovieRating = r.Movie.Rating,
                MoviePrice = r.Movie.Budget
            })
            .ToListAsync(cancellationToken);

        var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Rents");
        var rangeBase = worksheet.Cells.LoadFromCollection(rents, true);

        var tableRents = worksheet.Tables.Add(rangeBase, "TableRents");
        tableRents.ShowHeader = true;
        tableRents.TableStyle = TableStyles.Medium16;
        tableRents.ShowTotal = true;
        tableRents.Columns[nameof(RentExcelItem.Id)].TotalsRowFunction = RowFunctions.Count;
        // tableRents.Columns[nameof(RentExcelItem.Rating)].TotalsRowFunction = RowFunctions.Average;
        tableRents.Columns[nameof(RentExcelItem.Amount)].TotalsRowFunction = RowFunctions.Sum;
        rangeBase.Calculate();
        rangeBase.AutoFitColumns();

        package.Save();

        return new GetRentsExcelOutput
        {
            FileName = $"Rents_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Content = package.GetAsByteArray()
        };
    }
}
