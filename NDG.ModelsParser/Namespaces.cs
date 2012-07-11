using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace NDG.XFormsParser
{
    public class Namespaces
    {
        public static readonly XNamespace DefaultNamespace = "http://www.w3.org/2002/xforms";
        public static readonly XNamespace XHtmlNamespace = "http://www.w3.org/1999/xhtml";
        public static readonly XNamespace XmlEventsNamespace = "http://www.w3.org/2001/xml-events";
        public static readonly XNamespace XsdNamespace = "http://www.w3.org/2001/XMLSchema";
        public static readonly XNamespace JavaRosaNamespace = "http://openrosa.org/javarosa";
        public static readonly XNamespace JavaRosaMetaDataNamespace = "http://openrosa.org/xforms/metadata";
    }
}
