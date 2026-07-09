using Rotativa.AspNetCore;
namespace Ts.Application.Helpers
{
    public static class PdfHelper<T>
    {
        public static ViewAsPdf GeneratePdf(T ViewModel, string ViewName, string FileName = null, string Orientation = null, bool AddFooter = false)
        {
            var pdf = new ViewAsPdf(ViewName, ViewModel)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };

            if (AddFooter)
                pdf.CustomSwitches += "--print-media-type --footer-right \"  Generated on: " +
                                     DateTime.UtcNow.AddHours(5.5).Date.ToString("dd-MM-yyyy") +
                                     "  Page: [page]/[toPage]\"" +
                                     " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\"";

            if (!string.IsNullOrEmpty(FileName))
                pdf.FileName = $"{FileName}.pdf";

            pdf.PageOrientation = !string.IsNullOrEmpty(Orientation) && Orientation == "Landscape"
                ? Rotativa.AspNetCore.Options.Orientation.Landscape
                : Rotativa.AspNetCore.Options.Orientation.Portrait;

            return pdf;
        }
    }
}
