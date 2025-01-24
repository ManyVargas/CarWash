using Application.UsesCases.Facturas.GenerarFactura;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class FacturaPdfGenerator
{
    public byte[] GenerarPdfFactura(GenerarFacturaRequest factura)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Configurar la página
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                // Encabezado
                page.Header().Height(50).AlignMiddle().Text("BrillarCar - Factura del consumidor")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                // Contenido principal
                page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                {
                    // Información del cliente
                    column.Item().Text($"Id Empleado: {factura.UsuarioEmail ?? factura.UsuarioTelefono}").Bold();
                    column.Item().Text($"Id Cliente: {factura.ClienteEmail ?? factura.ClienteTelefono}").Bold();
                    column.Item().Text($"Fecha: {DateTime.Now:dd/MM/yyyy}");

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Height(1).Background(Colors.Grey.Medium);
                    });

                    // Tabla de servicios
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4); // Nombre del servicio
                            columns.RelativeColumn(2); // Cantidad
                            columns.RelativeColumn(3); // Precio unitario
                            columns.RelativeColumn(3); // Total
                        });

                        // Encabezados de la tabla
                        table.Header(header =>
                        {
                            header.Cell().Text("Servicio").Bold();
                            header.Cell().Text("Cantidad").Bold();
                            header.Cell().Text("Precio Unitario").Bold();
                            header.Cell().Text("Total").Bold();
                        });

                        // Filas de la tabla
                        foreach (var detalle in factura.Detalles)
                        {
                            table.Cell().Text(detalle.NombreServicio);
                            table.Cell().Text(detalle.Cantidad.ToString());
                            table.Cell().Text($"RD$ {detalle.Precio:F2}");
                            table.Cell().Text($"RD$ {detalle.Total:F2}");
                        }
                    });

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Height(1).Background(Colors.Grey.Medium);
                    });

                    // Total general
                    var totalGeneral = factura.Detalles.Sum(d => d.Total);
                    column.Item().AlignRight().Text($"Total General: RD$ {totalGeneral:F2}")
                        .Bold().FontSize(14);
                });

                // Pie de página
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Gracias por su preferencia").FontSize(10);
                });
            });
        });

        // Exportar a PDF
        return document.GeneratePdf();
    }
}
