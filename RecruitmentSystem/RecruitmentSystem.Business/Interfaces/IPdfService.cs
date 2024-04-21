namespace RecruitmentSystem.Business.Interfaces;

public interface IPdfService
{
    string GetTextFromPdf(byte[] pdfBytes);
}