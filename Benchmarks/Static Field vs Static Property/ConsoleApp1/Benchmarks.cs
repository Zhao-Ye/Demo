using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

public class Benchmarks
{
	[Benchmark]
	public void StaticFieldWriteLine() => Console.WriteLine($"{StaticField.Test}");

	[Benchmark]
    public string GetStaticField() => StaticField.Test.ToString();

    [Benchmark]
	public void StaticPropertyWriteLine() => Console.WriteLine($"{StaticProperty.Test}");

    [Benchmark]
    public string GetStaticProperty() => StaticProperty.Test.ToString();
}

public class StaticField
{
	private readonly string _value;

	private StaticField(string value)
	{
		this._value = value;
	}

	public override string ToString() => _value;

	public static readonly StaticField Test = new StaticField("Test");
}

public class StaticProperty
{
	private readonly string _value;

	private StaticProperty(string value)
	{
		this._value = value;
	}

	public override string ToString() => _value;

	public static StaticProperty Test { get; } = new StaticProperty("Test");
}