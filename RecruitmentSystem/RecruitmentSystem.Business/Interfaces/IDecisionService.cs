using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Interfaces;

public interface IDecisionService
{
    Task CreateDecision(Decision decision);
    Task Remove(Decision decision);
}