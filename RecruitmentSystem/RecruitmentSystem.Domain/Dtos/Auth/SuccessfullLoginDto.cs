namespace RecruitmentSystem.Domain.Dtos.Auth;

public class SuccessfullLoginDto
{
    public SuccessfullLoginDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}