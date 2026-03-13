namespace SmartWarehouseManagementSystem.Wrappers;

public class SuccessResponse
{
    public int Status { get; set; }
    public string Error { get; set; }
    public string Message { get; set; }

    public SuccessResponse(int status = 200, string error = "", string message = "Başarılı")
    {
        Status = status;
        Error = error;
        Message = message;
    }
}
