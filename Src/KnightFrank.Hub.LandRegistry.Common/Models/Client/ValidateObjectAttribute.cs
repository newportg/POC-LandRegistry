using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client
{
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null);

            Validator.TryValidateObject(value, context, results, true);

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }

    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }

    public static class ValidationResults
    {
        public static string PrintResults(IEnumerable<ValidationResult> results, Int32 indentationLevel)
        {
            string result = string.Empty;

            foreach (var validationResult in results)
            {
                indentationLevel += SetIndentation(indentationLevel);

                result += validationResult.ErrorMessage + ": ";
                if (validationResult.MemberNames != null)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        result += memberName + "\t";
                    }
                }
                result += "\n";

                Console.WriteLine(validationResult.ErrorMessage);
                if (validationResult is CompositeValidationResult)
                {
                    result += PrintResults(((CompositeValidationResult)validationResult).Results, indentationLevel + 1);
                }
            }
            return result;
        }

        public static int SetIndentation(int indentationLevel)
        {
            return indentationLevel * 4;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OnlyOnePropertyAttribute : ValidationAttribute
    {
        private string[] PropertyList { get; set; }

        public OnlyOnePropertyAttribute(params string[] propertyList)
        {
            this.PropertyList = propertyList;
        }

        //See http://stackoverflow.com/a/1365669
        //public override object TypeId
        //{
        //    get
        //    {
        //        return this;
        //    }
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo propertyInfo;
            var existMembers = new List<string>();

            foreach (var property in PropertyList)
            {
                propertyInfo = value.GetType().GetProperty(property);
                if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
                {
                    existMembers.Add(propertyInfo.Name);
                }
            }

            if (existMembers.Count == 0 || existMembers.Count > 1)
                return new ValidationResult(this.ErrorMessage, existMembers);

            return ValidationResult.Success;
        }

        public override bool IsValid(object value)
        {
            PropertyInfo propertyInfo;
            foreach (string propertyName in PropertyList)
            {
                propertyInfo = value.GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
