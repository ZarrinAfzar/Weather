using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Weather.Data.Base
{
    public class SendSms
    {
        public SendSms()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public XElement XmsRequest(int username, string password, string sendfrom, string sendto, string MSG)
        {
            var number = sendto.Split(';', '|', ',').ToList();
            XElement[] a = new XElement[number.Count];
            for (int i = 0; i < number.Count; i++)
            {
                a[i] = new XElement("recipient", number[i].ToString());
            }
            var content = new XElement("xmsrequest",
                new XElement("userid", username),
                new XElement("password", password),
                new XElement("action", "smssend"),
                new XElement("body",
                    new XElement("type", "otm"),
                    new XElement("message",
                        new XAttribute("originator", sendfrom), MSG),
                    a));
            return content;
        }
    }
}
