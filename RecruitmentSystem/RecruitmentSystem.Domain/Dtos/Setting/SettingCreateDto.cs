namespace RecruitmentSystem.Domain.Dtos.Setting;

public class SettingCreateDto
{
    public int AiScoreWeight { get; set; }
    public int CompanyScoreWeight { get; set; }
    public int TotalScoreWeight { get; set; }
}