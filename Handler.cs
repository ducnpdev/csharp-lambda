using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using System.Xml;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotnetCsharp;
public class Handler
{

    public APIGatewayProxyResponse Hello(APIGatewayProxyRequest request)
    {
        Console.WriteLine("this is function abc");
        Guid myuuid = Guid.NewGuid();
        string myuuidAsString = myuuid.ToString();
        Console.WriteLine("Your UUID is: " + myuuidAsString);
        //string indiString = "Hello, this is example lambda call soap" + myuuidAsString;

        Execute();

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = "Hello, this is example lambda call soap" + myuuidAsString,
            Headers = new Dictionary<string, string> { { "Content-Type",
 "text/plain" } }
        };


        return response;
    }

    public static void Execute()
    {
        Console.WriteLine("Execute function");

        var _url = "https://www.dataaccess.com/webservicesserver/NumberConversion.wso";

        XmlDocument xml = CreateSoap();
        HttpWebRequest webR = CreateRequest(_url, "");
        Callsoap(xml, webR);
        IAsyncResult result = webR.BeginGetResponse(null, null);

        result.AsyncWaitHandle.WaitOne();

        string strResult;
        using (WebResponse response = webR.EndGetResponse(result))
        {
            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
            {
                strResult = rd.ReadToEnd();
            }
            Console.WriteLine("strResult-start.");
            Console.WriteLine(strResult);
            Console.WriteLine("strResult-end.");
        }
    }

    private static HttpWebRequest CreateRequest(string url, string action)
    {
        Console.WriteLine("CreateRequest function");

        HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
        webrequest.Headers.Add("SOAPAction", action);
        webrequest.ContentType = "text/xml;charset=\"utf-8\"";
        webrequest.Accept = "text/xml";
        webrequest.Method = "POST";
        return webrequest;
    }

    private static void Callsoap(XmlDocument xml, HttpWebRequest request)
    {
        Console.WriteLine("Callsoap function");

        using (Stream stream = request.GetRequestStream())
        {
            xml.Save(stream);
        }
    }

    private static XmlDocument CreateSoap()
    {
        Console.WriteLine("CreateSoap function");

        XmlDocument soapDoc = new XmlDocument();
         soapDoc.LoadXml(
            @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <NumberToDollars xmlns=""http://www.dataaccess.com/webservicesserver/"">
      <dNum>500</dNum>
    </NumberToDollars>
  </soap:Body>
</soap:Envelope>");
        return soapDoc;
    }

}

