// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using BenchmarkDotNet.Extensions;
using MessagePack;
using ProtoBuf;

namespace MicroBenchmarks.Serializers
{
    internal static class DataGenerator
    {
        internal static T Generate<T>()
        {
            if (typeof(T) == typeof(LoginViewModel))
                return (T)(object)CreateLoginViewModel();
            if (typeof(T) == typeof(Location))
                return (T)(object)CreateLocation();
            if (typeof(T) == typeof(IndexViewModel))
                return (T)(object)CreateIndexViewModel();
            if (typeof(T) == typeof(MyEventsListerViewModel))
                return (T)(object)CreateMyEventsListerViewModel();
            if (typeof(T) == typeof(BinaryData))
                return (T)(object)CreateBinaryData(1024);
            if (typeof(T) == typeof(CollectionsOfPrimitives))
                return (T)(object)CreateCollectionsOfPrimitives(1024); // 1024 values was copied from CoreFX benchmarks
            if (typeof(T) == typeof(XmlElement))
                return (T)(object)CreateXmlElement();
            if (typeof(T) == typeof(SimpleStructWithProperties))
                return (T)(object)new SimpleStructWithProperties { Num = 1, Text = "Foo" };
            if (typeof(T) == typeof(SimpleListOfInt))
                return (T)(object)new SimpleListOfInt { 10, 20, 30 };
            if (typeof(T) == typeof(ClassImplementingIXmlSerialiable))
                return (T)(object)new ClassImplementingIXmlSerialiable { StringValue = "Hello world" };
            if (typeof(T) == typeof(Dictionary<string, string>))
                return (T)(object)ValuesGenerator.ArrayOfUniqueValues<string>(100).ToDictionary(value => value);
            if (typeof(T) == typeof(ImmutableDictionary<string, string>))
                return (T)(object)ImmutableDictionary.CreateRange(ValuesGenerator.ArrayOfUniqueValues<string>(100).ToDictionary(value => value));
            if (typeof(T) == typeof(ImmutableSortedDictionary<string, string>))
                return (T)(object)ImmutableSortedDictionary.CreateRange(ValuesGenerator.ArrayOfUniqueValues<string>(100).ToDictionary(value => value));
            if (typeof(T) == typeof(HashSet<string>))
                return (T)(object)new HashSet<string>(ValuesGenerator.ArrayOfUniqueValues<string>(100));
            if (typeof(T) == typeof(ArrayList))
                return (T)(object)new ArrayList(ValuesGenerator.ArrayOfUniqueValues<string>(100));
            if (typeof(T) == typeof(Hashtable))
                return (T)(object)new Hashtable(ValuesGenerator.ArrayOfUniqueValues<string>(100).ToDictionary(value => value));
            if (typeof(T) == typeof(Parameterless_Point))
                return (T)(object)CreateParameterlessPoint();
            if (typeof(T) == typeof(Parameterized_Point_1Arg))
                return (T)(object)CreateParameterizedPoint1Arg();
            if (typeof(T) == typeof(Parameterized_Point_2Args))
                return (T)(object)CreateParameterizedPoint2Args();
            if (typeof(T) == typeof(Parameterless_ClassWithPrimitives))
                return (T)(object)CreateParameterlessClassWithPrimitives();
            if (typeof(T) == typeof(Parameterized_ClassWithPrimitives_3Args))
                return (T)(object)CreateParameterizedClassWithPrimitives3Args();
            if (typeof(T) == typeof(Parameterized_ClassWithPrimitives_8Args))
                return (T)(object)CreateParameterizedClassWithPrimitives8Args();
            if (typeof(T) == typeof(Parameterized_IndexViewModel_1Arg))
                return (T)(object)CreateParameterizedIndexViewModel1Arg();
            if (typeof(T) == typeof(Parameterized_IndexViewModel_2Args))
                return (T)(object)CreateParameterizedIndexViewModel2Args();

            throw new NotImplementedException();
        }

        private static LoginViewModel CreateLoginViewModel()
            => new LoginViewModel
            {
                Email = "name.familyname@not.com",
                Password = "abcdefgh123456!@",
                RememberMe = true
            };

        private static Location CreateLocation()
            => new Location
            {
                Id = 1234,
                Address1 = "The Street Name",
                Address2 = "20/11",
                City = "The City",
                State = "The State",
                PostalCode = "abc-12",
                Name = "Nonexisting",
                PhoneNumber = "+0 11 222 333 44",
                Country = "The Greatest"
            };

        private static IndexViewModel CreateIndexViewModel()
            => new IndexViewModel
            {
                IsNewAccount = false,
                FeaturedCampaign = new CampaignSummaryViewModel
                {
                    Description = "Very nice campaing",
                    Headline = "The Headline",
                    Id = 234235,
                    OrganizationName = "The Company XYZ",
                    ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                    Title = "Promoting Open Source"
                },
                ActiveOrUpcomingEvents = Enumerable.Repeat(
                    new ActiveOrUpcomingEvent
                    {
                        Id = 10,
                        CampaignManagedOrganizerName = "Name FamiltyName",
                        CampaignName = "The very new campaing",
                        Description = "The .NET Foundation works with Microsoft and the broader industry to increase the exposure of open source projects in the .NET community and the .NET Foundation. The .NET Foundation provides access to these resources to projects and looks to promote the activities of our communities.",
                        EndDate = DateTime.UtcNow.AddYears(1),
                        Name = "Just a name",
                        ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                        StartDate = DateTime.UtcNow
                    },
                    count: 20).ToList()
            };

        private static MyEventsListerViewModel CreateMyEventsListerViewModel()
            => new MyEventsListerViewModel
            {
                CurrentEvents = Enumerable.Repeat(CreateMyEventsListerItem(), 3).ToList(),
                FutureEvents = Enumerable.Repeat(CreateMyEventsListerItem(), 9).ToList(),
                PastEvents = Enumerable.Repeat(CreateMyEventsListerItem(), 60).ToList() // usually  there is a lot of historical data
            };

        private static MyEventsListerItem CreateMyEventsListerItem()
            => new MyEventsListerItem
            {
                Campaign = "A very nice campaing",
                EndDate = DateTime.UtcNow.AddDays(7),
                EventId = 321,
                EventName = "wonderful name",
                Organization = "Local Animal Shelter",
                StartDate = DateTime.UtcNow.AddDays(-7),
                TimeZone = TimeZoneInfo.Utc.DisplayName,
                VolunteerCount = 15,
                Tasks = Enumerable.Repeat(
                    new MyEventsListerItemTask
                    {
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(1),
                        Name = "A very nice task to have"
                    }, 4).ToList()
            };

        private static BinaryData CreateBinaryData(int size)
            => new BinaryData
            {
                ByteArray = CreateByteArray(size)
            };

        private static CollectionsOfPrimitives CreateCollectionsOfPrimitives(int count)
            => new CollectionsOfPrimitives
            {
                ByteArray = CreateByteArray(count),
                DateTimeArray = CreateDateTimeArray(count),
                Dictionary = CreateDictionaryOfIntString(count),
                ListOfInt = CreateListOfInt(count)
            };

        private static DateTime[] CreateDateTimeArray(int count)
        {
            DateTime[] arr = new DateTime[count];
            int kind = (int)DateTimeKind.Unspecified;
            int maxDateTimeKind = (int) DateTimeKind.Local;
            DateTime val = DateTime.Now.AddHours(count/2);
            for (int i = 0; i < count; i++)
            {
                arr[i] = DateTime.SpecifyKind(val, (DateTimeKind)kind);
                val = val.AddHours(1);
                kind = (kind + 1)%maxDateTimeKind;
            }

            return arr;
        }
        
        private static Dictionary<int, string> CreateDictionaryOfIntString(int count)
        {
            Dictionary<int, string> dictOfIntString = new Dictionary<int, string>(count);
            for (int i = 0; i < count; ++i)
            {
                dictOfIntString[i] = i.ToString();
            }

            return dictOfIntString;
        }

        private static byte[] CreateByteArray(int size)
        {
            byte[] obj = new byte[size];
            for (int i = 0; i < obj.Length; ++i)
            {
                unchecked
                {
                    obj[i] = (byte)i;
                }
            }
            return obj;
        }

        private static List<int> CreateListOfInt(int count) => Enumerable.Range(0, count).ToList();

        private static XmlElement CreateXmlElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(@"<html></html>");
            XmlElement xmlElement = xmlDoc.CreateElement("Element");
            xmlElement.InnerText = "Element innertext";
            return xmlElement;
        }

        private static Parameterless_Point CreateParameterlessPoint()
        {
            var point = new Parameterless_Point();
            point.X = 234235;
            point.Y = 912874;
            return point;
        }

        private static Parameterized_Point_1Arg CreateParameterizedPoint1Arg()
        {
            var point = new Parameterized_Point_1Arg(234235);
            point.Y = 912874;
            return point;
        }

        private static Parameterized_Point_2Args CreateParameterizedPoint2Args()
        {
            var point = new Parameterized_Point_2Args(234235, 912874);
            return point;
        }

        private static Parameterless_ClassWithPrimitives CreateParameterlessClassWithPrimitives()
        {
            var point = new Parameterless_ClassWithPrimitives();

            point.FirstInt = 348943;
            point.SecondInt = 348943;
            point.FirstString = "934sdkjfskdfssf";
            point.SecondString = "sdad9434243242";
            point.FirstDateTime = DateTime.Now;
            point.SecondDateTime = DateTime.Now.AddHours(1).AddYears(1);

            point.X = 234235;
            point.Y = 912874;
            point.Z = 434934;

            point.ThirdInt = 348943;
            point.FourthInt = 348943;
            point.ThirdString = "934sdkjfskdfssf";
            point.FourthString = "sdad9434243242";
            point.ThirdDateTime = DateTime.Now;
            point.FourthDateTime = DateTime.Now.AddHours(1).AddYears(1);

            return point;
        }

        private static Parameterized_ClassWithPrimitives_3Args CreateParameterizedClassWithPrimitives3Args()
        {
            var point = new Parameterized_ClassWithPrimitives_3Args(x: 234235, y: 912874, z: 434934);

            point.FirstInt = 348943;
            point.SecondInt = 348943;
            point.FirstString = "934sdkjfskdfssf";
            point.SecondString = "sdad9434243242";
            point.FirstDateTime = DateTime.Now;
            point.SecondDateTime = DateTime.Now.AddHours(1).AddYears(1);

            point.ThirdInt = 348943;
            point.FourthInt = 348943;
            point.ThirdString = "934sdkjfskdfssf";
            point.FourthString = "sdad9434243242";
            point.ThirdDateTime = DateTime.Now;
            point.FourthDateTime = DateTime.Now.AddHours(1).AddYears(1);

            return point;
        }

        private static Parameterized_ClassWithPrimitives_8Args CreateParameterizedClassWithPrimitives8Args()
        {
            var point = new Parameterized_ClassWithPrimitives_8Args(
                firstDateTime: DateTime.Now,
                secondDateTime: DateTime.Now.AddHours(1).AddYears(1),
                x: 234235,
                y: 912874,
                z: 434934,
                thirdInt: 348943,
                fourthInt: 348943,
                thirdString: "934sdkjfskdfssf");

            point.FirstInt = 348943;
            point.SecondInt = 348943;
            point.FirstString = "934sdkjfskdfssf";
            point.SecondString = "sdad9434243242";
            point.FourthString = "sdad9434243242";
            point.ThirdDateTime = DateTime.Now;
            point.FourthDateTime = DateTime.Now.AddHours(1).AddYears(1);

            return point;
        }

        private static Parameterized_IndexViewModel_1Arg CreateParameterizedIndexViewModel1Arg()
            => new Parameterized_IndexViewModel_1Arg(isNewAccount: false)
            {
                FeaturedCampaign = new CampaignSummaryViewModel
                {
                    Description = "Very nice campaing",
                    Headline = "The Headline",
                    Id = 234235,
                    OrganizationName = "The Company XYZ",
                    ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                    Title = "Promoting Open Source"
                },
                ActiveOrUpcomingEvents = Enumerable.Repeat(
                    new ActiveOrUpcomingEvent
                    {
                        Id = 10,
                        CampaignManagedOrganizerName = "Name FamiltyName",
                        CampaignName = "The very new campaing",
                        Description = "The .NET Foundation works with Microsoft and the broader industry to increase the exposure of open source projects in the .NET community and the .NET Foundation. The .NET Foundation provides access to these resources to projects and looks to promote the activities of our communities.",
                        EndDate = DateTime.UtcNow.AddYears(1),
                        Name = "Just a name",
                        ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                        StartDate = DateTime.UtcNow
                    },
                    count: 20).ToList()
            };

        private static Parameterized_IndexViewModel_2Args CreateParameterizedIndexViewModel2Args()
            => new Parameterized_IndexViewModel_2Args(
                featuredCampaign: new CampaignSummaryViewModel
                {
                    Description = "Very nice campaing",
                    Headline = "The Headline",
                    Id = 234235,
                    OrganizationName = "The Company XYZ",
                    ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                    Title = "Promoting Open Source"
                },
                isNewAccount: false
                )
            {
                ActiveOrUpcomingEvents = Enumerable.Repeat(
                    new ActiveOrUpcomingEvent
                    {
                        Id = 10,
                        CampaignManagedOrganizerName = "Name FamiltyName",
                        CampaignName = "The very new campaing",
                        Description = "The .NET Foundation works with Microsoft and the broader industry to increase the exposure of open source projects in the .NET community and the .NET Foundation. The .NET Foundation provides access to these resources to projects and looks to promote the activities of our communities.",
                        EndDate = DateTime.UtcNow.AddYears(1),
                        Name = "Just a name",
                        ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                        StartDate = DateTime.UtcNow
                    },
                    count: 20).ToList()
            };
    }

    // the view models come from a real world app called "AllReady"
    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class LoginViewModel
    {
        [ProtoMember(1)] [Key(0)] public string Email { get; set; }
        [ProtoMember(2)] [Key(1)] public string Password { get; set; }
        [ProtoMember(3)] [Key(2)] public bool RememberMe { get; set; }
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class Location
    {
        [ProtoMember(1)] [Key(0)] public int Id { get; set; }
        [ProtoMember(2)] [Key(1)] public string Address1 { get; set; }
        [ProtoMember(3)] [Key(2)] public string Address2 { get; set; }
        [ProtoMember(4)] [Key(3)] public string City { get; set; }
        [ProtoMember(5)] [Key(4)] public string State { get; set; }
        [ProtoMember(6)] [Key(5)] public string PostalCode { get; set; }
        [ProtoMember(7)] [Key(6)] public string Name { get; set; }
        [ProtoMember(8)] [Key(7)] public string PhoneNumber { get; set; }
        [ProtoMember(9)] [Key(8)] public string Country { get; set; }
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class ActiveOrUpcomingCampaign
    {
        [ProtoMember(1)] [Key(0)] public int Id { get; set; }
        [ProtoMember(2)] [Key(1)] public string ImageUrl { get; set; }
        [ProtoMember(3)] [Key(2)] public string Name { get; set; }
        [ProtoMember(4)] [Key(3)] public string Description { get; set; }
        [ProtoMember(5)] [Key(4)] public DateTimeOffset StartDate { get; set; }
        [ProtoMember(6)] [Key(5)] public DateTimeOffset EndDate { get; set; }
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class ActiveOrUpcomingEvent
    {
        [ProtoMember(1)] [Key(0)] public int Id { get; set; }
        [ProtoMember(2)] [Key(1)] public string ImageUrl { get; set; }
        [ProtoMember(3)] [Key(2)] public string Name { get; set; }
        [ProtoMember(4)] [Key(3)] public string CampaignName { get; set; }
        [ProtoMember(5)] [Key(4)] public string CampaignManagedOrganizerName { get; set; }
        [ProtoMember(6)] [Key(5)] public string Description { get; set; }
        [ProtoMember(7)] [Key(6)] public DateTimeOffset StartDate { get; set; }
        [ProtoMember(8)] [Key(7)] public DateTimeOffset EndDate { get; set; }
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class CampaignSummaryViewModel
    {
        [ProtoMember(1)] [Key(0)] public int Id { get; set; }
        [ProtoMember(2)] [Key(1)] public string Title { get; set; }
        [ProtoMember(3)] [Key(2)] public string Description { get; set; }
        [ProtoMember(4)] [Key(3)] public string ImageUrl { get; set; }
        [ProtoMember(5)] [Key(4)] public string OrganizationName { get; set; }
        [ProtoMember(6)] [Key(5)] public string Headline { get; set; }
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class IndexViewModel 
    {
        [ProtoMember(1)] [Key(0)] public List<ActiveOrUpcomingEvent> ActiveOrUpcomingEvents { get; set; }
        [ProtoMember(2)] [Key(1)] public CampaignSummaryViewModel FeaturedCampaign { get; set; }
        [ProtoMember(3)] [Key(2)] public bool IsNewAccount { get; set; }
        [IgnoreMember] public bool HasFeaturedCampaign => FeaturedCampaign != null;
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class MyEventsListerViewModel
    {
        // the orginal type defined these fields as IEnumerable,
        // but XmlSerializer failed to serialize them with "cannot serialize member because it is an interface" error
        [ProtoMember(1)] [Key(0)] public List<MyEventsListerItem> CurrentEvents { get; set; } = new List<MyEventsListerItem>();
        [ProtoMember(2)] [Key(1)] public List<MyEventsListerItem> FutureEvents { get; set; } = new List<MyEventsListerItem>();
        [ProtoMember(3)] [Key(2)] public List<MyEventsListerItem> PastEvents { get; set; } = new List<MyEventsListerItem>();
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class MyEventsListerItem
    {
        [ProtoMember(1)] [Key(0)] public int EventId { get; set; }
        [ProtoMember(2)] [Key(1)] public string EventName { get; set; }
        [ProtoMember(3)] [Key(2)] public DateTimeOffset StartDate { get; set; }
        [ProtoMember(4)] [Key(3)] public DateTimeOffset EndDate { get; set; }
        [ProtoMember(5)] [Key(4)] public string TimeZone { get; set; }
        [ProtoMember(6)] [Key(5)] public string Campaign { get; set; }
        [ProtoMember(7)] [Key(6)] public string Organization { get; set; }
        [ProtoMember(8)] [Key(7)] public int VolunteerCount { get; set; }

        [ProtoMember(9)] [Key(8)] public List<MyEventsListerItemTask> Tasks { get; set; } = new List<MyEventsListerItemTask>();
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class MyEventsListerItemTask
    {
        [ProtoMember(1)] [Key(0)] public string Name { get; set; }
        [ProtoMember(2)] [Key(1)] public DateTimeOffset? StartDate { get; set; }
        [ProtoMember(3)] [Key(2)] public DateTimeOffset? EndDate { get; set; }

        [IgnoreMember]
        public string FormattedDate
        {
            get
            {
                if (!StartDate.HasValue || !EndDate.HasValue)
                {
                    return null;
                }

                var startDateString = string.Format("{0:g}", StartDate.Value);
                var endDateString = string.Format("{0:g}", EndDate.Value);

                return string.Format($"From {startDateString} to {endDateString}");
            }
        }
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class BinaryData
    {
        [ProtoMember(1)] [Key(0)] public byte[] ByteArray { get; set; }
    }

    [Serializable]
    [ProtoContract]
    [MessagePackObject]
    public class CollectionsOfPrimitives
    {
        [ProtoMember(1)] [Key(0)] public byte[] ByteArray { get; set; }
        [ProtoMember(2)] [Key(1)] public DateTime[] DateTimeArray { get; set; }
        
        [XmlIgnore] // xml serializer does not support anything that implements IDictionary..
        [ProtoMember(3)] [Key(2)] public Dictionary<int, string> Dictionary { get; set; }
        
        [ProtoMember(4)] [Key(3)] public List<int> ListOfInt { get; set; }
    }
    
    public struct SimpleStructWithProperties
    {
        public int Num { get; set; }
        public string Text { get; set; }
    }

    public class SimpleListOfInt : List<int> { }

    public class Parameterless_Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Parameterized_Point_1Arg
    {
        public int X { get; }
        public int Y { get; set; }

        public Parameterized_Point_1Arg(int x) => X = x;
    }

    public class Parameterized_Point_2Args
    {
        public int X { get; }
        public int Y { get; }

        public Parameterized_Point_2Args(int x, int y) => (X, Y) = (x, y);
    }

    public class Parameterless_ClassWithPrimitives
    {
        public int FirstInt { get; set; }
        public int SecondInt { get; set; }

        public string FirstString { get; set; }
        public string SecondString { get; set; }

        public DateTime FirstDateTime { get; set; }
        public DateTime SecondDateTime { get; set; }

        public int X { get; set;  }
        public int Y { get; set;  }
        public int Z { get; set; }

        public int ThirdInt { get; set; }
        public int FourthInt { get; set; }

        public string ThirdString { get; set; }
        public string FourthString { get; set; }

        public DateTime ThirdDateTime { get; set; }
        public DateTime FourthDateTime { get; set; }
    }

    public class Parameterized_ClassWithPrimitives_3Args
    {
        public int FirstInt { get; set; }
        public int SecondInt { get; set; }

        public string FirstString { get; set; }
        public string SecondString { get; set; }

        public DateTime FirstDateTime { get; set; }
        public DateTime SecondDateTime { get; set; }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public int ThirdInt { get; set; }
        public int FourthInt { get; set; }

        public string ThirdString { get; set; }
        public string FourthString { get; set; }

        public DateTime ThirdDateTime { get; set; }
        public DateTime FourthDateTime { get; set; }


        public Parameterized_ClassWithPrimitives_3Args(int x, int y, int z) => (X, Y, Z) = (x, y, z);
    }

    public class Parameterized_ClassWithPrimitives_8Args
    {
        public int FirstInt { get; set; }
        public int SecondInt { get; set; }

        public string FirstString { get; set; }
        public string SecondString { get; set; }

        public DateTime FirstDateTime { get; }
        public DateTime SecondDateTime { get; }
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public int ThirdInt { get; }
        public int FourthInt { get; }
        public string ThirdString { get; }
        
        public string FourthString { get; set; }

        public DateTime ThirdDateTime { get; set; }
        public DateTime FourthDateTime { get; set; }


        public Parameterized_ClassWithPrimitives_8Args(
            DateTime firstDateTime,
            DateTime secondDateTime,
            int x,
            int y,
            int z,
            int thirdInt,
            int fourthInt,
            string thirdString)
        {
            FirstDateTime = firstDateTime;
            SecondDateTime = secondDateTime;
            X = x;
            Y = y;
            Z = z;
            ThirdInt = thirdInt;
            FourthInt = fourthInt;
            ThirdString = thirdString;
        }
    }

    public class Parameterized_IndexViewModel_1Arg
    {
        public List<ActiveOrUpcomingEvent> ActiveOrUpcomingEvents { get; set; }
        public CampaignSummaryViewModel FeaturedCampaign { get; set; }
        public bool IsNewAccount { get; set; }
        public bool HasFeaturedCampaign => FeaturedCampaign != null;

        public Parameterized_IndexViewModel_1Arg(bool isNewAccount)
        {
            IsNewAccount = isNewAccount;
        }
    }

    public class Parameterized_IndexViewModel_2Args
    {
        public List<ActiveOrUpcomingEvent> ActiveOrUpcomingEvents { get; set; }
        public CampaignSummaryViewModel FeaturedCampaign { get; }
        public bool IsNewAccount { get; }
        public bool HasFeaturedCampaign => FeaturedCampaign != null;

        public Parameterized_IndexViewModel_2Args(CampaignSummaryViewModel featuredCampaign, bool isNewAccount)
        {
            FeaturedCampaign = featuredCampaign;
            IsNewAccount = isNewAccount;
        }
    }

    public class ClassImplementingIXmlSerialiable : IXmlSerializable
    {
        public string StringValue { get; set; }
        private bool BoolValue { get; set; }

        public ClassImplementingIXmlSerialiable() => BoolValue = true;

        public System.Xml.Schema.XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            StringValue = reader.GetAttribute("StringValue");
            BoolValue = bool.Parse(reader.GetAttribute("BoolValue"));
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("StringValue", StringValue);
            writer.WriteAttributeString("BoolValue", BoolValue.ToString());
        }
    }
}