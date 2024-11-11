using System.ComponentModel;
using Lab10.Core;

namespace Lab10;

public class SubBindingList : BindingList<Car>
{
    private bool Model = false;
    private bool Year = false;
    private bool Motor = false;

    public SubBindingList(List<Car> cars)
    {
        foreach (Car car in cars)
        {
            Add(car);
        }
    }
    public List<Car> Find(string text, string combo)
    {
        List<Car> matchingCars = new List<Car>();

        foreach (Car car in this)
        {
            switch (combo)
            {
                case "Model":
                {
                    if (car.Model == text)
                    {
                        matchingCars.Add(car);
                    }

                    break;
                }
                case "Year":
                {
                    if (car.Year == int.Parse(text))
                    {
                        matchingCars.Add(car);
                    }

                    break;
                }
                case "Motor":
                {
                    if (car.Motor.Model == text)
                    {
                        matchingCars.Add(car);
                    }

                    break;
                }
            }
        }

        return matchingCars;
    }

    public List<Car> AddElement(string model, string engineModel, double horsepower, double displacement, int year)
    {
        List<Car> matchingCars = this.ToList();
        matchingCars.Add(new Car(model, new Engine(displacement, horsepower, engineModel), year));
        return matchingCars;
    }

    public List<Car> Sort(string property)
    {
        List<Car> matchingCars = this.ToList();
        if(property.GetType().GetInterface("IComparable") != null)
        {
            switch (property)
            {
                case "Model":
                    {
                        Model = !Model;
                        if (Model)
                        {
                            return matchingCars = matchingCars.OrderBy(car => car.Model).ToList();
                        }
                        return matchingCars = matchingCars.OrderByDescending(car => car.Model).ToList();
                    }
                case "Year":
                    {
                        Year = !Year;
                        if (Year)
                        {
                            return matchingCars = matchingCars.OrderBy(car => car.Year).ToList();
                        }
                        return matchingCars = matchingCars.OrderByDescending(car => car.Year).ToList();
                    }
                case "Motor":
                    {
                        Motor = !Motor;
                        if (Motor)
                        {
                            return matchingCars = matchingCars.OrderBy(car => car.Motor.Model).ToList();
                        }
                        return matchingCars = matchingCars.OrderByDescending(car => car.Motor.Model).ToList();
                    }
                default:
                    return matchingCars;
            }
        }
        return matchingCars;
    }
        
}