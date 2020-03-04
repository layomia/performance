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
    public class WriteParameterizedConstructor<TWithParameterlessCtor, TWithParameterizedCtor1, TWithParameterizedCtor2>
    {
        private TWithParameterlessCtor _value0;
        private TWithParameterizedCtor1 _value1;
        private TWithParameterizedCtor2 _value2;

        private object _objectWithObjectProperty0;
        private object _objectWithObjectProperty1;
        private object _objectWithObjectProperty2;

        [GlobalSetup]
        public void Setup()
        {
            _value0 = DataGenerator.Generate<TWithParameterlessCtor>();
            _value1 = DataGenerator.Generate<TWithParameterizedCtor1>();
            _value2 = DataGenerator.Generate<TWithParameterizedCtor2>();

            _objectWithObjectProperty0 = new { Prop = (object)_value0 };
            _objectWithObjectProperty1 = new { Prop = (object)_value1 };
            _objectWithObjectProperty2 = new { Prop = (object)_value2 };
        }

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark(Baseline = true)]
        public byte[] SerializeToUtf8Bytes_Parameterless() => JsonSerializer.SerializeToUtf8Bytes(_value0);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        public byte[] SerializeToUtf8Bytes_Parameterized_LessArgs() => JsonSerializer.SerializeToUtf8Bytes(_value1);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        public byte[] SerializeToUtf8Bytes_Parameterized_MoreArgs() => JsonSerializer.SerializeToUtf8Bytes(_value2);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        public byte[] SerializeToUtf8Bytes_Parameterless_ObjectProperty() => JsonSerializer.SerializeToUtf8Bytes(_objectWithObjectProperty0);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        public byte[] SerializeToUtf8Bytes_Parameterized_LessArgs_ObjectProperty() => JsonSerializer.SerializeToUtf8Bytes(_objectWithObjectProperty1);

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        public byte[] SerializeToUtf8Bytes_Parameterized_MoreArgs_ObjectProperty() => JsonSerializer.SerializeToUtf8Bytes(_objectWithObjectProperty2);
    }

    //[GenericTypeArguments(typeof(SimpleStructWithProperties), typeof(SimpleStructWithProperties_1Arg), typeof(SimpleStructWithProperties_2Args))]
    [GenericTypeArguments(typeof(LoginViewModel), typeof(Parameterized_LoginViewModel_1Arg), typeof(Parameterized_LoginViewModel_3Args))]
    [GenericTypeArguments(typeof(Location), typeof(Parameterized_Location_1Arg), typeof(Parameterized_Location_9Args))]
    [GenericTypeArguments(typeof(IndexViewModel), typeof(Parameterized_IndexViewModel_1Arg), typeof(Parameterized_IndexViewModel_2Args))]
    [GenericTypeArguments(typeof(MyEventsListerViewModel), typeof(Parameterized_MyEventsListerViewModel_1Arg), typeof(Parameterized_MyEventsListerViewModel_3Args))]
    [GenericTypeArguments(typeof(Parameterless_Point), typeof(Parameterized_Point_1Arg), typeof(Parameterized_Point_2Args))]
    [GenericTypeArguments(typeof(Parameterless_ClassWithPrimitives), typeof(Parameterized_ClassWithPrimitives_3Args), typeof(Parameterized_ClassWithPrimitives_8Args))]
    [GenericTypeArguments(typeof(Parameterless_ComplexClass), typeof(Parameterized_ComplexClass_2Args), typeof(Parameterized_ComplexClass_8Args))]
    public class WriteParameterizedConstructorAsync<TWithParameterlessCtor, TWithParameterizedCtor1, TWithParameterizedCtor2>
    {
        private TWithParameterlessCtor _value0;
        private TWithParameterizedCtor1 _value1;
        private TWithParameterizedCtor2 _value2;

        private MemoryStream _memoryStream0;
        private MemoryStream _memoryStream1;
        private MemoryStream _memoryStream2;

        [GlobalSetup]
        public async Task Setup()
        {
            _value0 = DataGenerator.Generate<TWithParameterlessCtor>();
            _value1 = DataGenerator.Generate<TWithParameterizedCtor1>();
            _value2 = DataGenerator.Generate<TWithParameterizedCtor2>();

            _memoryStream0 = new MemoryStream(capacity: short.MaxValue);
            await JsonSerializer.SerializeAsync(_memoryStream0, _value0);

            _memoryStream1 = new MemoryStream(capacity: short.MaxValue);
            await JsonSerializer.SerializeAsync(_memoryStream1, _value1);

            _memoryStream2 = new MemoryStream(capacity: short.MaxValue);
            await JsonSerializer.SerializeAsync(_memoryStream2, _value2);
        }

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark(Baseline = true)]
        public async Task SerializeAsync_Parameterless()
        {
            _memoryStream0.Position = 0;
            await JsonSerializer.SerializeAsync(_memoryStream0, _value0);
        }

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        public async Task SerializeAsync_Parameterized_LessArgs()
        {
            _memoryStream1.Position = 0;
            await JsonSerializer.SerializeAsync(_memoryStream1, _value1);
        }

        [BenchmarkCategory(Categories.Libraries, Categories.JSON)]
        [Benchmark]
        public async Task SerializeAsync_Parameterized_MoreArgs()
        {
            _memoryStream2.Position = 0;
            await JsonSerializer.SerializeAsync(_memoryStream2, _value2);
        }
    }
}
