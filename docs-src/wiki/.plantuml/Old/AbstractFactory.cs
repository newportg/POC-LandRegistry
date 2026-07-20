// Abstract Factory
public abstract class AbstractFactory
{
    public abstract AbstractProduct CreateProductA();
    public abstract AbstractProduct CreateProductB();
}

// Abstract Product
public abstract class AbstractProduct
{
    public abstract void DisplayInfo();
}

// Concrete Product A
public class ConcreteProductA : AbstractProduct
{
    public override void DisplayInfo()
    {
        Console.WriteLine("This is Product A.");
    }
}

// Concrete Product B
public class ConcreteProductB : AbstractProduct
{
    public override void DisplayInfo()
    {
        Console.WriteLine("This is Product B.");
    }
}

// Concrete Factory
public class ConcreteFactory : AbstractFactory
{
    public override AbstractProduct CreateProductA()
    {
        return new ConcreteProductA();
    }

    public override AbstractProduct CreateProductB()
    {
        return new ConcreteProductB();
    }
}

// Example Usage
class Program
{
    static void Main(string[] args)
    {
        AbstractFactory factory = new ConcreteFactory();

        AbstractProduct productA = factory.CreateProductA();
        productA.DisplayInfo();

        AbstractProduct productB = factory.CreateProductB();
        productB.DisplayInfo();
    }
}
