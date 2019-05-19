namespace NeoVastLibTestHarness
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using NeoVastLib;

    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();

            foreach (var test in program.GetTests())
            {
                try
                {
                    test.Invoke(program, null);
                    Console.WriteLine($"Test {test.Name} passed");
                }
                catch (TargetInvocationException ex) when (ex.InnerException is InvalidVastException vex)
                {
                    Console.WriteLine($"Test {test.Name} failed because of {vex.Name}: {vex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            Console.ReadLine();
        }

        IEnumerable<MethodInfo> GetTests() =>
            from mi in typeof(Program).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance |
                                                  BindingFlags.DeclaredOnly)
            from attr in Attribute.GetCustomAttributes(mi).OfType<TestAttribute>()
            select mi;

        [Test]
        private void MinimalNoAdResponse()
        {
            var vastDoc = new XDocument(
                new VastElement(
                    new VersionAttribute("4.1"),
                    new ErrorElement(
                        new XCData("http://adserver.com/noad.gif"))));
            Console.WriteLine(vastDoc.ToString());
            var doc = new XmlDocument();
            doc.Schemas.Add(XmlSchema.Read(File.OpenRead(@"C:\\src\\NeoVastLib\\vast_4.1.xsd"),
                (sender, eventArgs) => Console.WriteLine(eventArgs.Message)));
            doc.LoadXml(vastDoc.ToString());
        }

        [Test]
        private void MoreComplexNoAdResponse()
        {
            var vastDoc = new XDocument(
                new VastElement(
                    new VersionAttribute("4.1"),
                    new ErrorElement(
                        new XCData("http://adserver.com/noad.gif")),
                    new ErrorElement(
                        new XCData("http://www.example.com"))));
            Console.WriteLine(vastDoc.ToString());
            var doc = new XmlDocument();
            doc.Schemas.Add(XmlSchema.Read(File.OpenRead(@"C:\\src\\NeoVastLib\\vast_4.1.xsd"),
                (sender, eventArgs) => Console.WriteLine(eventArgs.Message)));
            doc.LoadXml(vastDoc.ToString());
        }

        //[Test]
        private void MinimalAdResponse()
        {
            var vastDoc = new XDocument(
                new VastElement(
                    new VersionAttribute("4.1"),
                    new AdElement()));
            Console.WriteLine(vastDoc.ToString());
            var doc = new XmlDocument();
            doc.Schemas.Add(XmlSchema.Read(File.OpenRead(@"C:\\src\\NeoVastLib\\vast_4.1.xsd"),
                (sender, eventArgs) => Console.WriteLine(eventArgs.Message)));
            doc.LoadXml(vastDoc.ToString());
        }

        [Test]
        private void MinimalCompleteResponse()
        {
            var vastDoc = new XDocument(
                new VastElement( new VersionAttribute("4.0"),
                    new AdElement( new IdAttribute("2shsta4"),
                        new InLineElement(
                            new AdSystemElement(new VersionAttribute("1.0"),
                                "The TradeDesk"),
                            new AdTitleElement(
                                "2shsta4"),
                            new AdServingIdElement(
                                Guid.NewGuid().ToString("d")),
                            new ImpressionElement(
                                new XCData("about:blank")),
                            new CreativesElement(
                                new CreativeElement(
                                    new LinearElement(
                                        new DurationElement(
                                            "00:00:15"),
                                        new MediaFilesElement(
                                            new MediaFileElement(
                                                new DeliveryAttribute("progressive"),
                                                new HeightAttribute(720),
                                                new WidthAttribute(1280),
                                                new BitrateAttribute(3000),
                                                new TypeAttribute("video/mp4"),
                                                new XCData("https://s3.eu-west-2.amazonaws.com/creative-delivery/prod/2019/AHG/IPG+(Interpublic+Group)/Cadreon/April/Accenture/Spain/15.04+Approved+creative/Accenture_NAN_Talent_ES.mp4")
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    )
                )
            );
            Console.WriteLine(vastDoc.ToString());
            var doc = new XmlDocument();
            doc.Schemas.Add(XmlSchema.Read(File.OpenRead(@"C:\\src\\NeoVastLib\\vast_4.1.xsd"),
                (sender, eventArgs) => Console.WriteLine(eventArgs.Message)));
            doc.LoadXml(vastDoc.ToString());
        }
    }

    internal class TestAttribute : Attribute
    {
    }
}
