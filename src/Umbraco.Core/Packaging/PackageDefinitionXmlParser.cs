using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Umbraco.Extensions;

namespace Umbraco.Cms.Core.Packaging
{
    /// <summary>
    /// Converts a <see cref="PackageDefinition"/> to and from XML
    /// </summary>
    public class PackageDefinitionXmlParser
    {
        private static readonly IList<string> s_emptyStringList = new List<string>();
        private static readonly IList<GuidUdi> s_emptyGuidUdiList = new List<GuidUdi>();


        public PackageDefinition ToPackageDefinition(XElement xml)
        {
            if (xml == null)
            {
                return null;
            }

            var retVal = new PackageDefinition
            {
                Id = xml.AttributeValue<int>("id"),
                Name = xml.AttributeValue<string>("name") ?? string.Empty,
                PackagePath = xml.AttributeValue<string>("packagePath") ?? string.Empty,
                PackageId = xml.AttributeValue<Guid>("packageGuid"),
                ContentNodeId = xml.Element("content")?.AttributeValue<string>("nodeId") ?? string.Empty,
                ContentLoadChildNodes = xml.Element("content")?.AttributeValue<bool>("loadChildNodes") ?? false,
                MediaUdis = xml.Element("media")?.Elements("nodeUdi").Select(x => (GuidUdi)UdiParser.Parse(x.Value)).ToList() ?? s_emptyGuidUdiList,
                MediaLoadChildNodes = xml.Element("media")?.AttributeValue<bool>("loadChildNodes") ?? false,
                Macros = xml.Element("macros")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                Templates = xml.Element("templates")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                Stylesheets = xml.Element("stylesheets")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                Scripts = xml.Element("scripts")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                PartialViews = xml.Element("partialViews")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                DocumentTypes = xml.Element("documentTypes")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                MediaTypes = xml.Element("mediaTypes")?.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                Languages = xml.Element("languages")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                DictionaryItems = xml.Element("dictionaryitems")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
                DataTypes = xml.Element("datatypes")?.Value.Split(Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries).ToList() ?? s_emptyStringList,
            };

            return retVal;
        }

        public XElement ToXml(PackageDefinition def)
        {
            var packageXml = new XElement("package",
                new XAttribute("id", def.Id),
                new XAttribute("name", def.Name ?? string.Empty),
                new XAttribute("packagePath", def.PackagePath ?? string.Empty),
                new XAttribute("packageGuid", def.PackageId),
                new XElement("datatypes", string.Join(",", def.DataTypes ?? Array.Empty<string>())),

                new XElement("content",
                    new XAttribute("nodeId", def.ContentNodeId ?? string.Empty),
                    new XAttribute("loadChildNodes", def.ContentLoadChildNodes)),

                new XElement("templates", string.Join(",", def.Templates ?? Array.Empty<string>())),
                new XElement("stylesheets", string.Join(",", def.Stylesheets ?? Array.Empty<string>())),
                new XElement("scripts", string.Join(",", def.Scripts ?? Array.Empty<string>())),
                new XElement("partialViews", string.Join(",", def.PartialViews ?? Array.Empty<string>())),
                new XElement("documentTypes", string.Join(",", def.DocumentTypes ?? Array.Empty<string>())),
                new XElement("mediaTypes", string.Join(",", def.MediaTypes ?? Array.Empty<string>())),
                new XElement("macros", string.Join(",", def.Macros ?? Array.Empty<string>())),
                new XElement("languages", string.Join(",", def.Languages ?? Array.Empty<string>())),
                new XElement("dictionaryitems", string.Join(",", def.DictionaryItems ?? Array.Empty<string>())),

                new XElement(
                    "media",
                    def.MediaUdis.Select(x => (object)new XElement("nodeUdi", x))
                    .Union(new[] { new XAttribute("loadChildNodes", def.MediaLoadChildNodes) }))
                );
            return packageXml;
        }


    }
}
