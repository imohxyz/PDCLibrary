using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cinema9.Logics.Movies.Queries.GetMoviesPdf;

public record GetMoviesPdfQuery : IRequest<GetMoviesPdfOutput>
{
}

public record GetMoviesPdfOutput
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Content { get; init; } = [];
}

public record GetMoviesPdfDocument : IDocument
{
    public IList<MoviePdfItem> Movies { get; }

    public GetMoviesPdfDocument(IList<MoviePdfItem> movies)
    {
        Movies = movies;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item()
                    .Text("Cinema9")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Movies Count: ").SemiBold();
                    text.Span(Movies.Count.ToString());
                });
            });

            row.ConstantItem(100).Height(50).Placeholder(".NET Core");
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Element(ComposeTable);
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("#");
                header.Cell().Element(CellStyle).Text("Title");
                header.Cell().Element(CellStyle).AlignCenter().Text("Release Date");
                header.Cell().Element(CellStyle).AlignCenter().Text("Rating");
                header.Cell().Element(CellStyle).Text("Country");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            var index = 0;

            foreach (var movie in Movies)
            {
                index = Movies.IndexOf(movie) + 1;

                table.Cell().Element(CellStyle).Text($"{index}");
                table.Cell().Element(CellStyle).Text(movie.Title);
                table.Cell().Element(CellStyle).AlignCenter().Text($"{movie.ReleaseDate:d MMMM yyyy}");
                table.Cell().Element(CellStyle).AlignCenter().Text($"{movie.Rating:N1}");
                table.Cell().Element(CellStyle).Text(movie.CountryName);

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(x =>
        {
            x.Span("Blazor Server with .NET 9");
        });
    }
}

public record MoviePdfItem
{
    public required string Title { get; set; }
    public required DateTimeOffset ReleaseDate { get; set; }
    public required float Rating { get; set; }
    public required decimal Budget { get; set; }
    public required string CountryName { get; set; }
}

public class GetMoviesPdfHandler(MyDatabase database) : IRequestHandler<GetMoviesPdfQuery, GetMoviesPdfOutput>
{
    public async Task<GetMoviesPdfOutput> Handle(GetMoviesPdfQuery request, CancellationToken cancellationToken)
    {
        var movies = await database.Movies
            .AsNoTracking()
            .Select(x => new MoviePdfItem
            {
                Title = x.Title,
                ReleaseDate = x.ReleaseDate,
                Rating = x.Rating,
                Budget = x.Budget,
                CountryName = x.Country.Name
            })
            .ToListAsync(cancellationToken);

        var document = new GetMoviesPdfDocument(movies);

        return new GetMoviesPdfOutput
        {
            FileName = $"Movies_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf",
            ContentType = "application/pdf",
            Content = document.GeneratePdf()
        };
    }
}
