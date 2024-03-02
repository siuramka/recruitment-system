namespace RecruitmentSystem.Domain.Dtos.Application;

public class ApplicationStepDto
{
    public required string StepType { get; set; }
    public int PositionAscending { get; set; }
    public bool IsCurrentStep { get; set; }
}