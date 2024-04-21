using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Assessment;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class AssessmentService : IAssessmentService
{
    private RecruitmentDbContext _db;
    
    public AssessmentService(RecruitmentDbContext db)
    {
        _db = db;
    }

    public async Task<Assessment> CreateAssessment(Guid applicationId, AssessmentCreateDto assessmentCreateDto)
    {
        var newAssessment = new Assessment
        {
            ApplicationId = applicationId,
            Content = assessmentCreateDto.Content,
            StartTime = DateTime.Now.ToUniversalTime(),
            EndTime = assessmentCreateDto.EndTime.ToUniversalTime()
        };
        
        await _db.Assessments.AddAsync(newAssessment);
        await _db.SaveChangesAsync();

        return newAssessment;
    }
}