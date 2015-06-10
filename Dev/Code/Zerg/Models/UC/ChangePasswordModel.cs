namespace Zerg.Models.UC
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        public string SecondPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string MobileYzm { get; set; }

        /// <summary>
        /// 码
        /// </summary>
        public string Hidm { get; set; }
    }

    public class ForgetPasswordModel
    {
        public string Phone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Yzm { get; set; }

        /// <summary>
        /// 码
        /// </summary>
        public string Hidm { get; set; }

        public string first_password { get; set; }
        public string second_password { get; set; }
    }
}