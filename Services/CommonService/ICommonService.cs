namespace chic_lighting.Services.CommonService
{
    public interface ICommonService
    {
        Task<string> Hash(string value);
        Task sendEmail(string subject, string body, string to);
        Task<string> CreateRandomString(int length);
        Task updateBestSeller();
    }
}
