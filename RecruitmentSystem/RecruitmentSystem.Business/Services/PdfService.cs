using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace RecruitmentSystem.Business.Services;

public interface IPdfService
{
    string GetTextFromPdf(byte[] pdfBytes);
}

public class PdfService : IPdfService
{
    public string GetTextFromPdf(byte[] pdfBytes)
    {
        var loadedDocument = new PdfLoadedDocument(pdfBytes);
        
        var extractedTexts = loadedDocument.Pages.Cast<PdfPageBase>().Aggregate("", (current, page) => current + page.ExtractText(true));

        loadedDocument.Close(true);

        return extractedTexts;
    }


}