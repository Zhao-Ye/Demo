using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ConsoleApp1;

public class Benchmarks
{
    [Benchmark]
    public string GetDescription() => GetDescription(TestEnum.Test);

    [Benchmark]
    public string GetDescriptionByConcurrentDictionary() => GetDescriptionByConcurrentDictionary(TestEnum.Test);

    [Benchmark]
    public string EnumClass() => TestEnumClass.Test.ToString();
    
    [Benchmark]
    public string EnumStruct() => TestEnumStruct.Test.ToString();

    public string GetDescription<T>(T value)
    {
        if (value is null) return "";
        FieldInfo? fieldInfo = value.GetType().GetField(value.ToString() ?? throw new InvalidOperationException());
        if (fieldInfo is null) return value.ToString() ?? "";

        DescriptionAttribute? descriptionAttribute = fieldInfo.GetCustomAttribute<DescriptionAttribute>();

        return descriptionAttribute is null ? value.ToString() ?? "" : descriptionAttribute.Description;
    }

    private ConcurrentDictionary<Enum, string> _ConcurrentDictionary = new ConcurrentDictionary<Enum, string>();
    public string GetDescriptionByConcurrentDictionary(Enum value)
    {
        return _ConcurrentDictionary.GetOrAdd(value, (key) =>
        {
            var type = key.GetType();
            var field = type.GetField(key.ToString());
            return field == null ? key.ToString() : GetDescription(field);
        });
    }
}

public enum TestEnum
{
    [Description("TestEnum")] Test
}

public class TestEnumClass
{
    private string _value;
    private TestEnumClass(string value)
    {
        _value= value;
    }
    public override string ToString() => _value;

    public static readonly TestEnumClass Test = new TestEnumClass("TestEnumClass");
}

public readonly struct TestEnumStruct
{
    public static TestEnumStruct Test { get; } = new TestEnumStruct("TestEnumStruct");

    private readonly string _value;
    private TestEnumStruct(string value)
    {
        _value = value;
    }
    public override string ToString() => _value;
}