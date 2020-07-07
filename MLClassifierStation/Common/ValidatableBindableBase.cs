using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MLClassifierStation.Common
{
    // https://www.eidias.com/blog/2013/7/13/utilizing-inotifydataerrorinfo-in-wpf-mvvm-app
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        private object threadLock = new object();
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event EventHandler<ChildErrorsChangedEventArgs> ChildErrorsChanged;

        protected List<ValidatableBindableBase> childViewModels = new List<ValidatableBindableBase>();
        protected ValidatableBindableBase parentViewModel;

        private ValidationContext validationContext;

        public ValidationContext ValidationContext
        {
            get
            {
                if (validationContext == null)
                    validationContext = new ValidationContext(this);

                return validationContext;
            }
            set => validationContext = value;
        }

        public bool IsValid => !HasErrors;

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void OnChildErrorsChanged(string propertyName, List<string> childErrors)
        {
            if (childErrors.Count > 0)
            {
                //clear previous errors from tested property
                if (errors.ContainsKey(propertyName))
                    errors.Remove(propertyName);

                errors.Add(propertyName, childErrors);
            }

            ChildErrorsChanged?.Invoke(this, new ChildErrorsChangedEventArgs(propertyName, childErrors));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                if (errors.ContainsKey(propertyName) && (errors[propertyName] != null) && errors[propertyName].Count > 0)
                    return errors[propertyName].ToList();
                else
                    return null;
            }
            else
                return errors.SelectMany(err => err.Value.ToList());
        }

        public bool HasErrors => errors.Any(propErrors => propErrors.Value != null 
                                            && propErrors.Value.Count > 0);

        // https://www.tutorialspoint.com/mvvm/mvvm_validations.htm
        protected override bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            bool isSet = base.SetProperty(ref member, val, propertyName);
            ValidateProperty(val, propertyName);
            return isSet;
        }

        public void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
        {
            lock (threadLock)
            {
                ValidationContext.MemberName = propertyName;
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateProperty(value, ValidationContext, validationResults);

                //clear previous errors from tested property
                if (errors.ContainsKey(propertyName))
                    errors.Remove(propertyName);
                OnErrorsChanged(propertyName);

                HandleValidationResults(validationResults);
            }
        }

        public virtual void Validate()
        {
            lock (threadLock)
            {
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, ValidationContext, validationResults, true);

                //clear all previous errors
                var propNames = errors.Keys.ToList();
                errors.Clear();
                propNames.ForEach(pn => OnErrorsChanged(pn));

                HandleValidationResults(validationResults);
            }

            foreach (ValidatableBindableBase childViewModel in childViewModels)
                childViewModel.Validate();
        }

        private void HandleValidationResults(List<ValidationResult> validationResults)
        {
            //Group validation results by property names
            var resultsByPropNames = from res in validationResults
                                     from mname in res.MemberNames
                                     group res by mname into g
                                     select g;

            //add errors to dictionary and inform  binding engine about errors
            foreach (var prop in resultsByPropNames)
            {
                var messages = prop.Select(r => r.ErrorMessage).ToList();
                errors.Add(prop.Key, messages);
                OnErrorsChanged(prop.Key);
            }
        }

        protected virtual void NotifyParentChildErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            IEnumerable errors = GetErrors(null);
            List<string> errorsList = new List<string>();
            foreach (var error in errors)
                errorsList.Add(error.ToString());

            parentViewModel.OnChildErrorsChanged(e.PropertyName, errorsList);
        }
    }

    public class ChildErrorsChangedEventArgs
    {
        public string PropertyName { get; private set; }
        public List<string> Errors { get; private set; }

        public ChildErrorsChangedEventArgs(string propertyName, List<string> errors)
        {
            PropertyName = propertyName;
            Errors = errors;
        }
    }
}