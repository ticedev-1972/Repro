namespace Repro.Api.Function.Models;

public class ResponseJson
{
    public ResponseJson(string name)
    {
        Name = name;
    }

    public string Name { get; }
}