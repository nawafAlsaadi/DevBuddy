using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoCRUD.Domain.Models;
using Bogus;
using Bogus.DataSets;

namespace AutoCRUD.Application.Services.StringBuilderService.Faker
{
    public static class FakerHelper
    {
     
        public static StringBuilder GetRolesForProperties(List<PropertyInfo> properties, List<string> fakersClasses, Bogus.Faker faker)
        {
            var sb = new StringBuilder();
            if (properties.Any())
                sb.AppendLine();

            foreach (var property in properties)
            {
                if (fakersClasses.Contains(property.Name))
                {
                    //continue;
                    sb.AppendLine(@$"        RuleFor(x => x.{property.Name}, f => new {property.Name}Faker(""ar"").Generate());");
                }
                else
                {
                    var methodFaker = GetFakerForProperty(property, faker);
                    if (!string.IsNullOrEmpty(methodFaker))
                    {
                        sb.AppendLine($"        RuleFor(x => x.{property.Name}, f => {methodFaker});");
                    }
                }
            }
            return sb;
        }


        private static Type? GetCollectionElementType(Type type)
        {
            if (type.IsGenericType) return type.GetGenericArguments()[0];
            if (type.IsArray) return type.GetElementType();
            return null;
        }

        private static bool IsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        private static string GetFakerForProperty(PropertyInfo property, Bogus.Faker faker)
        {
            if (HandelAbnormalCases.IsIdKey(property.Name))
            {
                return "Guid.NewGuid().ToString()";
            }

            if (HandelAbnormalCases.IsForeignKey(property.Name))
            {
                return string.Empty;
            }

            if (property.Type != null)
            {
                var isCollection = IsCollection(property.Type);
                var propertyType = isCollection ? GetCollectionElementType(property.Type) : property.Type;

                return property.Type.Name.ToLower().Contains("string")
                    ? FakerHelper.GenerateStringBasedOnName(property.Name)
                    : FakerHelper.GetFakerMethodViaType(propertyType!.Name, property.Name, faker, isCollection).ToString();
            }

            return "default";
        }
        public static string GetFakerMethodViaType(string propertyType, string propertyName, Bogus.Faker f, bool IsCollection = false)
        {
            propertyType = propertyType.ToLower();
            string method;
            string forCollection = string.Empty;
            if (IsCollection)
            {
                forCollection = "f.Make(5, () =>";
            }
            method = propertyType.ToLower() switch
            {
                var type when type.Contains("boolean") || type.Contains("bool") => "f.Random.Bool()",
                var type when type.Contains("datetime") => "f.Date.Recent(3)",
                var type when type.Contains("int32") || type.Contains("int") =>
                 HandelAbnormalCases.HandelIntId(propertyType, propertyName),
                var type when type.Contains("decimal") => "f.Random.Decimal(0.00m, 10000.00m)",
                var type when type.Contains("double") => "f.Random.Double(0.00, 10000.00)",
                var type when type.Contains("float") || type.Contains("single") => "f.Random.Float(0.00f, 10000.00f)",
                var type when type.Contains("byte") => "f.Random.Byte(0, 255)",
                var type when type.Contains("sbyte") => "f.Random.SByte(sbyte.MinValue, sbyte.MaxValue)",
                var type when type.Contains("long") => "f.Random.Long(long.MinValue, long.MaxValue)",
                var type when type.Contains("ulong") => "f.Random.ULong(ulong.MinValue, ulong.MaxValue)",
                var type when type.Contains("short") => "f.Random.Short(short.MinValue, short.MaxValue)",
                var type when type.Contains("ushort") => "f.Random.UShort(ushort.MinValue, ushort.MaxValue)",
                var type when type.Contains("guid") => "f => Guid.NewGuid().ToString())",
                var type when type.Contains("timespan") => "f.Date.Timespan()",
                var type when type.Contains("char") => " f.Random.Char('A', 'Z')",
                var type when type.Contains("char") => "f.Random.Char('A', 'Z')",
                //var type when type.Contains("Enum") => "f.PickRandom(new[] { "Option1", "Option2", "Option3", "Option4" })",
                var type when type.Contains("dateonly") => "DateOnly.FromDateTime(f.Date.Recent(3))", // Newer .NET type
                var type when type.Contains("timeonly") => "TimeOnly.FromDateTime(f.Date.Recent(3))", // Newer .NET type
                var type when type.Contains("uint32") || type.Contains("uint") => "f.Random.UInt(0, 1000)",
                var type when type.Contains("ulong") => "f.Random.ULong(0, ulong.MaxValue)",
                var type when type.Contains("ushort") => "f.Random.UShort(0, ushort.MaxValue)",
                var type when type.Contains("nuint") => "f.Random.UInt(0, uint.MaxValue)", // Native unsigned integer
                //var type when type.Contains("string") => fHelper.GenerateStringBasedOnName(property.Name, f)!,

                _ => ""
                // Default case for unsupported types
            };
            if (IsCollection) return forCollection + method + ")";
            return method;
        }
        public static string GenerateStringBasedOnName(string propertyName)
        {

            string type = GetFakerTypeFromPropertyName(propertyName);
            //var faker = new Bogus.Faker();
            return type switch
            {
                "Email" => "f.Internet.Email()",
                "PhoneNumber" => "f.Phone.PhoneNumber()",
                "Name" => "f.Name.FullName()",
                "Password" => "f.Internet.Password()",
                "DateOfBirth" => "f.Date.Past(40, DateTime.Now.AddYears(-18)).ToShortDateString()",
                "Gender" => "f.PickRandom(new[] { \"Male\", \"Female\", \"Non-Binary\" })",
                "Profile" => "f.Lorem.Paragraph()",

                // Job and Work
                "Job" => "f.Name.JobTitle()",
                "Department" => "f.Commerce.Department()",
                "Company" => "f.Company.CompanyName()",

                // Address and Location
                "Address" => "f.Address.FullAddress()",
                "City" => "f.Address.City()",
                "GeoCoordinates" => $"{{f.Address.Latitude()}}, {{f.Address.Longitude()}}",

                // Finance
                "Finance" => "f.Finance.Amount(500, 5000).ToString(\"C\")",
                "CreditCard" => "f.Finance.CreditCardNumber()",

                // E-Commerce
                "Product" => "f.Commerce.ProductName()",
                "Order" => "f.Random.Guid().ToString()",
                //"Discount" => "f.Random.Int(5, 50)%",
                "Discount" => "f.Random.Int(5, 50) + \"%\"",

                // Internet and Digital
                "Url" => "f.Internet.Url()",
                "Image" => "f.Internet.Avatar()",
                "IpAddress" => "f.Internet.Ip()",
                "Domain" => "f.Internet.DomainName()",
                "Username" => "f.Internet.UserName()",

                // Time and Date
                "Date" => "f.Date.Past(5).ToShortDateString()",
                "Time" => "f.Date.Soon().ToShortTimeString()",

                // Numbers and Identifiers
                "Number" => " f.Random.Int(1, 1000000)",
                "Guid" => "Guid.NewGuid().ToString()",

                // Vehicle and Transport
                "Vehicle" => " f.Vehicle.Model()",
                "LicensePlate" => "f.Vehicle.Vin()",

                // Miscellaneous
                "Color" => " f.Commerce.Color()",
                //"Enum" => f.PickRandom(new[] { "Option1", "Option2", "Option3", "Option4" }),
                "Lorem" => "f.Lorem.Sentence()",
                "Review" => " f.Rant.Review()",

                // Media and Files
                "File" => "f.System.FileName()",
                "Video" => "f.Internet.UrlWithPath()",
                "Audio" => "f.Internet.Url()",

                // Education and Learning
                "Course" => "f.Company.Bs()",
                "Grade" => "f.Random.Double(2.0, 4.0).ToString(\"F2\")", // Simulating GPA or similar

                _ => "f.Lorem.Sentence()"
            };
        }

        private static string GetFakerTypeFromPropertyName(string propertyName)
        {
            propertyName = propertyName.ToLowerInvariant().Replace("_", "").Replace("-", "").Trim();

            // User and Personal Information
            if (Regex.IsMatch(propertyName, @"(email|mail|contactemail|useremail|officialemail)"))
                return "Email";
            if (Regex.IsMatch(propertyName, @"(phone|mobile|contact|phonenumber|telephone|cell|userphone)"))
                return "PhoneNumber";
            if (Regex.IsMatch(propertyName, @"(name|firstname|lastname|middlename|fullname|nickname|username|displayname|accountname|realname)"))
                return "Name";
            if (Regex.IsMatch(propertyName, @"(password|security|pin|accesscode|authcode|passphrase|secret)"))
                return "Password";
            if (Regex.IsMatch(propertyName, @"(dob|birth|birthday|birthdate|age|dateofbirth|yearofbirth)"))
                return "DateOfBirth";
            if (Regex.IsMatch(propertyName, @"(gender|sex|male|female|othergender)"))
                return "Gender";
            if (Regex.IsMatch(propertyName, @"(profile|bio|about|introduction|summary|details)"))
                return "Profile";

            // Job and Work
            if (Regex.IsMatch(propertyName, @"(job|title|designation|role|position|descriptor|occupation|specialization|career)"))
                return "Job";
            if (Regex.IsMatch(propertyName, @"(department|team|division|unit|section|branch|workspace)"))
                return "Department";
            if (Regex.IsMatch(propertyName, @"(company|organization|corp|business|employer|startup|enterprise|firm|agency)"))
                return "Company";

            // Address and Location
            if (Regex.IsMatch(propertyName, @"(address|street|building|apartment|suite|postal|zip|area|location|coordinates|latitude|longitude|region|map)"))
                return "Address";
            if (Regex.IsMatch(propertyName, @"(city|town|village|state|province|region|country|district|municipality)"))
                return "City";
            if (Regex.IsMatch(propertyName, @"(latitude|longitude|lat|lng|coordinates|geo|geopoint)"))
                return "GeoCoordinates";

            // Finance
            if (Regex.IsMatch(propertyName, @"(price|amount|cost|value|salary|income|expense|revenue|budget|profit|loss|payment|fee|rate|wage|tax)"))
                return "Finance";
            if (Regex.IsMatch(propertyName, @"(creditcard|cardnumber|iban|accountnumber|bank|transaction|paymentid|account)"))
                return "CreditCard";

            // E-Commerce
            if (Regex.IsMatch(propertyName, @"(product|item|sku|brand|manufacturer|category|inventory|stock|goods|catalog|lineitem)"))
                return "Product";
            if (Regex.IsMatch(propertyName, @"(order|purchase|transactionid|sale|invoice|receipt|shipment|trackingnumber|delivery)"))
                return "Order";
            if (Regex.IsMatch(propertyName, @"(discount|coupon|promo|offer|voucher|code|redeem|deal)"))
                return "Discount";

            // Internet and Digital
            if (Regex.IsMatch(propertyName, @"(url|link|website|webpage|homepage|webaddress|endpoint|hyperlink)"))
                return "Url";
            if (Regex.IsMatch(propertyName, @"(image|photo|avatar|picture|profilepic|logo|icon|thumbnail|snapshot|gallery)"))
                return "Image";
            if (Regex.IsMatch(propertyName, @"(ip|ipv4|ipv6|hostname|server|hostaddress|address|machineip|network)"))
                return "IpAddress";
            if (Regex.IsMatch(propertyName, @"(domain|host|subdomain|webdomain|hostedzone)"))
                return "Domain";
            if (Regex.IsMatch(propertyName, @"(username|accountname|handle|userid|nickname)"))
                return "Username";

            // Time and Date
            if (Regex.IsMatch(propertyName, @"(date|day|month|year|timestamp|created|updated|joined|registered|expiration|expiry|duedate|timeframe)"))
                return "Date";
            if (Regex.IsMatch(propertyName, @"(time|hour|minute|second|timeofday|schedule|clock|duration|period|timer|timezone)"))
                return "Time";

            // Numbers and Identifiers
            if (Regex.IsMatch(propertyName, @"(number|identifier|code|key|serial|index|count|quantity|batch|sequence|reference)"))
                return "Number";
            if (Regex.IsMatch(propertyName, @"(id|guid|uuid|uniqueid|token|referenceid|idcode)"))
                return "Guid";

            // Vehicle and Transport
            if (Regex.IsMatch(propertyName, @"(vin|license|plate|vehicle|registration|chassis|model|manufacturer|engine|make)"))
                return "Vehicle";

            // Miscellaneous
            if (Regex.IsMatch(propertyName, @"(color|shade|hue|tone|palette|tint)"))
                return "Color";
            if (Regex.IsMatch(propertyName, @"(enum|type|category|option|classification|group|kind|status|state)"))
                return "Enum";
            if (Regex.IsMatch(propertyName, @"(description|details|info|text|content|summary|note|message|comments|remarks|overview)"))
                return "Lorem";
            if (Regex.IsMatch(propertyName, @"(review|feedback|testimonial|opinion|critique|evaluation)"))
                return "Review";

            // Media and Files
            if (Regex.IsMatch(propertyName, @"(file|filename|filepath|document|pdf|attachment|report|spreadsheet|sheet)"))
                return "File";
            if (Regex.IsMatch(propertyName, @"(video|movie|clip|stream|recording|footage)"))
                return "Video";
            if (Regex.IsMatch(propertyName, @"(audio|sound|voice|recording|song|podcast)"))
                return "Audio";

            // Education and Learning
            if (Regex.IsMatch(propertyName, @"(course|class|module|lesson|program|subject|curriculum)"))
                return "Course";
            if (Regex.IsMatch(propertyName, @"(grade|mark|score|gpa|result|assessment|evaluation)"))
                return "Grade";

            // Catch-all fallback
            return "Lorem";
        }
        public static List<Dictionary<string, object>> GetRandomData(Bogus.Faker faker, List<PropertyInfo> properties, int Max)
        { // this will removed
            var randomDataList = new List<Dictionary<string, object>>();
            for (int i = 0; i < Max; i++)
            {
                var randomData = new Dictionary<string, object>();
                foreach (var property in properties)
                {
                    if (HandelAbnormalCases.HandleIdProperty(property, i, randomData)) continue;

                    // Handle foreign key properties
                    if (HandelAbnormalCases.HandleForeignKeyProperty(property, faker, randomData, Max)) continue;

                    var GetMethodFaker = GetFakerMethodViaType(property.Type?.ToString(), property.Name, faker);

                    if (GetMethodFaker != null)
                        randomData[property.Name] = GetMethodFaker;

                    else
                        randomData[property.Name] = GetFakerMethodViaType(property.Type.ToString(), property.Name, faker);

                    if (String.IsNullOrEmpty(randomData[property.Name].ToString()))
                    {
                        randomData.Remove(property.Name);
                    }
                }
                randomDataList.Add(randomData);

            }
            return randomDataList;
        }

    }


}

