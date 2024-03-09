using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace RecruitmentSystem.Business.Services;

public class PdfService
{
    public string GetTextFromPdf(byte[] pdfBytes)
    {
        PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfBytes);
        
        string extractedTexts = "";
        
        foreach (PdfPageBase page in loadedDocument.Pages)
        {
            extractedTexts += page.ExtractText(true);
        }
        
        loadedDocument.Close(true);

        return extractedTexts;
    }


}