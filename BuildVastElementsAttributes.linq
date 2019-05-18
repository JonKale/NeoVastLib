<Query Kind="Statements">
  <NuGetReference>AWSSDK.Core</NuGetReference>
  <NuGetReference>AWSSDK.S3</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>protobuf-net</NuGetReference>
</Query>

string header = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behaviour and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable UnusedMember.Global
namespace NeoVastLib
{";

var Prefixes = new[] { '?', '/', '*', '+' };
// cardinalities are:
//   ? - zero or one
//   / - one only
//   * - zero to many
//   + - one to many

var Operators = new Dictionary<char, (string, string)> {
    { '/', ("==", "exactly") },
    { '?', ("<=", "at most") },
    { '+', (">=", "at least") }
};

var Groups = new Dictionary<char, (string, string)> {
    { '(', ("^", "contain either") },
    { '[', ("|", "contain at least one of the") },
    { ')', default },
    { ']', default }
};

var elements = new List<string>();
var attributes = new List<string>();
var containers = new Dictionary<string, HashSet<(string, char)>>();
var groups = new Dictionary<string, (char, HashSet<string>)>();

using (var fileReader = File.OpenText(@"C:\src\NeoVastLib\Elements and attributes.txt"))
{
    string line;
    while ((line = fileReader.ReadLine()) != null)
    {
        if (line.Length == 0)
            continue;
        var lineParts = line.Split(new[] { ' ', ',', '(', ')', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
        if (lineParts[0][0] == '/')
            elements.Add(lineParts[0].Substring(1));
        for (var i = 1; i < lineParts.Length; ++i)
            if (Prefixes.Contains(lineParts[i][0]))
            {
                if (!containers.ContainsKey(lineParts[0].Substring(1)))
                    containers[lineParts[0].Substring(1)] = new HashSet<(string, char)>();
                containers[lineParts[0].Substring(1)].Add((lineParts[i].Substring(1), lineParts[i][0]));
            }
            else
                attributes.Add(lineParts[i]);
        // process groupings
        int groupStart = line.IndexOfAny(Groups.Keys.ToArray()),
            groupEnd = line.IndexOfAny(Groups.Keys.ToArray(), groupStart + 1);
        if (groupStart > -1)
        {
            var groupMembers = line.Substring(groupStart, groupEnd - groupStart)
                                   .Split(new[] { ' ', ',', '(', ')', '[', ']', '*', '?', '/', '+' }, StringSplitOptions.RemoveEmptyEntries);
            if (!groups.ContainsKey(lineParts[0].Substring(1)))
                groups.Add(lineParts[0].Substring(1), (line[groupStart], new HashSet<string>()));
            groups[lineParts[0].Substring(1)].Item2.UnionWith(new HashSet<string>(groupMembers));                                   
        }
    }
}

elements.Distinct().OrderBy(e => e).Dump();
containers.Distinct().OrderBy(c => c.Key).Dump();
attributes.Distinct().OrderBy(a => a).Dump();
groups.Dump();

using (var fileWriter = new StreamWriter(File.OpenWrite(@"C:\src\NeoVastLib\NeoVastLib\ContainerValidations.cs")))
{
    fileWriter.Write(header);
    fileWriter.Write(@"
    using System.Linq;
");
    foreach (var pair in containers.Distinct().OrderBy(c => c.Key))
    {
        var name = pair.Key == "VAST" ? "Vast" :
                   pair.Key == "VASTAdTagURI" ? "VastAdTagUri" :
                   pair.Key == "HTMLResource" ? "HtmlResource" :
                   pair.Key == "IFrame" ? "Iframe" :
                   pair.Key;
        var children = pair.Value;
        fileWriter.Write(String.Format(@"
    public sealed partial class {0}Element : VElement
    {{
        protected override bool ValidateContained()
        {{
            if (!this.HasElements)
            {{
                this.validationFailureMessage = ""must have children"";
                return false;
            }}

            if (!this.Elements().All(e => {1}))
            {{
                this.validationFailureMessage = ""children must only be {2}"";
                return false;
            }}
", name,
   String.Join(" || ", children.Select(c => $"e is {c.Item1}Element")),
   String.Join(" or ", children.Select(c => $"{c.Item1}Element"))));

        fileWriter.Write(String.Join(Environment.NewLine, children.Where(c => Operators.Keys.Contains(c.Item2)).Select(c => $@"
            if (!(this.Elements().Count(e => e is {c.Item1}Element) {Operators[c.Item2].Item1} 1))
            {{
                this.validationFailureMessage = ""must have {Operators[c.Item2].Item2} one {c.Item1}Element"";
                return false;
            }}
")));
        fileWriter.Write(String.Format(@"
            return false;
        }}
    }}
"));
    }

    fileWriter.Write(@"}");
}

using (var fileWriter = new StreamWriter(File.OpenWrite(@"C:\src\NeoVastLib\NeoVastLib\GroupValidations.cs")))
{
    fileWriter.Write(header);
    fileWriter.Write(@"
    using System.Linq;
");
    foreach (var group in groups.Distinct().OrderBy(c => c.Key))
    {
        var name = group.Key == "VAST" ? "Vast" :
                   group.Key == "VASTAdTagURI" ? "VastAdTagUri" :
                   group.Key == "HTMLResource" ? "HtmlResource" :
                   group.Key == "IFrame" ? "Iframe" :
                   group.Key;
        
        fileWriter.Write($@"
    public sealed partial class {name}Element : VElement
    {{
        protected override bool ValidateGroups()
        {{
            if ({String.Join($" {Groups[group.Value.Item1].Item1} ", group.Value.Item2.Select(c => $"this.Elements().Any(e => e is {c}Element)"))})
            {{
                return true;
            }}

            this.validationFailureMessage = ""children must contain {Groups[group.Value.Item1].Item2} the {String.Join($" or ", group.Value.Item2.Select(c => $"{c}Element"))} types"";
            return false;
        }}
    }}
");
    }

    fileWriter.Write(@"}");
}

using (var fileWriter = new StreamWriter(File.OpenWrite(@"C:\src\NeoVastLib\NeoVastLib\VastElements.cs")))
{
    fileWriter.Write(header);
    fileWriter.Write(@"
    using System;
");

    foreach (var name in elements.Distinct().OrderBy(e => e))
    {
        fileWriter.Write(String.Format(@"
    public sealed partial class {0}Element : VElement
    {{
        public {0}Element() : base(""{1}"")
        {{
            if (!this.Validate())
            {{
                throw new InvalidVastElementException(""{1}"", this.validationFailureMessage);
            }}
        }}

        public {0}Element(object content) : base(""{1}"", content)
        {{
            if (!this.Validate())
            {{
                throw new InvalidVastElementException(""{1}"", this.validationFailureMessage);
            }}
        }}

        public {0}Element(params object[] content) : base(""{1}"", content)
        {{
            if (!this.Validate())
            {{
                throw new InvalidVastElementException(""{1}"", this.validationFailureMessage);
            }}
        }}
    }}
", name == "VAST" ? "Vast" :
   name == "VASTAdTagURI" ? "VastAdTagUri" :
   name == "HTMLResource" ? "HtmlResource" :
   name == "IFrame" ? "Iframe" :
   name,
   name));
    }

    fileWriter.Write(@"}");    
}

using (var fileWriter = new StreamWriter(File.OpenWrite(@"C:\src\NeoVastLib\NeoVastLib\VastAttributes.cs")))
{
    fileWriter.Write(header);
    fileWriter.Write(@"
    using System;
");

    foreach (var name in attributes.Distinct().OrderBy(a => a))
    {
        fileWriter.Write(String.Format(@"
    public sealed partial class {0}Attribute : VAttribute
    {{
        public {0}Attribute(object value) : base(""{1}"", value)
        {{
            if (!this.Validate())
            {{
                throw new InvalidVastAttributeException(""{1}"", value, this.validationFailureMessage);
            }}
        }}
    }}
", name == "skipoffset" ? "SkipOffset" :
   name == "pxratio" ? "PxRatio" :
   name[0].ToString().ToUpperInvariant() + name.Substring(1), 
   name));
    }

    fileWriter.Write(@"}");
}
