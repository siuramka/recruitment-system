using System.Net.Mime;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class ScoreService
{
    
    private IConfiguration _configuration;
    private RecruitmentDbContext _db;
    private PdfService _pdfService;
    private IMapper _mapper;

    public ScoreService(IConfiguration configuration, RecruitmentDbContext db, PdfService pdfService, IMapper mapper)
    {
        _configuration = configuration;
        _db = db;
        _pdfService = pdfService;
        _mapper = mapper;
    }

    public async Task UpdateScreeningAiScore(Application application, int score)
    {
        var cv = await _db.Cvs.FirstOrDefaultAsync(cv => cv.ApplicationId == application.Id);
        
        var evaluation = new Evaluation()
        {
            AiScore = score,
            Content = string.Empty,
        };

        _db.Evaluations.Add(evaluation);
        await _db.SaveChangesAsync();

        cv.EvaluationId = evaluation.Id;

        await _db.SaveChangesAsync();
    }
}