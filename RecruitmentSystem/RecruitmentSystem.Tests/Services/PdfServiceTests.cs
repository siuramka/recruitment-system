using System.Text;
using FluentAssertions;
using RecruitmentSystem.Business.Services;

namespace RecruitmentSystem.Tests.Services;

public class PdfServiceTests
{
    [Test]
    public async Task GetTextFromPdf_ShouldGetText_WhenTextIsValid()
    {
        const string minimalString = "JVBERi0xLjAKMSAwIG9iajw8L1R5cGUvQ2F0YWxvZy9QYWdlcyAyIDAgUj4+ZW5kb2JqIDIgMCBvYmo8PC9UeXBlL1BhZ2VzL0tpZHNbMyAwIFJdL0NvdW50IDE+PmVuZG9iaiAzIDAgb2JqPDwvVHlwZS9QYWdlL01lZGlhQm94WzAgMCAzIDNdPj5lbmRvYmoKeHJlZgowIDQKMDAwMDAwMDAwMCA2NTUzNSBmCjAwMDAwMDAwMTAgMDAwMDAgbgowMDAwMDAwMDUzIDAwMDAwIG4KMDAwMDAwMDEwMiAwMDAwMCBuCnRyYWlsZXI8PC9TaXplIDQvUm9vdCAxIDAgUj4+CnN0YXJ0eHJlZgoxNDkKJUVPRg==";
        var bytes = Convert.FromBase64String(minimalString);
        var pdfService = new PdfService();
        var result = pdfService.GetTextFromPdf(bytes);
        result.Should().Be("\r\n\r\n");
    }
}