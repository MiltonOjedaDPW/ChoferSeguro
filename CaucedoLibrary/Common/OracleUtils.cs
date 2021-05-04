using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.Base.Common
{
    public class CallOraReportParams
    {
        public string rptRoute { get; set; }
        public string format { get; set; }
        public string reportServiceEndPoint { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public List<OraReportParam> parameters = null;
        public string AsXML
        {
            get
            {
                return
            $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:v2='http://xmlns.oracle.com/oxp/service/v2'>
               <soapenv:Header/>
               <soapenv:Body>
                  <v2:runReport>
                     <v2:reportRequest>
                        <v2:attributeFormat>{format}</v2:attributeFormat>
                        <v2:attributeLocale>en-US</v2:attributeLocale>
                        <v2:attributeTemplate></v2:attributeTemplate>
                        <v2:reportAbsolutePath>{rptRoute}</v2:reportAbsolutePath>
                        <v2:reportOutputPath></v2:reportOutputPath>
                        {ParamsXML}
                     </v2:reportRequest>
                     <v2:userID>{userName}</v2:userID>
                     <v2:password>{password}</v2:password>
                  </v2:runReport>
               </soapenv:Body>
            </soapenv:Envelope>";
            }
        }
        public string ParamsXML
        {
            get
            {
                var result = new StringBuilder();
                if (parameters != null && parameters.Count > 0)
                {
                    result.AppendLine(@"<v2:parameterNameValues>
                                     <v2:listOfParamNameValues>");
                    foreach (var p in parameters)
                    {
                        result.AppendLine(p.ToXml());
                    }
                    result.Append(@"</v2:listOfParamNameValues>
                                    </v2:parameterNameValues>");
                }
                return result.ToString();
            }
        }
    }
    public class OraReportParam
    {
        public string UIType { get; set; }
        public string DataType { get; set; }
        public string DateFormatString { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string DefaultValue { get; set; }
        public string FieldSize { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ToXml()
        {
            return $@"<v2:item>
                     <v2:UIType>{UIType}</v2:UIType>
                     <v2:dataType>{DataType}</v2:dataType>
                     <v2:dateFormatString>{DateFormatString}</v2:dateFormatString>                     
                     <v2:defaultValue>{DefaultValue}</v2:defaultValue>
                     <v2:fieldSize>{FieldSize}</v2:fieldSize>
                     <v2:label>{Label}</v2:label>
                     <v2:name>{Name}</v2:name>                     
                     <v2:values>                        
                        <v2:item>{Value}</v2:item>
                     </v2:values>
                  </v2:item>";
        }
    }

    public static class OracleUtils
    {
        public static string CallOraReportXML(CallOraReportParams parameters)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //(SecurityProtocolType)3072;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(parameters.reportServiceEndPoint);
            
            request.Headers.Add("SOAPAction:");
            request.ContentType = "text/xml"; //"application/soap+xml";// // or application/soap+xml for SOAP 1.2
            request.Method = "POST";
            request.KeepAlive = true;

            //In case you have a proxy to resolve the server name also add these lines
            /*var proxyServer = new WebProxy("XX.XX.XX.XX", 1234);
            proxyServer.Credentials = CredentialCache.DefaultCredentials; // or username + password
            request.Proxy = proxyServer;*/
            
            byte[] byteArray = Encoding.UTF8.GetBytes(parameters.AsXML);
            request.ContentLength = byteArray.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            var text = reader.ReadToEnd();
            text = text.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?><soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><soapenv:Body><runReportResponse xmlns=\"http://xmlns.oracle.com/oxp/service/v2\"><runReportReturn><metaDataList xsi:nil=\"true\"/><reportBytes>", "");
            text = text.Replace("</reportBytes><reportContentType>text/plain;charset=UTF-8</reportContentType><reportFileID xsi:nil=\"true\"/><reportLocale xsi:nil=\"true\"/></runReportReturn></runReportResponse></soapenv:Body></soapenv:Envelope>", "");
            var result = Base64Decode(text);
            // cleanp
            reader.Close();
            requestStream.Close();
            responseStream.Close();
            response.Close();
            return result;
        }


        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
