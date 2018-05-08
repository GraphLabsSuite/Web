using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;

namespace GraphLabs.Guard
{
    public static class Guard
    {

        private sealed class ContractException : Exception
        {
            public ContractException(string description, string memberName, string filePath, int lineNumber ) : base(FormatErrorMessage(description, memberName, filePath, lineNumber)) { }

            private static string FormatErrorMessage(string description, string memberName, string filePath, int lineNumber )
            {
                return ($"{description} in method {memberName}, source file path: {filePath}, line {lineNumber}.");
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotNull<T>(
        T argument,
        [InvokerParameterName]string argName,
        [CallerMemberName]string memberName = null,
        [CallerFilePath]string filePath = null,
        [CallerLineNumber]int lineNumber = 0
        )
        where T : class
        {
            if (argument == null)
            {
                throw new ContractException($"Argument {argName} shoud be not null ", memberName, filePath, lineNumber);
            }
        }

        [Conditional("DEBUG")]
        public static void AreAssignedTypes(
        Type defaultType,
        Type type,
        [CallerMemberName]string memberName = null,
        [CallerFilePath]string filePath = null,
        [CallerLineNumber]int lineNumber = 0
        )
        {
            if (!defaultType.IsAssignableFrom(type))
            {
                throw new ContractException($"Type should be assigned to the default type", memberName, filePath, lineNumber);
            }
        }

        [Conditional("DEBUG")]
        public static void AreEqual<T>(
        T a, 
        T b,
        [InvokerParameterName]string argName1,
        [InvokerParameterName]string argName2,
        [CallerMemberName]string memberName = null,
        [CallerFilePath]string filePath = null,
        [CallerLineNumber]int lineNumber = 0
        )
        {
            if (!Equals(a, b))
            {
                throw new ContractException($"{argName1} should be equal to the {argName2} ", memberName, filePath, lineNumber);
            }
        }


        [Conditional("DEBUG")]
        public static void IsNotNullOrWhiteSpace(
            string argument,
            [CallerMemberName]string memberName = null,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int lineNumber = 0
            )
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ContractException($"String argument should not be null or white space ", memberName, filePath, lineNumber);
            }
        }

        [Conditional("DEBUG")]
        public static void IsPositive(
            long num,
            [InvokerParameterName]string argName,
            [CallerMemberName]string memberName = null,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int lineNumber = 0    
            )
        {
            if (num <= 0)
            {
                throw new ContractException($"Long argument {argName} should be positive ", memberName, filePath, lineNumber);
            }
        }

        [Conditional("DEBUG")]
        public static void IsPositive(
            int num,
            [InvokerParameterName]string argName,
            [CallerMemberName]string memberName = null,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int lineNumber = 0
            )
        {
            if (num <= 0)
            {
                throw new ContractException($"Int argument {argName} should be positive ", memberName, filePath, lineNumber);
            }
        }

        [Conditional("DEBUG")]
        public static void IsNotEmpty(
            string argument,
            [InvokerParameterName]string argName, 
            [CallerMemberName]string memberName = null,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int lineNumber = 0
            )
        {
            if (argument == string.Empty)
            {
                throw new ContractException($"String {argName} shoud not be empty ", memberName, filePath, lineNumber);
            }
        }

        [Conditional("DEBUG")]
        public static void IsTrueAssertion(
            bool argument,
            [NotNull] string message,
            [CallerMemberName]string memberName = null,
            [CallerFilePath]string filePath = null,
            [CallerLineNumber]int lineNumber = 0)
        {
            if (argument == false)
            {
                throw new ContractException(message, memberName, filePath, lineNumber);
            }
        }

        [Conditional("DEBUG")]
        public static void IsTrueAssertion(
          bool argument,
          [CallerMemberName]string memberName = null,
          [CallerFilePath]string filePath = null,
          [CallerLineNumber]int lineNumber = 0)
        {
            if (argument == false)
            {
                throw new ContractException("Assertion should be true", memberName, filePath, lineNumber);
            }
        }

    }

}
