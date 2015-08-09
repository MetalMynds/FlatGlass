using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MetalMynds.Utilities
{
    public static class HtmlHelper
    {
        public static String StripTags(String Html)
        {
            //This pattern Matches everything found inside html tags;

            //(.|\n) - > Look for any character or a new line

            // *?  -> 0 or more occurences, and make a non-greedy search meaning

            //That the match will stop at the first available '>' it sees, and not at the last one

            //(if it stopped at the last one we could have overlooked

            //nested HTML tags inside a bigger HTML tag..)

            // Thanks to Oisin and Hugh Brown for helping on this one...

            string pattern = @"<(.|\n)*?>";

            return Regex.Replace(Html, pattern, string.Empty);
        }

        private static NameValueCollection SetAttribute(NameValueCollection Attributes, String Attribute, String Value)
        {
            if (Value != null && Value != String.Empty)
            {
                if (Attributes == null)
                {
                    Attributes = new NameValueCollection();
                }

                Attributes.Add(Attribute, Value);
            }

            return Attributes;
        }

        private static NameValueCollection SetAttribute(NameValueCollection Attributes, String Attribute, int Value)
        {
            if (Value != int.MinValue)
            {
                if (Attributes == null)
                {
                    Attributes = new NameValueCollection();
                }

                Attributes.Add(Attribute, Value.ToString());
            }

            return Attributes;
        }

        // Generic Methods
        // Note: These methods are to be used to add not urenently tags supported
        // IMPORTANT: They are not to be used by the tag methods to 'write' the xml.

        public static void WriteCompleteTag(XmlWriter Writer, String Name, String Value)
        {
            XmlHelper.WriteCompleteNode(Writer, Name.ToLower(), Value, null);
        }

        public static void WriteCompleteTag(XmlWriter Writer, String Name, String Value, NameValueCollection Attributes)
        {
            XmlHelper.WriteCompleteNode(Writer, Name.ToLower(), Value, Attributes);
        }

        public static void WriteStartTag(XmlWriter Writer, String Name)
        {
            XmlHelper.WriteStartNode(Writer, Name.ToLower());
        }

        public static void WriteStartTag(XmlWriter Writer, String Name, NameValueCollection Attributes)
        {
            XmlHelper.WriteStartNode(Writer, Name.ToLower(), Attributes);
        }

        public static void WriteEndTag(XmlWriter Writer, String Name)
        {
            XmlHelper.WriteEndNode(Writer, Name.ToLower());
        }

        public static void WriteRaw(XmlWriter Writer, String RawText)
        {
            XmlHelper.WriteRaw(Writer, RawText);
        }

        public static void WriteLineBreak(XmlWriter Writer)
        {
            XmlHelper.WriteRaw(Writer, "</br>");
        }

        // Specific Tag Methods

        // Tag: Html

        public static void WriteStartHtml(XmlWriter Writer)
        {
            WriteStartHtml(Writer, null);
        }

        public static void WriteStartHtml(XmlWriter Writer, NameValueCollection Attributes)
        {
            XmlHelper.WriteStartNode(Writer, "html", Attributes);
        }

        public static void WriteStartHtml(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "html", Attributes);
        }

        public static void WriteEndHtml(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "html");
        }

        // Tag: Head

        public static void WriteHead(XmlWriter Writer, String Value)
        {
            XmlHelper.WriteCompleteNode(Writer, "head", Value);
        }

        public static void WriteStartHead(XmlWriter Writer)
        {
            XmlHelper.WriteStartNode(Writer, "head");
        }

        public static void WriteEndHead(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "head");
        }

        // Tag: Style

        public static void WriteStyle(XmlWriter Writer, String Value)
        {
            NameValueCollection attributes = null;

            attributes = SetAttribute(attributes, "type", "txt/css");

            XmlHelper.WriteCompleteNode(Writer, "style", Value);
        }

        public static void WriteStyle(XmlWriter Writer, String Value, String Mime)
        {
            NameValueCollection attributes = null;

            attributes = SetAttribute(attributes, "type", Mime);

            XmlHelper.WriteCompleteNode(Writer, "style", Value);
        }

        public static void WriteStartStyle(XmlWriter Writer, String Value, String Mime)
        {
            NameValueCollection attributes = null;

            attributes = SetAttribute(attributes, "type", Mime);

            XmlHelper.WriteStartNode(Writer, "style", attributes);
        }

        public static void WriteEndStyle(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "style");
        }

        // Tag: Anchor

        public static void WriteAnchor(XmlWriter Writer, String Identifier)
        {
            WriteAnchor(Writer, null, Identifier, null, null, null, null);
        }

        public static void WriteAnchor(XmlWriter Writer, Uri Url)
        {
            WriteAnchor(Writer, null, null, Url, null, null, null);
        }

        public static void WriteAnchor(XmlWriter Writer, String Class, Uri Url)
        {
            WriteAnchor(Writer, Class, null, Url, null, null, null);
        }

        public static void WriteAnchor(XmlWriter Writer, String Class, Uri Url, String Target)
        {
            WriteAnchor(Writer, Class, null, Url, Target, null, null);
        }

        public static void WriteAnchor(XmlWriter Writer, String Class, String Identifier, Uri Url, String Target, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);
            Attributes = SetAttribute(Attributes, "href", Url.ToString());
            Attributes = SetAttribute(Attributes, "target", Target);

            //XmlHelper.WriteStartNode(Writer, "html", Attributes);

            XmlHelper.WriteCompleteNode(Writer, "a", Value, Attributes);
        }

        // Tag: Bold

        public static void WriteBold(XmlWriter Writer, String Value)
        {
            WriteBold(Writer, null, null, Value);
        }

        public static void WriteBold(XmlWriter Writer, String Class, String Identifier, String Value)
        {
            WriteBold(Writer, Class, Identifier, Value, null);
        }

        public static void WriteBold(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "b", Value, Attributes);
        }

        // Tag: Strong

        public static void WriteStrong(XmlWriter Writer, String Value)
        {
            WriteStrong(Writer, null, null, Value);
        }

        public static void WriteStrong(XmlWriter Writer, String Class, String Identifier, String Value)
        {
            WriteStrong(Writer, Class, Identifier, Value, null);
        }

        public static void WriteStrong(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "strong", Value, Attributes);
        }

        public static void WriteBig(XmlWriter Writer, String Value)
        {
            WriteCompleteTag(Writer, "big", Value);
        }

        public static void WriteBig(XmlWriter Writer, String Value, NameValueCollection Attributes)
        {
            XmlHelper.WriteCompleteNode(Writer, "big", Value, Attributes);
        }

        public static void WriteStartBody(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartBody(Writer, Class, Identifier, null);
        }

        public static void WriteStartBody(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "body", Attributes);
        }

        public static void WriteEndBody(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "body");
        }

        public static void WriteBody(XmlWriter Writer, String Class, String Identifier, String Value)
        {
            WriteBody(Writer, Class, Identifier, Value, null);
        }

        public static void WriteBody(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "body", Value);
        }

        public static void WriteEmphasis(XmlWriter Writer, String Value)
        {
            XmlHelper.WriteCompleteNode(Writer, "emphasis", Value);
        }

        public static void WriteHeading(XmlWriter Writer, int Number, String Value)
        {
            XmlHelper.WriteCompleteNode(Writer, String.Format("h{0}", Number), Value);
        }

        public static void WriteItalic(XmlWriter Writer, String Value)
        {
            XmlHelper.WriteCompleteNode(Writer, "i", Value);
        }

        public static void WriteImage(XmlWriter Writer, String Class, String Identifier, String Source, String Value, String Alternative)
        {
            WriteImage(Writer, Class, Identifier, Source, Value, Alternative, null);
        }

        public static void WriteImage(XmlWriter Writer, String Class, String Identifier, String Source, String Value, String Alternative, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            Attributes = SetAttribute(Attributes, "src", Source);
            Attributes = SetAttribute(Attributes, "alt", Alternative);

            XmlHelper.WriteCompleteNode(Writer, "img", String.Empty, Attributes);
        }

        public static void WriteImage(XmlWriter Writer, String Source, String Value, int Width, int Height, String Alternative)
        {
            WriteImage(Writer, Source, Value, Width, Height, Alternative, null);
        }

        public static void WriteImage(XmlWriter Writer, String Source, String Value, int Width, int Height, String Alternative, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "src", Source);
            Attributes = SetAttribute(Attributes, "alt", Alternative);
            Attributes = SetAttribute(Attributes, "width", Width.ToString());
            Attributes = SetAttribute(Attributes, "height", Height.ToString());

            XmlHelper.WriteCompleteNode(Writer, "img", Value, Attributes);
        }

        public static void WriteStartOrderedList(XmlWriter Writer)
        {
            WriteStartOrderedList(Writer, String.Empty, String.Empty, null);
        }

        public static void WriteStartOrderedList(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartOrderedList(Writer, Class, Identifier, null);
        }

        public static void WriteStartOrderedList(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "ol", Attributes);
        }

        public static void WriteEndOrderedList(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "ol");
        }

        public static void WriteStartUnorderedList(XmlWriter Writer)
        {
            WriteStartUnorderedList(Writer, String.Empty, String.Empty, null);
        }

        public static void WriteStartUnorderedList(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartUnorderedList(Writer, Class, Identifier, null);
        }

        public static void WriteStartUnorderedList(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "ul", Attributes);
        }

        public static void WriteEndUnorderedList(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "ul");
        }

        public static void WriteListItem(XmlWriter Writer, String Class, String Identifier, String Value)
        {
            WriteListItem(Writer, Class, Identifier, Value, null);
        }

        public static void WriteListItem(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "li", Value, Attributes);
        }

        public static void WriteStartListItem(XmlWriter Writer)
        {
            WriteStartListItem(Writer, String.Empty, String.Empty, null);
        }

        public static void WriteStartListItem(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartListItem(Writer, Class, Identifier, null);
        }

        public static void WriteStartListItem(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "li", Attributes);
        }

        public static void WriteEndListItem(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "li");
        }

        public static void WriteDivider(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "div", Value, Attributes);
        }

        public static void WriteStartDivider(XmlWriter Writer)
        {
            WriteStartDivider(Writer, String.Empty, String.Empty, null);
        }

        public static void WriteStartDivider(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartDivider(Writer, Class, Identifier, null);
        }

        public static void WriteStartDivider(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "div", Attributes);
        }

        public static void WriteEndDivider(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "div");
        }

        public static void WriteTable(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "table", Value, Attributes);
        }

        public static void WriteStartTable(XmlWriter Writer)
        {
            WriteStartTable(Writer, String.Empty, String.Empty, null);
        }

        public static void WriteStartTable(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartTable(Writer, Class, Identifier, null);
        }

        public static void WriteStartTable(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "table", Attributes);
        }

        public static void WriteEndTable(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "table");
        }

        public static void WriteTableHeader(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "th", Value, Attributes);
        }

        public static void WriteStartTableHeader(XmlWriter Writer)
        {
            WriteStartTableHeader(Writer, String.Empty, String.Empty, null);
        }

        public static void WriteStartTableHeader(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartTableHeader(Writer, Class, Identifier, null);
        }

        public static void WriteStartTableHeader(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "th", Attributes);
        }

        public static void WriteEndTableHeader(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "th");
        }

        public static void WriteTableRow(XmlWriter Writer, String Class, String Identifier, String Value, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteCompleteNode(Writer, "tr", Value, Attributes);
        }

        public static void WriteStartTableRow(XmlWriter Writer)
        {
            WriteStartTableRow(Writer, String.Empty, String.Empty, null);
        }

        public static void WriteStartTableRow(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartTableRow(Writer, Class, Identifier, null);
        }

        public static void WriteStartTableRow(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "tr", Attributes);
        }

        public static void WriteEndTableRow(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "tr");
        }

        public static void WriteTableCell(XmlWriter Writer, String Value)
        {
            WriteTableCell(Writer, null, null, Value, int.MinValue, int.MinValue, null);
        }

        public static void WriteTableCell(XmlWriter Writer, String Class, String Identifier, String Value)
        {
            WriteTableCell(Writer, Class, Identifier, Value, int.MinValue, int.MinValue, null);
        }

        public static void WriteTableCell(XmlWriter Writer, int ColumnSpan, int RowSpan, String Value)
        {
            WriteTableCell(Writer, null, null, Value, ColumnSpan, RowSpan, null);
        }

        public static void WriteTableCell(XmlWriter Writer, String Class, String Identifier, String Value, int ColumnSpan, int RowSpan, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);
            Attributes = SetAttribute(Attributes, "colspan", ColumnSpan.ToString());
            Attributes = SetAttribute(Attributes, "rowspan", RowSpan.ToString());

            XmlHelper.WriteCompleteNode(Writer, "td", Value, Attributes);
        }

        public static void WriteStartTableCell(XmlWriter Writer)
        {
            WriteStartTableCell(Writer, String.Empty, String.Empty, int.MinValue, int.MinValue, null);
        }

        public static void WriteStartTableCell(XmlWriter Writer, int ColumnSpan, int RowSpan)
        {
            WriteStartTableCell(Writer, String.Empty, String.Empty, ColumnSpan, RowSpan, null);
        }

        public static void WriteStartTableCell(XmlWriter Writer, String Class, String Identifier, int ColumnSpan, int RowSpan)
        {
            WriteStartTableCell(Writer, Class, Identifier, ColumnSpan, RowSpan);
        }

        public static void WriteStartTableCell(XmlWriter Writer, String Class, String Identifier, int ColumnSpan, int RowSpan, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);
            Attributes = SetAttribute(Attributes, "colspan", ColumnSpan.ToString());
            Attributes = SetAttribute(Attributes, "rowspan", RowSpan.ToString());

            XmlHelper.WriteStartNode(Writer, "td", Attributes);
        }

        public static void WriteEndTableCell(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "tr");
        }

        public static void WriteStartTableBody(XmlWriter Writer)
        {
            WriteStartTableBody(Writer, null, null, null);
        }

        public static void WriteStartTableBody(XmlWriter Writer, String Class, String Identifier)
        {
            WriteStartTableBody(Writer, Class, Identifier, null);
        }

        public static void WriteStartTableBody(XmlWriter Writer, String Class, String Identifier, NameValueCollection Attributes)
        {
            Attributes = SetAttribute(Attributes, "class", Class);
            Attributes = SetAttribute(Attributes, "id", Identifier);

            XmlHelper.WriteStartNode(Writer, "td", Attributes);
        }

        public static void WriteEndTableBody(XmlWriter Writer)
        {
            XmlHelper.WriteEndNode(Writer, "tr");
        }
    }
}