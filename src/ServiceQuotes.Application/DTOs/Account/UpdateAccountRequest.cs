namespace ServiceQuotes.Application.DTOs.Account
{
    public class UpdateAccountRequest
    {
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
