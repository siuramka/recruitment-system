namespace RecruitmentSystem.Domain.Dtos.Setting;

public class SettingDto
{
    public Guid Id { get; set; }
    public int AiScoreWeight { get; set; }
    public int CompanyScoreWeight { get; set; }
    public int TotalScoreWeight { get; set; }
}