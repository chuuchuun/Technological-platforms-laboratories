using System;
using System.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

using System.Xml.Linq;


namespace Lab_9
{
    class Program
    {
        private static List<Car>? _myCars = new()
    {
        new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
        new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
        new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
        new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
        new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
        new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
        new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
        new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
        new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
    };
        private const string FileName = "CarsCollection.xml";

        private static void Main(string[] args)
        {
            TaskOne();

            Serialize(FileName);
            foreach (Car car in _myCars)
            {
                Console.WriteLine($"Year: {car.Year}, Motor Model: {car.Motor.Model}, Horsepower: {car.Motor.Horsepower}, Displacement: {car.Motor.Displacement}");
            }
            _myCars = Deserialize(FileName);
            /*
            Console.WriteLine();
            Console.WriteLine();
            foreach (var x in _myCars)
            {
                Console.WriteLine($"Year: {x.Year}, Motor Model: {x.Motor.Model}, Horsepower: {x.Motor.Horsepower}, Displacement: {x.Motor.Displacement}");
            }
            */
            XPath(FileName);
            LinqSerialization();
            MyCarsToXhtmlTable();
            ModifyCarsCollectionXml();
        }

        private static void TaskOne()
        {
            var projectedCars = _myCars
                .Where(c => c.Model == "A6")
                .Select(c => new
                {
                    engineType = c.Motor.Model == "TDI" ? "diesel" : "petrol",
                    hppl = (double)c.Motor.Horsepower / c.Motor.Displacement
                });
            var groupedCars = projectedCars.GroupBy(c => c.engineType).OrderBy(g => g.Key);

            foreach (var group in groupedCars)
            {
                Console.WriteLine($"{group.Key}: {string.Join(", ", group.Select(c => c.hppl))} (avg: {group.Average(c => c.hppl)})");
            }
        }

        private static void Serialize(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            var currentDirectory = Directory.GetCurrentDirectory();
            Path.Combine(currentDirectory, fileName);
            using var writer = new StreamWriter(fileName);
            serializer.Serialize(writer, _myCars);
        }

        private static List<Car>? Deserialize(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            using Stream reader = new FileStream(fileName, FileMode.Open);
            return serializer.Deserialize(reader) as List<Car>;
        }

        private static void XPath(string fileName)
        {
            XElement rootNode = XElement.Load(fileName);
            double avgHP = (double) rootNode.XPathEvaluate("sum(//car/engine[@Model!=\"TDI\"]/Horsepower) div count(//car/engine[@Model!=\"TDI\"]/Horsepower)");
            Console.WriteLine($"Przeciętna moc samochodów o silnikach innych niż TDI: {avgHP}");

            IEnumerable<XElement> models = rootNode.XPathSelectElements("//car/engine[@Model and not(@Model = preceding::car/engine/@Model)]");

            foreach (var model in models)
            {
                Console.WriteLine(model.Attribute("Model")?.Value);
            }
        }

        private static void LinqSerialization()
        {
            IEnumerable<XElement>? nodes = _myCars?
                .Select(n =>
                    new XElement("car",
                        new XElement("Model", n.Model),
                        new XElement("engine",
                            new XAttribute("Model", n.Motor.Model),
                            new XElement("Horsepower", n.Motor.Horsepower),
                            new XElement("Displacement", n.Motor.Displacement)),
                        new XElement("Year", n.Year)));
            XElement rootNode = new XElement("cars", nodes);
            rootNode.Save("CarsFromLinq.xml");
        }

        private static void MyCarsToXhtmlTable()
        {
            IEnumerable<XElement>? rows = _myCars?.Select(car => new XElement("tr",
                new XAttribute("style", "border: 2px solid black"),
                new XElement("td", new XAttribute("style", "border: 2px double black"), car.Model),
                new XElement("td", new XAttribute("style", "border: 2px double black"), car.Motor.Model),
                new XElement("td", new XAttribute("style", "border: 2px double black"), car.Motor.Displacement),
                new XElement("td", new XAttribute("style", "border: 2px double black"), car.Motor.Horsepower),
                new XElement("td", new XAttribute("style", "border: 2px double black"), car.Year)));

            XElement table = new XElement("table",
                new XAttribute("style", "border: 2px double black"),
                rows
            );

            XElement template = XElement.Load("template.html");
            var body = template.Element("{http://www.w3.org/1999/xhtml}body");
            body?.Add(table);
            template.Save("templateDone.html");
        }

        private static void ModifyCarsCollectionXml()
        {
            XDocument doc = XDocument.Load("CarsCollection.xml");

            foreach (XElement car in doc.Root!.Elements())
            {
                foreach (XElement field in car.Elements())
                {
                    if (field.Name == "engine")
                    {
                        foreach (var engineElement in field.Elements())
                        {
                            if (engineElement.Name == "Horsepower")
                            {
                                engineElement.Name = "hp";
                            }
                        }
                    }
                    else if (field.Name == "Model")
                    {
                        var yearField = car.Element("Year");
                        XAttribute attribute = new XAttribute("Year", yearField!.Value);
                        field.Add(attribute);
                        yearField.Remove();
                    }
                }
            }

            doc.Save("CarsCollectionModified.xml");
        }
    }
}