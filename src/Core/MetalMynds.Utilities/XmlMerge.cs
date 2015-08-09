using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace MetalMynds.Utilities
{
    public class XmlMerge
    {
        private static List<string> _xpathIgnoreRules = new List<string>();

        /// <summary>
        /// This can be used to tell the engine to ignore specific tag/attribute difference
        /// </summary>
        /// <param name="xpathIgnoreRule"></param>
        public static void AddXPathIgnoreRule(string xpathIgnoreRule)
        {
            _xpathIgnoreRules.Add(xpathIgnoreRule);
        }

        public static void AddXPathIgnoreRules(List<String> xpathIgnoreRule)
        {
            _xpathIgnoreRules.AddRange(xpathIgnoreRule);
        }

        public static void ClearIgnoreList()
        {
            _xpathIgnoreRules.Clear();
        }

        public static XmlDocument MergeXml(string originalDocumentPath, string updatedDocumentPath)
        {
            XmlDocument originalDocument = new XmlDocument();
            originalDocument.Load(originalDocumentPath);

            XmlDocument updatedDocument = new XmlDocument();
            updatedDocument.Load(updatedDocumentPath);

            return MergeXml(originalDocument, updatedDocument);

        }

        public static XmlDocument MergeXml(XmlDocument originalDocument, XmlDocument updatedDocument)
        {

            try
            {

                XmlDocument mergedDocument = new XmlDocument();

                mergedDocument.LoadXml(originalDocument.DocumentElement.OuterXml);

                if (mergedDocument.DocumentElement == null || mergedDocument.DocumentElement.ChildNodes.Count == 1)
                    return null;

                if (updatedDocument.DocumentElement == null)
                    return originalDocument;

                ApplyIgnoreRules(mergedDocument, updatedDocument);
                MergeXmlNode(mergedDocument, updatedDocument);

                return mergedDocument;

            }
            catch (Exception ex)
            {
                throw new XmlMergeFailedException(ex);
            }

        }

        private static void MergeXmlNode(XmlNode originalDocumentNode, XmlNode updatedDocumentNode)
        {
            foreach (XmlNode updatedNode in updatedDocumentNode.ChildNodes)
            {
                //Sip node which doesn't contain anything to merge
                if (updatedNode.NodeType == XmlNodeType.XmlDeclaration ||
                    updatedNode.NodeType == XmlNodeType.Comment ||
                    updatedNode.NodeType == XmlNodeType.Document ||
                    updatedNode.NodeType == XmlNodeType.DocumentFragment ||
                    updatedNode.NodeType == XmlNodeType.DocumentType ||
                    updatedNode.NodeType == XmlNodeType.Notation ||
                    updatedNode.NodeType == XmlNodeType.ProcessingInstruction ||
                    updatedNode.NodeType == XmlNodeType.SignificantWhitespace ||
                    updatedNode.NodeType == XmlNodeType.Whitespace) continue;

                string pathInUpdatedDocument = GetXpathNodePath(updatedNode);
                XmlNode originalNode = originalDocumentNode.SelectSingleNode(pathInUpdatedDocument);

                //Add nodes which exists in updated document but not in the original
                if (originalNode == null)
                {
                    pathInUpdatedDocument = GetXpathNodePath(updatedDocumentNode);
                    originalNode = originalDocumentNode.SelectSingleNode(pathInUpdatedDocument);

                    XPathNavigator mergedDocumentNodeNavigatorAdd = originalNode.CreateNavigator();
                    XPathNavigator updatedDocumentNodeNavigatorAdd = updatedNode.CreateNavigator();

                    mergedDocumentNodeNavigatorAdd.AppendChild(updatedDocumentNodeNavigatorAdd);
                    continue;
                }

                XPathNavigator mergedDocumentNodeNavigator = originalNode.CreateNavigator();
                XPathNavigator updatedDocumentNodeNavigator = updatedNode.CreateNavigator();

                if (updatedDocumentNodeNavigator.HasChildren && mergedDocumentNodeNavigator.HasChildren)
                {
                    if (originalNode.FirstChild.NodeType == XmlNodeType.Text && updatedNode.FirstChild.NodeType == XmlNodeType.Text)
                        mergedDocumentNodeNavigator.ReplaceSelf(updatedDocumentNodeNavigator);
                    else
                        MergeXmlNode(originalNode, updatedNode);
                }
                else
                    mergedDocumentNodeNavigator.ReplaceSelf(updatedDocumentNodeNavigator);

            }
        }

        private static XmlDocument ApplyIgnoreRules(XmlDocument mergedDocument, XmlDocument updatedDocument)
        {
            foreach (string xpathIgnoreRule in _xpathIgnoreRules)
            {
                XmlNodeList mergedMatchedNodeList = mergedDocument.SelectNodes(xpathIgnoreRule);
                XmlNodeList updatedMatchedNodeList = updatedDocument.SelectNodes(xpathIgnoreRule);

                if (mergedMatchedNodeList != null && updatedMatchedNodeList != null && mergedMatchedNodeList.Count == updatedMatchedNodeList.Count)
                    for (int matchIndex = 0; matchIndex < mergedMatchedNodeList.Count; matchIndex++)
                    {
                        XPathNavigator mergedMatchedNode = mergedMatchedNodeList[matchIndex].CreateNavigator();
                        XPathNavigator updatedMatchedNode = updatedMatchedNodeList[matchIndex].CreateNavigator();

                        if (mergedMatchedNode.NodeType == XPathNodeType.Attribute)
                            mergedMatchedNode.SetValue(updatedMatchedNode.Value);
                        else
                            mergedMatchedNode.ReplaceSelf(updatedMatchedNode);

                    }
            }

            return mergedDocument;
        }

        /// <summary>
        /// Reads all node attributes and creates XPath query string (string between squere brackets)
        /// e.g. /A/B/C[@id='12' and @name='dummy']
        /// </summary>
        /// <param name="node">Node to get the attributes for</param>
        /// <param name="xpath">Xpath of the current node</param>
        /// <returns></returns>
        public static string GetXpathAttributeQueryString(XmlNode node, string xpath)
        {
            string xpathAttributeQueryString = string.Empty;
            if (node.Attributes != null)
            {
                string[] xpathAttributeQueryStringArray = new string[node.Attributes.Count];
                for (int attributeIndex = 0; attributeIndex < node.Attributes.Count; attributeIndex++)
                {
                    XmlAttribute nodeAttribute = node.Attributes[attributeIndex];
                    xpathAttributeQueryStringArray[attributeIndex] = "@" + nodeAttribute.Name + "='" + nodeAttribute.Value + "'";

                    xpathAttributeQueryString = "[" + string.Join(" and ", xpathAttributeQueryStringArray) + "]";
                }
            }

            return xpathAttributeQueryString;
        }

        public static string GetXpathNodePath(XmlNode node)
        {
            if (node.NodeType != XmlNodeType.Document)
            {
                string currentPath = GetXpathNodePath(node.ParentNode) + "/" + node.Name;
                return currentPath + GetXpathAttributeQueryString(node, currentPath);
            }

            return "/";
        }
    }

    public class XmlMergeFailedException : Exception
    {

        public XmlMergeFailedException(Exception InnerException)
            : base(String.Format("Xml Merge Failed!\nError: {0}\nStack Trace:\n{1}",InnerException.Message, InnerException.StackTrace),InnerException)
        {
        }

    }

}
