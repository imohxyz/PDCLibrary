using OfficeOpenXml.Table;
using OfficeOpenXml;

namespace Cinema9.Logics.Movies.Queries.GetMoviesExcel;

public record GetMoviesExcelQuery : IRequest<GetMoviesExcelOutput>
{
}

public record GetMoviesExcelOutput
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Content { get; init; } = [];
}

public record MovieExcelItem
{
    public required string Title { get; set; }
    public required DateTimeOffset ReleaseDate { get; set; }
    public required float Rating { get; set; }
    public required decimal Budget { get; set; }
    public required string CountryName { get; set; }
}

public class GetMoviesExcelHandler(MyDatabase database) : IRequestHandler<GetMoviesExcelQuery, GetMoviesExcelOutput>
{
    public async Task<GetMoviesExcelOutput> Handle(GetMoviesExcelQuery request, CancellationToken cancellationToken)
    {
        var movies = await database.Movies
            .AsNoTracking()
            .Select(x => new MovieExcelItem
            {
                Title = x.Title,
                ReleaseDate = x.ReleaseDate,
                Rating = x.Rating,
                Budget = x.Budget,
                CountryName = x.Country.Name
            })
            .ToListAsync(cancellationToken);

        var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Movies");
        var rangeBase = worksheet.Cells.LoadFromCollection(movies, true);

        var tableMovies = worksheet.Tables.Add(rangeBase, "TableMovies");
        tableMovies.ShowHeader = true;
        tableMovies.TableStyle = TableStyles.Medium16;
        tableMovies.ShowTotal = true;
        tableMovies.Columns[nameof(MovieExcelItem.Title)].TotalsRowFunction = RowFunctions.Count;
        tableMovies.Columns[nameof(MovieExcelItem.Rating)].TotalsRowFunction = RowFunctions.Average;
        tableMovies.Columns[nameof(MovieExcelItem.Budget)].TotalsRowFunction = RowFunctions.Sum;
        rangeBase.Calculate();
        rangeBase.AutoFitColumns();

        package.Save();

        return new GetMoviesExcelOutput
        {
            FileName = $"Movies_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Content = package.GetAsByteArray()
        };
    }
}
