namespace RecruitmentSystem.Domain.Models;

public class Setting
{
    public Guid Id { get; set; }
    public SettingsName Name { get; set; }
    public string Value { get; set; } = string.Empty;
}