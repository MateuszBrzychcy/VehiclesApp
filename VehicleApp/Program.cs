using VehicleApp.Data;
using VehicleApp.Entities;
using VehicleApp.Repositories;


var carRepository = new SqlRepository<Car>(new VehicleAppDbContext());
//var truckRepository = new SqlRepository<Truck>(new VehicleAppDbContext());

carRepository.ItemAdded += CarRepositoryOnItemAdded;
carRepository.ItemRemoved += CarRepositoryOnItemRemoved;

static void CarRepositoryOnItemAdded<T>(object? sender, T vehicle) where T : class, IEntity, new()
{
    var log = String.Format($"[{DateTime.Now}] -  VehicleAdded  - [{vehicle.ToString()}]");
    using (var fileAudit = File.AppendText("Audit.txt"))
    {
        fileAudit.WriteLine(log);
    };
}

static void CarRepositoryOnItemRemoved<T>(object? sender, T vehicle) where T : class, IEntity, new()
{
    var log = String.Format($"[{DateTime.Now}] - VehicleRemoved - [{vehicle.ToString()}]");
    using (var fileAudit = File.AppendText("Audit.txt"))
    {
        fileAudit.WriteLine(log);
    };
}

Console.WriteLine("Witaj w programie, który ułatwia zarządzanie bazą pojazdów. \nMożesz dodawać do bazy danych samochody osobowe oraz ciężarowe(Narazie tylko osobowe)\n");
string? selectMenu = "0";
string text =
    "MENU GŁÓWNE\nWpisz:\n 1 - Aby dodać nowy pojazd\n 2 - Aby usunąć pojazd\n 3 - Aby wyświetlić listę pojazdów\n 9 - Aby wyjść z programu";
do
{
    Console.WriteLine(text);
    selectMenu = Console.ReadLine();
    switch (selectMenu)
    {
        case "1":
            AddVehicle();
            break;

        case "2":
            RemoveVehicle();
            break;

        case "3":
            ShowListOfVehicles();
            break;

        case "9":
            continue;

        default:
            Console.Clear();
            Console.WriteLine("Podano złą wartość!");
            continue;
    }
}
while (selectMenu != "9");

void AddVehicle()
{
    Console.Clear();
    var selectAddingVehicle = "0";
    do
    {
        Console.WriteLine("Dodaj pojazd\nWpisz: \n 1 - Aby dodać samochód osobowy\n 2 - Aby dodać samochód ciężarowy\n 3 - Aby cofnąć się do poprzedniego menu");
        selectAddingVehicle = Console.ReadLine();

        switch (selectAddingVehicle)
        {
            case "1":
                AddCar();
                break;

            case "2":
                AddTruck();
                break;

            case "3":
                Console.Clear();
                break;

            default:
                Console.Clear();
                Console.WriteLine("Podano złą wartość");
                continue;
        }
    }
    while (selectAddingVehicle != "1" && selectAddingVehicle != "2" && selectAddingVehicle != "3");

}

void AddCar()
{
    var newcar = new Car();
    Console.Clear();

    Console.WriteLine("Podaj markę samochodu:");
    newcar.Brand = Console.ReadLine();

    Console.WriteLine("Podaj Model:");
    newcar.Name = Console.ReadLine();

    Console.WriteLine("Podaj ilość siedzeń:");
    int numberOfSeats;
    int.TryParse(Console.ReadLine(), out numberOfSeats);
    newcar.NumberOfSeats = numberOfSeats;

    Console.WriteLine("Podaj rok produkcji:");
    newcar.YearOfManufacture = Console.ReadLine();

    Console.WriteLine("Podaj rodzaj paliwa (1-benzyna, 2-diesel)");
    var selectFuelType = Console.ReadLine();
    if (selectFuelType == "1")
    {
        newcar.FuelType = FuelType.Petrol;
    }
    else if (selectFuelType == "2")
    {
        newcar.FuelType = FuelType.Diesel;
    }
    else
    {
        throw new ArgumentException("Błąd. Podano zły rodzaj paliwa.");
    }

    Console.WriteLine($"Podaj typ nadwozia (\"sedan\", \"kombi\" lub \"hatchback\"");
    newcar.BodyType = Console.ReadLine();

    carRepository.Add(newcar);
    carRepository.Save();
    carRepository.ItemAdded += CarRepositoryOnItemAdded;

    Console.WriteLine($"Dodano samochód osobowy: {newcar.Brand} {newcar.Name}");
}

void AddTruck()
{
    Console.WriteLine("Narazie sie nie da dodawać ciężarówek");
    //var newTruck = new Truck();
    //Console.Clear();

    //Console.WriteLine("Podaj markę samochodu ciężarowego:");
    //newTruck.Brand = Console.ReadLine();

    //Console.WriteLine("Podaj Model:");
    //newTruck.Name = Console.ReadLine();

    //Console.WriteLine("Podaj rok produkcji:");
    //newTruck.YearOfManufacture = Console.ReadLine();

    //Console.WriteLine("Podaj rodzaj paliwa (1-benzyna, 2-diesel)");
    //var selectFuelType = Console.ReadLine();
    //if (selectFuelType == "1")
    //{
    //    newTruck.FuelType = FuelType.Petrol;
    //}
    //else if (selectFuelType == "2")
    //{
    //    newTruck.FuelType = FuelType.Diesel;
    //}
    //else
    //{
    //    throw new ArgumentException("Błąd. Podano zły rodzaj paliwa.");
    //}

    //    truckRepository.Add(newTruck);
    //    truckRepository.Save();
    //    truckRepository.ItemAdded += CarRepositoryOnItemAdded;


    //Console.WriteLine($"Dodano samochód ciężarowy: {newTruck.Brand} {newTruck.Name}");
}

void RemoveVehicle()
{
    Console.Clear();
    Console.WriteLine("Wpisz\n 1 - Aby usunąć samochód osobowy\n 2 - Aby usunąć samochód ciężarowy");
    var selectCarOrVehicle = Console.ReadLine();
    switch (selectCarOrVehicle)
    {
        case "1":
            ShowCars();
            RemoveCar();
            break;

        case "2":
            //ShowTrucks();
            RemoveTruck();
            break;

        default:
            throw new ArgumentException("Podano niewłaściwy numer");
    }
}
void RemoveCar()
{
    int selectId;
    bool selectSucces = false;
    while (selectSucces == false)
    {
        Console.WriteLine("Wpisz numer Id pojazdu ktróry chcesz usunąć:");
        int.TryParse(Console.ReadLine(), out selectId);
        using (var dataBase = new VehicleAppDbContext())
        {
            var carRepository = new SqlRepository<Car>(dataBase);
            var selectedVehicle = carRepository.GetById(selectId);
            if (selectedVehicle != null)
            {
                carRepository.Remove(selectedVehicle);
                carRepository.Save();
                carRepository.ItemRemoved += CarRepositoryOnItemRemoved;

                selectSucces = true;
                Console.Clear();
                Console.WriteLine($"Usunięto pojazd {selectedVehicle.ToString()}");
            }
            else
            {
                Console.WriteLine("Nie ma pojazdu o podanym Id");
            }
        }
    }
}
void RemoveTruck()
{
    Console.WriteLine("Narazie sie nie da usuwać ciężarówek");
    //int selectId;
    //bool selectSucces = false;
    //while (selectSucces == false)
    //{
    //    Console.WriteLine("Wpisz numer Id pojazdu ktróry chcesz usunąć:");
    //    int.TryParse(Console.ReadLine(), out selectId);
    //    using (var dataBase = new VehicleAppDbContext())
    //    {
    //        var truckRepository = new SqlRepository<Truck>(dataBase);
    //        var selectedVehicle = truckRepository.GetById(selectId);
    //        if (selectedVehicle != null)
    //        {
    //            truckRepository.Remove(selectedVehicle);
    //            truckRepository.Save();
    //            truckRepository.ItemRemoved += CarRepositoryOnItemRemoved;

    //            selectSucces = true;
    //            Console.Clear();
    //            Console.WriteLine($"Usunięto pojazd {selectedVehicle.ToString()}");
    //        }
    //        else
    //        {
    //            Console.WriteLine("Nie ma pojazdu o podanym Id");
    //        }
    //    }
    //}
}

void ShowListOfVehicles()
{
    Console.Clear();
    ShowCars();
    ShowTrucks();
}

void ShowCars()
{
    Console.WriteLine("Samochody osobowe:");
    Console.WriteLine($"ID {"Marka",11} {"Model",11}");
    Console.WriteLine("-------------------------");

    var carList = carRepository.GetAll();
    foreach (var car in carList)
    {
        Console.WriteLine($"{car.Id,-3}{car.Brand,11} {car.Name,11}");
    }

}
void ShowTrucks()
{
    //Console.WriteLine("Samochody ciężarowe:");
    //Console.WriteLine($"ID {"Marka",11} {"Model",11}");
    //Console.WriteLine("-------------------------");

    //    var truckList = truckRepository.GetAll();
    //    foreach (var truck in truckList)
    //    {
    //        Console.WriteLine($"{truck.Id,-3}{truck.Brand,11}{truck.Name,11}");
    //    }

}