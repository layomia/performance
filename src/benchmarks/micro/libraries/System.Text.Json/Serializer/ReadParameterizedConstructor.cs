// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using MicroBenchmarks;
using MicroBenchmarks.Serializers;
using System.IO;
using System.Threading.Tasks;

namespace System.Text.Json.Serialization.Tests
{
    //[GenericTypeArguments(typeof(SimpleStructWithProperties), typeof(SimpleStructWithProperties_1Arg), typeof(SimpleStructWithProperties_2Args))]
    [GenericTypeArguments(typeof(LoginViewModel), typeof(Parameterized_LoginViewModel_1Arg), typeof(Parameterized_LoginViewModel_3Args))]
    [GenericTypeArguments(typeof(Location), typeof(Parameterized_Location_1Arg), typeof(Parameterized_Location_9Args))]
    [GenericTypeArguments(typeof(IndexViewModel), typeof(Parameterized_IndexViewModel_1Arg), typeof(Parameterized_IndexViewModel_2Args))]
    [GenericTypeArguments(typeof(MyEventsListerViewModel), typeof(Parameterized_MyEventsListerViewModel_1Arg), typeof(Parameterized_MyEventsListerViewModel_3Args))]
    [GenericTypeArguments(typeof(Parameterless_Point), typeof(Parameterized_Point_1Arg), typeof(Parameterized_Point_2Args))]
    [GenericTypeArguments(typeof(Parameterless_ClassWithPrimitives), typeof(Parameterized_ClassWithPrimitives_3Args), typeof(Parameterized_ClassWithPrimitives_8Args))]
    [GenericTypeArguments(typeof(Parameterless_ComplexClass), typeof(Parameterized_ComplexClass_2Args), typeof(Parameterized_ComplexClass_8Args))]
    public class ReadParameterizedConstructor<TTypeWithParameterlessCtor, TTypeWithParameterizedCtor1, TTypeWithParameterizedCtorType2>
    {
        private string _serialized0;
        private string _serialized1;
        private string _serialized2;

        [GlobalSetup]
        public void Setup()
        {
            TTypeWithParameterlessCtor value0 = DataGenerator.Generate<TTypeWithParameterlessCtor>();
            TTypeWithParameterizedCtor1 value1 = DataGenerator.Generate<TTypeWithParameterizedCtor1>();
            TTypeWithParameterizedCtorType2 value2 = DataGenerator.Generate<TTypeWithParameterizedCtorType2>();

            _serialized0 = JsonSerializer.Serialize(value0);
            _serialized1 = JsonSerializer.Serialize(value1);
            _serialized2 = JsonSerializer.Serialize(value2);

            Console.WriteLine(_serialized0);
            Console.WriteLine(_serialized1);
            Console.WriteLine(_serialized2);
        }

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark(Baseline = true)]
        public TTypeWithParameterlessCtor Deserialize_Parameterless() => JsonSerializer.Deserialize<TTypeWithParameterlessCtor>(_serialized0);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        // Scenario:
        // - Less than 7 constructor arguments
        // - Fewer arguments than the below benchmark; check type being deserialized for exact count.
        // - JSON has constructor arguments first (as an optimization, the serializer will always produce this sort of order when serializing).
        public TTypeWithParameterizedCtor1 Deserialize_Parameterized_LessArgs_CtorArgsFirstJsonOrder() => JsonSerializer.Deserialize<TTypeWithParameterizedCtor1>(_serialized1);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        // Scenario:
        // - More arguments than the above benchmark; check type being deserialized for exact count.
        // - JSON has constructor arguments first (as an optimization, the serializer will always produce this sort of order when serializing).
        public TTypeWithParameterizedCtorType2 Deserialize_Parameterized_MoreArgs_CtorArgsFirstJsonOrder() => JsonSerializer.Deserialize<TTypeWithParameterizedCtorType2>(_serialized2);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        // Scenario:
        // - Less than 7 constructor arguments
        // - Fewer arguments than the below benchmark; check type being deserialized for exact count.
        // - JSON has constructor arguments in the middle of the payload, representing "average" case where the input wasn't written by the serializer.
        public TTypeWithParameterizedCtor1 Deserialize_Parameterized_LessArgs_AverageJsonOrder() => JsonSerializer.Deserialize<TTypeWithParameterizedCtor1>(_serialized0);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        // Scenario:
        // - More arguments than the above benchmark; check type being deserialized for exact count.
        // - JSON has constructor arguments in the middle of the payload, representing "average" case where the input wasn't written by the serializer.
        public TTypeWithParameterizedCtorType2 Deserialize_Parameterized_MoreArgs_AverageJsonOrder() => JsonSerializer.Deserialize<TTypeWithParameterizedCtorType2>(_serialized0);
    }

    ////[GenericTypeArguments(typeof(SimpleStructWithProperties), typeof(SimpleStructWithProperties_1Arg), typeof(SimpleStructWithProperties_2Args))]
    //[GenericTypeArguments(typeof(LoginViewModel), typeof(Parameterized_LoginViewModel_1Arg), typeof(Parameterized_LoginViewModel_3Args))]
    //[GenericTypeArguments(typeof(Location), typeof(Parameterized_Location_1Arg), typeof(Parameterized_Location_9Args))]
    //[GenericTypeArguments(typeof(IndexViewModel), typeof(Parameterized_IndexViewModel_1Arg), typeof(Parameterized_IndexViewModel_2Args))]
    //[GenericTypeArguments(typeof(MyEventsListerViewModel), typeof(Parameterized_MyEventsListerViewModel_1Arg), typeof(Parameterized_MyEventsListerViewModel_3Args))]
    //[GenericTypeArguments(typeof(Parameterless_Point), typeof(Parameterized_Point_1Arg), typeof(Parameterized_Point_2Args))]
    //[GenericTypeArguments(typeof(Parameterless_ClassWithPrimitives), typeof(Parameterized_ClassWithPrimitives_3Args), typeof(Parameterized_ClassWithPrimitives_8Args))]
    //[GenericTypeArguments(typeof(Parameterless_ComplexClass), typeof(Parameterized_ComplexClass_2Args), typeof(Parameterized_ComplexClass_8Args))]
    //public class ReadParameterizedConstructorAsync<TTypeWithParameterlessCtor, TTypeWithParameterizedCtor1, TTypeWithParameterizedCtor2>
    //{
    //    private MemoryStream _memoryStream0;
    //    private MemoryStream _memoryStream1;
    //    private MemoryStream _memoryStream2;

    //    [GlobalSetup]
    //    public async Task Setup()
    //    {
    //        TTypeWithParameterlessCtor value0 = DataGenerator.Generate<TTypeWithParameterlessCtor>();
    //        TTypeWithParameterizedCtor1 value1 = DataGenerator.Generate<TTypeWithParameterizedCtor1>();
    //        TTypeWithParameterizedCtor2 value2 = DataGenerator.Generate<TTypeWithParameterizedCtor2>();

    //        _memoryStream0 = new MemoryStream(capacity: short.MaxValue);
    //        await JsonSerializer.SerializeAsync(_memoryStream0, value0);

    //        _memoryStream1 = new MemoryStream(capacity: short.MaxValue);
    //        await JsonSerializer.SerializeAsync(_memoryStream1, value1);

    //        _memoryStream2 = new MemoryStream(capacity: short.MaxValue);
    //        await JsonSerializer.SerializeAsync(_memoryStream2, value2);
    //    }

    //    [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
    //    [Benchmark(Baseline = true)]
    //    public async Task<TTypeWithParameterlessCtor> DeserializeAsync_Parameterless()
    //    {
    //        _memoryStream0.Position = 0;
    //        var value = await JsonSerializer.DeserializeAsync<TTypeWithParameterlessCtor>(_memoryStream0);
    //        return value;
    //    }

    //    [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
    //    [Benchmark]
    //    // Scenario:
    //    // - Less than 7 constructor arguments
    //    // - Fewer arguments than the below benchmark; check type being deserialized for exact count.
    //    // - JSON has constructor arguments first (as an optimization, the serializer will always produce this sort of order when serializing).
    //    public async Task<TTypeWithParameterizedCtor1> DeserializeAsync_Parameterized_LessArgs_CtorArgsFirstJsonOrder()
    //    {
    //        _memoryStream1.Position = 0;
    //        var value = await JsonSerializer.DeserializeAsync<TTypeWithParameterizedCtor1>(_memoryStream1);
    //        return value;
    //    }

    //    [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
    //    [Benchmark]
    //    // Scenario:
    //    // - More arguments than the above benchmark; check type being deserialized for exact count.
    //    // - JSON has constructor arguments first (as an optimization, the serializer will always produce this sort of order when serializing).
    //    public async Task<TTypeWithParameterizedCtor2> DeserializeAsync_Parameterized_MoreArgs_CtorArgsFirstJsonOrder()
    //    {
    //        _memoryStream2.Position = 0;
    //        var value = await JsonSerializer.DeserializeAsync<TTypeWithParameterizedCtor2>(_memoryStream2);
    //        return value;
    //    }

    //    [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
    //    [Benchmark]
    //    // Scenario:
    //    // - Less than 7 constructor arguments
    //    // - Fewer arguments than the below benchmark; check type being deserialized for exact count.
    //    // - JSON has constructor arguments in the middle of the payload, representing "average" case where the input wasn't written by the serializer.
    //    public async Task<TTypeWithParameterizedCtor1> DeserializeAsync_Parameterized_LessArgs_AverageJsonOrder()
    //    {
    //        _memoryStream0.Position = 0;
    //        var value = await JsonSerializer.DeserializeAsync<TTypeWithParameterizedCtor1>(_memoryStream0);
    //        return value;
    //    }

    //    [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
    //    [Benchmark]
    //    // Scenario:
    //    // - More arguments than the above benchmark; check type being deserialized for exact count.
    //    // - JSON has constructor arguments in the middle of the payload, representing "average" case where the input wasn't written by the serializer.
    //    public async Task<TTypeWithParameterizedCtor2> DeserializeAsync_Parameterized_MoreArgs_AverageJsonOrder()
    //    {
    //        _memoryStream0.Position = 0;
    //        var value = await JsonSerializer.DeserializeAsync<TTypeWithParameterizedCtor2>(_memoryStream0);
    //        return value;
    //    }
    //}
}
