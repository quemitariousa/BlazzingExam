using System;

namespace BlazzingExam.Core.Generators
{
    public static class NameGenerator
    {
        public static string GenerateUniqueCode() => Guid.NewGuid().ToString().Replace("-", "");
    }
}