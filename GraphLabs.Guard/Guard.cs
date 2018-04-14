using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Guard
{
    public static class Guard
    {

        private sealed class ContractException : Exception
        {
            public ContractException(string description, string nameOfMethod) : base(FormatErrorMessage(description, nameofMethod)) { }

            private static string FormatErrorMessage(string description, string nameOfMethod)
            {
                return ($"{description} in method {nameOfMethod}.");
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotNull<T>(
        string nameOfMethod,
        T argument)
        where T : class
        {
            if (argument == null)
            {
                throw new ContractException("Argument shoud be not null ", nameOfMethod);
            }
        }

        //Resharper.Annotations
        [Conditional("DEBUG")]
        public static void AreAssignedTypes(
        string nameOfMethod,
        Type defaultType,
        Type type
        )
        {
            if (!typeof(defaultType).IsAssignableFrom(type))
            {
                throw new ContractException("Specified type can not be assigned to the current type ", nameOfMethod);
            }
        }

        [Conditional("DEBUG")]
        public static void AreEqualTypes(
        string nameOfMethod,
        Type defaultType,
        Type type
        )
        {
            if (!typeof(defaultType).IsAssignableFrom(type))
            {
                throw new ContractException("Specified type is not equal to the current type ", nameOfMethod);
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotWhiteSpace(
            string nameOfMethod,
            string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ContractException("String should not be null or white space ", nameOfMethod);
            }
        }

        [Conditional("DEBUG")]
        public static void IsPositiveLong(
            string nameOfMethod,
            long num)
        {
            if (num <= 0)
            {
                throw new ContractException("Long must be positive ", nameOfMethod);
            }
        }

        [Conditional("DEBUG")]
        public static void IsPositiveInt(
            string nameOfMethod,
            int num)
        {
            if (num <= 0)
            {
                throw new ContractException("Int must be positive ", nameOfMethod);
            }
        }

        [Conditional("DEBUG")]
        public static void ComparingDates(
            string nameOfMethod,
            DateTime newDate,
            DateTime currentDate
            )
        {
            if (newDate < currentDate)
            {
                throw new ContractException("New date should be greater than current date ", nameOfMethod);
            }
        }


        [Conditional("DEBUG")]
        public static void IsCorrectIP( 
            string nameOfMethod,
            string ip)
        {
            if (!IpHelper.CheckIsValidIP(ip))
            {
                throw new ContractException("The current ip is not correct ", nameOfMethod;
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotEmpty<T>( // и для строки
            string nameOfMethod,
            T argument)
        {
            if (argument == T.Empty)
            {
                throw new ContractException("Argumen shoud not be empty ", nameOfMethod);
            }
        }

        //roles.Any() используется только один раз












    }

}
