using System.Runtime.Serialization;

namespace RecruitmentSystem.Domain.Models;

public enum SettingsName
{
    [EnumMember(Value = "AiScoreWeight")]
    AiScoreWeight,
    [EnumMember(Value = "CompanyScoreWeight")]
    CompanyScoreWeight,
    [EnumMember(Value = "TotalScoreWeight")]
    TotalScoreWeight,
}