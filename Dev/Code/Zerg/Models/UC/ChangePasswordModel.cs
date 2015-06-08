namespace Zerg.Models.UC
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        public string NewTwoPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Yzm { get; set; }

       
    }
}