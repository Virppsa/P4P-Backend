namespace P4P.Models.DTOs.Response;

public class GenericDataResponse<T>
{
    public string Message { get; set; }

    public T? Data { get; set; }
}
