namespace SmartWarehouseManagementSystem.Wrappers
{
    public class DataResponse<T> : SuccessResponse
    {
        public T Data { get; set; }

        public DataResponse(T data, int status = 200, string error = "", string message = "Başarılı")
            : base(status, error, message)
        {
            Data = data;
        }
    }
}
