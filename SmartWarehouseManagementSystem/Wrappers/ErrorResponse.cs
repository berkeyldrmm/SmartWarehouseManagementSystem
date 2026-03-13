namespace SmartWarehouseManagementSystem.Wrappers
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public ErrorResponse(int status, IEnumerable<string> errors)
        {
            Status = status;
            Errors = errors;
        }
    }
}
