namespace projectManagementToolWebAPI.Model
{
    public class BaseResponse
    {
        private bool SuccessField = false;

        public bool HasHerror { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusMessage { get; set; }
        public bool Success { get { return SuccessField; } set { SuccessField = value; } }
    }
}