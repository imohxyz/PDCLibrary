using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Cinema9.Logics.Rents.Queries.GetRentsPdf;

public record GetRentsPdfQuery : IRequest<GetRentsPdfOutput>
{
}

public record GetRentsPdfOutput
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Content { get; init; } = [];
}

public record GetRentsPdfDocument : IDocument
{
    public IList<RentPdfItem> Rents { get; }

    public GetRentsPdfDocument(IList<RentPdfItem> rents)
    {
        Rents = rents;
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
                    text.Span("Rents Count: ").SemiBold();
                    text.Span(Rents.Count.ToString());
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
                header.Cell().Element(CellStyle).Text("Book Title");
                header.Cell().Element(CellStyle).Text("Qty");
                header.Cell().Element(CellStyle).Text("Book Price");
                header.Cell().Element(CellStyle).Text("Amount");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            var index = 0;

            foreach (var rent in Rents)
            {
                index = Rents.IndexOf(rent) + 1;

                table.Cell().Element(CellStyle).Text($"{index}");
                table.Cell().Element(CellStyle).Text(rent.MovieTitle);
                table.Cell().Element(CellStyle).Text($"{rent.Qty:N1}");
                table.Cell().Element(CellStyle).Text($"{rent.MoviePrice:N1}");
                table.Cell().Element(CellStyle).Text($"{rent.Amount:N1}");
                // table.Cell().Element(CellStyle).AlignCenter().Text($"{rent.ReleaseDate:d MMMM yyyy}");

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

public record RentPdfItem
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

public class GetRentsPdfHandler(MyDatabase database) : IRequestHandler<GetRentsPdfQuery, GetRentsPdfOutput>
{
    public async Task<GetRentsPdfOutput> Handle(GetRentsPdfQuery request, CancellationToken cancellationToken)
    {
        var rents = await database.Rents
            .AsNoTracking()
            .Include(r => r.Movie) // Eager load Movie data
            .Select(r => new RentPdfItem
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

        var document = new GetRentsPdfDocument(rents);

        return new GetRentsPdfOutput
        {
            FileName = $"Rents_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf",
            ContentType = "application/pdf",
            Content = document.GeneratePdf()
        };
    }
}
