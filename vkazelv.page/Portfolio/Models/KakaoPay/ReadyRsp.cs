namespace Portfolio.Models.KakaoPay
{
    public class ReadyRsp
    {
        public string tid { get; set; }
        public string next_redirect_app_url { get; set; }
        public string next_redirect_mobile_url { get; set; }
        public string next_redirect_pc_url { get; set; }
        public string android_app_scheme { get; set; }
        public string ios_app_scheme { get; set; }
        public DateTime created_at { get; set; }
    }
}
