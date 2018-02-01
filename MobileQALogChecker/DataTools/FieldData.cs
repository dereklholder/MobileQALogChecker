using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MobileQALogChecker.DataTools
{
    public class FieldDataViewModel
    {
        public ObservableCollection<Field> Fields { get; set; }
        public FieldDataViewModel()
        {
            
            Fields = new ObservableCollection<Field>()
            {
                new Field("stuff", true)
            };          
        }
        
    }

    public class Field
    {
        
        public Field(string field, bool toValidate)
        {
            _field = field;
            _toValidate = toValidate;
        }
        private string _field {get; set;}
        private bool _toValidate {get; set;}
        
        public string Name
        {
            get { return _field; }
            private set { }
        }
        public bool ToValidate
        {
            get { return _toValidate; }
            set { _toValidate; }
        }
    }

}

