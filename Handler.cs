using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using System.Xml;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotnetCsharp;
public class Handler
{

    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
    //public Response Hello(Request request)
    //{
    //    return new Response("Go Serverless v1.0! Your function executed successfully!", request);
    //}
    public APIGatewayProxyResponse Hello(APIGatewayProxyRequest request)
    {



        Console.WriteLine("this is function abc");
        Guid myuuid = Guid.NewGuid();
        string myuuidAsString = myuuid.ToString();
        Console.WriteLine("Your UUID is: " + myuuidAsString);
        Console.WriteLine("this is testingfsdfsd");

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = "Hello AWS Serverless",
            Headers = new Dictionary<string, string> { { "Content-Type",
 "text/plain" } }
        };

        return response;
    }

    public static void Execute()
    {
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
            Console.WriteLine("strResult");
            Console.WriteLine("strResult-end.");
        }


    }


    private static HttpWebRequest CreateRequest(string url, string action)
    {
        HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
        webrequest.Headers.Add("SOAPAction", action);
        webrequest.ContentType = "text/xml;charset=\"utf-8\"";
        webrequest.Accept = "text/xml";
        webrequest.Method = "POST";
        return webrequest;
    }
 
    private static void Callsoap(XmlDocument xml, HttpWebRequest request)
    {
        using (Stream stream = request.GetRequestStream())
        {
            xml.Save(stream);
        }
    }

    private static XmlDocument CreateSoap()
    {
        XmlDocument soapDoc = new XmlDocument();
        soapDoc.Load(
            @"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                <SOAP-ENV:Body>
                    <NumberToDollars xmlns = ""http://www.dataaccess.com/webservicesserver/"">
                        < dNum > 500 </ dNum >
                </ NumberToDollars >
                </SOAP-ENV:Body>
            </SOAP-ENV:Envelope>");
        return soapDoc;
    }

    //private static XmlDocument CreateSoapEnvelope()
    //{
    //    XmlDocument soapEnvelopeDocument = new XmlDocument();
    //    soapEnvelopeDocument.LoadXml(
    //    @"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" 
    //           xmlns:xsi=""http://www.w3.org/1999/XMLSchema-instance"" 
    //           xmlns:xsd=""http://www.w3.org/1999/XMLSchema"">
    //    <SOAP-ENV:Body>
    //        <HelloWorld xmlns=""http://tempuri.org/"" 
    //            SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
    //            <int1 xsi:type=""xsd:integer"">12</int1>
    //            <int2 xsi:type=""xsd:integer"">32</int2>
    //        </HelloWorld>
    //    </SOAP-ENV:Body>
    //</SOAP-ENV:Envelope>");
    //    return soapEnvelopeDocument;
    //}




}

public class Response
{
  public string Message {get; set;}
  public Request Request {get; set;}

  public Response(string message, Request request)
  {
    Message = message;
    Request = request;
  }
}

public class Request
{
  public string Key1 {get; set;}
  public string Key2 {get; set;}
  public string Key3 {get; set;}
}
