namespace MSIT161_B_PriAPI.DTOs
{
    public class UserUpdateDTO
    {
        public int fUserId { get; set; }
        public string fName { get; set; }
        public string fGender { get; set; }
        public DateTime? fBirthDate { get; set; }
        public string fAddress { get; set; }
        public string fProfileImage { get; set; }
    }
}
