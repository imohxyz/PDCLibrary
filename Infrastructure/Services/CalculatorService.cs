// Cinema9.Infrastructure/Services/CalculatorService.cs
using System.Text;
using System.Xml;

public class CalculatorService
{
    private readonly HttpClient _httpClient;

    public CalculatorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Updated CalculatorService with better error handling
    public async Task<int> MultiplyAsync(int a, int b)
    {
        var soapEnvelope = $"""
            <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope" xmlns:tem="http://tempuri.org/">
            <soap:Header/>
            <soap:Body>
                <tem:Multiply>
                    <tem:intA>{a}</tem:intA>
                    <tem:intB>{b}</tem:intB>
                </tem:Multiply>
            </soap:Body>
            </soap:Envelope>
            """;
        Console.WriteLine($"Content: {soapEnvelope}");
        using var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
        // content.Headers.Add("SOAPAction", "http://tempuri.org/Multiply");

        var response = await _httpClient.PostAsync("calculator.asmx", content);
        Console.WriteLine($"Response: {response}");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"SOAP request failed with status {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(responseContent);
        
        return int.Parse(xmlDoc.InnerText);
    }
}