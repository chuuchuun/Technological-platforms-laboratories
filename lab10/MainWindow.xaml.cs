using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab10.Core;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace Lab10;

public partial class MainWindow : Window
{
    private static readonly List<Car> MyCars = new()
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

    private List<Car> tmp;
    private BindingList<Car> myCarsBindingList;
    private SubBindingList carsList = new SubBindingList(MyCars);


    public MainWindow()
    {
        InitializeComponent();

        ComboBox.Items.Add("Model");
        ComboBox.Items.Add("Motor");
        ComboBox.Items.Add("Year");

        BindDataToGrid(MyCars);

        query_expression();
        method_based();
        task2();
    }

    private void BindDataToGrid(List<Car> cars)
    {
        myCarsBindingList = new BindingList<Car>(cars);
        BindingSource carBindingSource = new BindingSource();
        carBindingSource.DataSource = myCarsBindingList;
        CarsDataGrid.ItemsSource = carBindingSource;
    }

    private static void task2()
    {
        Func<Car, Car, int> arg1 = Func;
        Predicate<Car> arg2 = Predicate;
        Action<Car> arg3 = Action;
        MyCars.Sort(new Comparison<Car>(arg1));
        MyCars.FindAll(arg2).ForEach(arg3);
    }

    private static int Func(Car a, Car b)
    {
        if (a.Motor.Horsepower > b.Motor.Horsepower)
        {
            return -1;
        }

        if (a.Motor.Horsepower < b.Motor.Horsepower)
        {
            return 1;
        }

        return 0;
    }

    private static bool Predicate(Car a)
    {
        return a.Motor.Model == "TDI";
    }

    private static void Action(Car a)
    {
        MessageBox.Show("Model: " + a.Model + " Motor: " + a.Motor + " Year: " + a.Year);
    }

    private void Delete_Button(object sender, RoutedEventArgs e)
    {
        var selectedCar = CarsDataGrid.SelectedItem as Car;
        if (selectedCar != null)
        {
            tmp = carsList.ToList().Where(x => x != selectedCar).ToList();
            carsList = new SubBindingList(tmp);
            BindDataToGrid(tmp);
        }
    }

    private void Search_Button(object sender, RoutedEventArgs e)
    {
        var query = SearchTextBox.Text;
        if (ComboBox.SelectedItem is null || query == "" )
        {
            BindDataToGrid(carsList.ToList());
            return;
        }
        var property = ComboBox.SelectedItem.ToString();

        tmp = carsList.Find(query, property);
        BindDataToGrid(tmp);
    }

    public void Add_Button(object sender, RoutedEventArgs e)
    {
        var model = Model.Text;
        var engineModel = EngineModel.Text;
        var horsepower = float.Parse(Horsepower.Text);;
        var displacement = float.Parse(Displacement.Text);
        var year = int.Parse( Year.Text);

        
        tmp = carsList.AddElement(model, engineModel, horsepower, displacement, year);
        carsList = new SubBindingList(tmp);
        BindDataToGrid(tmp);
    }

    private static void query_expression()
    {
        var result = from c in MyCars
            where c.Model == "A6"
            let engineType = c.Motor.Model == "TDI" ? "diesel" : "petrol"
            let hppl = (double)c.Motor.Horsepower / c.Motor.Displacement
            group hppl by engineType
            into g
            orderby g.Average() descending
            select new
            {
                engineType = g.Key,
                avgHPPL = g.Average()
            };
        var odp = result.Aggregate("query expression syntax\n", (current, e) => current + (e.engineType + ": " + e.avgHPPL + " \n"));
        MessageBox.Show(odp);
    }

    private static void method_based()
    {
        var result = MyCars
            .Where(c => c.Model == "A6")
            .Select(c => new
            {
                engineType = c.Motor.Model == "TDI" ? "diesel" : "petrol",
                hppl = (double)c.Motor.Horsepower / c.Motor.Displacement
            })
            .GroupBy(c => c.engineType)
            .Select(g => new
            {
                engineType = g.Key,
                avgHPPL = g.Average(c => c.hppl)
            })
            .OrderByDescending(c => c.avgHPPL);
        
        var odp = result.Aggregate("method-based query syntax\n", (current, e) => current + (e.engineType + ": " + e.avgHPPL + " \n"));
        MessageBox.Show(odp);
    }

    private void Sort_Model(object sender, RoutedEventArgs e)
    {
        tmp = carsList.Sort("Model");
        BindDataToGrid(tmp);
    }

    private void Sort_Year(object sender, RoutedEventArgs e)
    {
        tmp = carsList.Sort("Year");
        BindDataToGrid(tmp);
    }

    private void Sort_Motor(object sender, RoutedEventArgs e)
    {
        tmp = carsList.Sort("Motor");
        BindDataToGrid(tmp);
    }
}